using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Service
{
    public sealed class StationeryRequestService
    {
        #region Singleton Design Pattern

        private static readonly StationeryRequestService instance = new StationeryRequestService();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static StationeryRequestService()
        {
        }

        private StationeryRequestService()
        {
        }

        public static StationeryRequestService Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion


        LogicDB context = new LogicDB();

        public List<RequestByItemViewModel> GetListRequestByItem()
        {
            List<RequestByItemViewModel> model = new List<RequestByItemViewModel>();
            var query = context.RequestByItemView.OrderBy(x => x.ItemId).ToList();
            foreach (var i in query)
            {
                var item = model.Find(x => x.ItemId == i.ItemId);
                var disb = context.DisbByDept.Where(x => x.ItemId == i.ItemId && x.DepartmentId == i.DepartmentId).FirstOrDefault();
                if (item != null)
                {
                    var newModel = new BreakdownByDeptViewModel
                    {
                        DepartmentId = i.DepartmentId,
                        DepartmentName = i.DepartmentName,
                        Quantity = disb == null ? (int)i.Quantity : ((int)i.Quantity - (int)disb.Quantity)
                    };
                    if (newModel.Quantity > 0)
                        item.requestList.Add(newModel);
                }
                else
                {
                    RequestByItemViewModel requestByItemViewModel = new RequestByItemViewModel();
                    requestByItemViewModel.ItemId = i.ItemId;
                    requestByItemViewModel.Description = i.Description;
                    requestByItemViewModel.requestList = new List<BreakdownByDeptViewModel>();
                    var newModel = new BreakdownByDeptViewModel
                    {
                        DepartmentId = i.DepartmentId,
                        DepartmentName = i.DepartmentName,
                        Quantity = disb == null ? (int)i.Quantity : ((int)i.Quantity - (int)disb.Quantity)
                    };
                    if (newModel.Quantity > 0)
                    {
                        requestByItemViewModel.requestList.Add(newModel);
                        model.Add(requestByItemViewModel);
                    }
                }
            }
            return model;
        }

        public string GetNewRetrievalId()
        {
            string rid;
            var ret = context.StationeryRetrieval.OrderByDescending(x => x.Date).OrderByDescending(x => x.RetrievalId).FirstOrDefault();
            if (ret.Date.Year == DateTime.Now.Year)
            {
                rid = "R" + DateTime.Now.Year.ToString() + "-" + (Convert.ToInt32(ret.RetrievalId.Substring(6, 4)) + 1).ToString("0000");
            }
            else
            {
                rid = "R" + DateTime.Now.Year.ToString() + "-" + "0001";
            }
            return rid;
        }

        //Convert breakdown of stationery by items to breakdown of stationery by dept
        public List<DisbursementByDeptViewModel> GenerateDisbursement(List<RequestByItemViewModel> model)
        {
            List<DisbursementByDeptViewModel> disbList = new List<DisbursementByDeptViewModel>();

            for (int i = 0; i < model.Count; i++)
            {
                for (int j = 0; j < model[i].requestList.Count; j++)
                {
                    var disb = disbList.Find(x => x.DepartmentId == model[i].requestList[j].DepartmentId);
                    if (disb != null)
                    {
                        var item = disb.requestList.Find(x => x.ItemId == model[i].ItemId);
                        if (item != null)
                        {
                            item.Quantity += model[i].requestList[j].Quantity;
                        }
                        else
                        {
                            if (model[i].requestList[j].RetrievedQty > 0)
                            {
                                BreakdownByItemViewModel breakdown = new BreakdownByItemViewModel();
                                breakdown.ItemId = model[i].ItemId;
                                breakdown.Description = model[i].Description;
                                breakdown.RetrievedQty = model[i].requestList[j].RetrievedQty;
                                disb.requestList.Add(breakdown);
                            }
                        }
                    }
                    else
                    {
                        if (model[i].requestList[j].RetrievedQty > 0)
                        {
                            DisbursementByDeptViewModel disbModel = new DisbursementByDeptViewModel();
                            disbModel.DepartmentId = model[i].requestList[j].DepartmentId;
                            disbModel.DepartmentName = model[i].requestList[j].DepartmentName;
                            BreakdownByItemViewModel breakdown = new BreakdownByItemViewModel();
                            breakdown.RetrievedQty = model[i].requestList[j].RetrievedQty;
                            breakdown.ItemId = model[i].ItemId;
                            breakdown.Description = model[i].Description;
                            disbModel.requestList = new List<BreakdownByItemViewModel>();
                            disbModel.requestList.Add(breakdown);
                            disbList.Add(disbModel);
                        }
                    }
                }
            }
            return disbList;
        }


        public bool SaveAndDisburse(List<RequestByItemViewModel> model, string userId)
        {
            using(var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    StationeryRetrieval retrieval = new StationeryRetrieval();
                    string rid = GetNewRetrievalId();
                    retrieval.RetrievalId = rid;
                    retrieval.RetrievedBy = userId;
                    retrieval.Date = DateTime.Now;

                    foreach (var sr in model)
                    {
                        int retQty = sr.requestList.Sum(x => x.RetrievedQty);
                        if (retQty > 0)
                        {
                            TransactionDetail detail = new TransactionDetail();
                            detail.ItemId = sr.ItemId;
                            detail.Quantity = retQty;
                            detail.TransactionDate = DateTime.Now;
                            detail.Remarks = "Retrieved";
                            detail.TransactionRef = rid;
                            retrieval.TransactionDetail.Add(detail);

                            //Less off from stationery

                            //var item = context.Stationery.FirstOrDefault(x => x.ItemId == sr.ItemId);
                            //if (item != null)
                            //{
                            //    item.QuantityWarehouse -= retQty;
                            //    item.QuantityTransit += retQty;
                            //}


                        }
                    }
                    context.StationeryRetrieval.Add(retrieval);
                    context.SaveChanges();

                    List<RequestByIdViewModel> requests = CreateDisbHelpers.GetRequestQuery(context).OrderBy(x => x.RequestId).ToList();
                    

                    List<DisbursementByDeptViewModel> disbList = GenerateDisbursement(model);

                    foreach (var dept in disbList)
                    {
                        string currentDeptId = dept.DepartmentId;
                        string disbNo = CreateDisbHelpers.GetNewDisbNo(context, currentDeptId);
                        string OTP;
                        do
                        {
                            Random rand = new Random();
                            OTP = rand.Next(10000).ToString("0000");
                        } while (context.Disbursement.Where(x => x.OTP == OTP).FirstOrDefault() != null);

                        var deptReqList = requests.Where(x => x.DepartmentId == dept.DepartmentId).ToList();

                        foreach (var req in deptReqList)
                        {
                            bool isComplete = true;

                            foreach (var item in req.ItemList)
                            {
                                var disbItem = dept.requestList.FirstOrDefault(x => x.ItemId == item.ItemId);
                                if (disbItem != null)
                                {
                                    if (disbItem.RetrievedQty >= item.Quantity)
                                    {
                                        disbItem.RetrievedQty -= item.Quantity;
                                    }
                                    else
                                    {

                                        item.Quantity = disbItem.RetrievedQty;
                                        disbItem.RetrievedQty = 0; //0
                                        isComplete = false;
                                    }
                                }
                            }

                            foreach (var item in req.ItemList)
                            {
                                if (item.Quantity > 0)
                                {
                                    Disbursement newDisb = new Disbursement();
                                    TransactionDetail newDetail = new TransactionDetail();
                                    newDisb.DisbursementId = CreateDisbHelpers.GetNewDisbId(context);
                                    newDisb.DisbursementNo = disbNo;
                                    newDisb.DepartmentId = currentDeptId;
                                    newDisb.DisbursedBy = userId;
                                    newDisb.Date = DateTime.Now;
                                    newDisb.RequestId = req.RequestId;
                                    newDisb.Status = "In Transit";
                                    newDisb.OTP = OTP;
                                    newDetail.ItemId = item.ItemId;
                                    newDetail.Quantity = item.Quantity;
                                    newDetail.TransactionRef = newDisb.DisbursementId;
                                    newDetail.TransactionDate = DateTime.Now;
                                    newDetail.UnitPrice = item.UnitPrice;
                                    newDetail.Remarks = "In Transit";
                                    newDisb.TransactionDetail.Add(newDetail);
                                    context.Disbursement.Add(newDisb);
                                    context.SaveChanges();

                                }
                            }

                            var currentReq = context.StationeryRequest.FirstOrDefault(x => x.RequestId == req.RequestId);
                            if (isComplete)
                            {
                                currentReq.Status = "Completed";
                            }
                            else
                            {
                                currentReq.Status = "Partially Fulfilled";
                            }
                        }
                    }

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return false;
                }
                
            }
            
        }
    }

    

    public static class CreateDisbHelpers
    {
        public static String GetNewDisbId(LogicDB context)
        {
            var disbursement = context.Disbursement.OrderByDescending(x => x.DisbursementId).First();
            string did = "DISB" + (Convert.ToInt32(disbursement.DisbursementId.Substring(4, 6)) + 1).ToString("000000");
            return did;
        }

        public static string GetNewDisbNo(LogicDB context, string DepartmentId)
        {
            var disbursement = context.Disbursement.OrderByDescending(x => x.DisbursementId).First();
            string dno = "D" + DepartmentId + (Convert.ToInt32(disbursement.DisbursementNo.Substring(5, 5)) + 1).ToString("00000");
            return dno;
        }

        public static List<RequestByIdViewModel> GetRequestQuery(LogicDB context)
        {
            List<SimpleRequestViewModel> requests = (from x in context.RequestByReqIdView
                                                select new SimpleRequestViewModel
                                                {
                                                    RequestId = x.RequestId,
                                                    DepartmentId = x.DepartmentId,
                                                    ItemId = x.ItemId,
                                                    UnitPrice = x.UnitPrice,
                                                    Quantity = x.Quantity
                                                }).ToList();

            var offsets = context.DisbByDept.ToList();
            foreach (var i in offsets)
            {
                var reqt = requests.Where(x => x.DepartmentId == i.DepartmentId && x.ItemId == i.ItemId).FirstOrDefault();
                if (reqt != null)
                {
                    if (reqt.Quantity > i.Quantity)
                    {
                        reqt.Quantity = reqt.Quantity - (int)i.Quantity;
                    }
                    else if (reqt.Quantity == i.Quantity)
                    {
                        requests.Remove(reqt);
                    }
                    else
                    {
                        i.Quantity -= reqt.Quantity;
                        while (i.Quantity > 0)
                        {
                            requests.Remove(reqt);
                            reqt = requests.Where(x => x.DepartmentId == i.DepartmentId && x.ItemId == i.ItemId).FirstOrDefault();
                            if (reqt != null)
                            {
                                if (reqt.Quantity > i.Quantity)
                                {
                                    reqt.Quantity = reqt.Quantity - (int)i.Quantity;
                                }
                                else if (reqt.Quantity == i.Quantity)
                                {
                                    requests.Remove(reqt);
                                }
                                else
                                {
                                    i.Quantity -= reqt.Quantity;
                                }
                            }
                        }
                    }
                }
            }

            List<RequestByIdViewModel> model = new List<RequestByIdViewModel>();

            foreach (var item in requests)
            {
                var exist = model.FirstOrDefault(x => x.RequestId == item.RequestId && x.DepartmentId == item.DepartmentId);
                if (exist != null)
                {
                    DeptAndItemViewModel newVM = new DeptAndItemViewModel();

                    newVM.ItemId = item.ItemId;
                    newVM.UnitPrice = item.UnitPrice;
                    newVM.Quantity = item.Quantity;

                    exist.ItemList.Add(newVM);
                }
                else
                {
                    RequestByIdViewModel newRVM = new RequestByIdViewModel();
                    newRVM.RequestId = item.RequestId;
                    newRVM.DepartmentId = item.DepartmentId;

                    List<DeptAndItemViewModel> newList = new List<DeptAndItemViewModel>();
                    DeptAndItemViewModel newVM = new DeptAndItemViewModel();
                    newVM.ItemId = item.ItemId;
                    newVM.UnitPrice = item.UnitPrice;
                    newVM.Quantity = item.Quantity;
                    newList.Add(newVM);

                    newRVM.ItemList = newList;
                    model.Add(newRVM);
                }
            }


            return model;
        }
    }
}
