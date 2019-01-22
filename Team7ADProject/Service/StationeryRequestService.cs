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
                    item.requestList.Add(newModel);
                }
                else
                {
                    RequestByItemViewModel requestByItemViewModel = new RequestByItemViewModel();
                    requestByItemViewModel.ItemId = i.ItemId;
                    requestByItemViewModel.Description = i.Description;
                    requestByItemViewModel.requestList = new List<BreakdownByDeptViewModel>();
                    requestByItemViewModel.requestList.Add(new BreakdownByDeptViewModel
                    {
                        DepartmentId = i.DepartmentId,
                        DepartmentName = i.DepartmentName,
                        Quantity = disb == null ? (int)i.Quantity : ((int)i.Quantity - (int)disb.Quantity)
                    });
                    model.Add(requestByItemViewModel);
                }
            }
            return model;
        }

        public string GetNewRetrievalId()
        {
            string rid;
            var ret = context.StationeryRetrieval.OrderByDescending(x => x.Date).FirstOrDefault();
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

        //Save retrieval list to database
        public List<RequestByItemViewModel> SaveRetrieval(List<RequestByItemViewModel> model, string RetrievedBy)
        {
            StationeryRetrieval retrieval = new StationeryRetrieval();
            string rid = GetNewRetrievalId();
            retrieval.RetrievalId = rid;
            retrieval.RetrievedBy = RetrievedBy;
            retrieval.Date = DateTime.Now;

            List<RequestByItemViewModel> modModel = new List<RequestByItemViewModel>(model);

            foreach (var sr in model)
            {
                if (sr.requestList.Sum(x => x.RetrievedQty) > 0)
                {
                    TransactionDetail detail = new TransactionDetail();
                    detail.ItemId = sr.ItemId;
                    detail.Quantity = sr.requestList.Sum(x => x.RetrievedQty);
                    detail.TransactionDate = DateTime.Now;
                    detail.Remarks = "Retrieved";
                    detail.TransactionRef = rid;
                    retrieval.TransactionDetail.Add(detail);
                }
                else
                {
                    modModel.Remove(sr);
                }
            }
            try
            {
                context.StationeryRetrieval.Add(retrieval);
                context.SaveChanges();
                return modModel;
            }
            catch (Exception)
            {
                return null;
            }


        }

        //Convert breakdown of stationery by items to breakdown of stationery by dept
        public List<DisbursementByDeptViewModel> GenerateDisbursement(List<RequestByItemViewModel> modModel)
        {
            List<DisbursementByDeptViewModel> disbList = new List<DisbursementByDeptViewModel>();

            for (int i = 0; i < modModel.Count; i++)
            {
                for (int j = 0; j < modModel[i].requestList.Count; j++)
                {
                    var disb = disbList.Find(x => x.DepartmentId == modModel[i].requestList[j].DepartmentId);
                    if (disb != null)
                    {
                        var item = disb.requestList.Find(x => x.ItemId == modModel[i].ItemId);
                        if (item != null)
                        {
                            item.Quantity += modModel[i].requestList[j].Quantity;
                        }
                        else
                        {
                            BreakdownByItemViewModel breakdown = new BreakdownByItemViewModel();
                            breakdown.ItemId = modModel[i].ItemId;
                            breakdown.Description = modModel[i].Description;
                            breakdown.RetrievedQty = modModel[i].requestList[j].RetrievedQty;
                            disb.requestList.Add(breakdown);
                        }
                    }
                    else
                    {
                        DisbursementByDeptViewModel disbModel = new DisbursementByDeptViewModel();
                        disbModel.DepartmentId = modModel[i].requestList[j].DepartmentId;
                        disbModel.DepartmentName = modModel[i].requestList[j].DepartmentName;
                        BreakdownByItemViewModel breakdown = new BreakdownByItemViewModel();
                        breakdown.RetrievedQty = modModel[i].requestList[j].RetrievedQty;
                        breakdown.ItemId = modModel[i].ItemId;
                        breakdown.Description = modModel[i].Description;
                        disbModel.requestList = new List<BreakdownByItemViewModel>();
                        disbModel.requestList.Add(breakdown);
                        disbList.Add(disbModel);
                    }
                }
            }

            return disbList;
        }

        public void SaveDisbursement()
        {





        }
    }

    public static class CreateDisbHelpers
    {
        public static String GetNewDisbId()
        {
            LogicDB context = new LogicDB();
            var disbursement = context.Disbursement.OrderByDescending(x => x.DisbursementId).First();
            string did = "DISB" + (Convert.ToInt32(disbursement.DisbursementId.Substring(4, 6)) + 1).ToString("000000");
            return did;
        }

        public static string GetNewDisbNo(string DepartmentId)
        {
            LogicDB context = new LogicDB();
            var disbursement = context.Disbursement.OrderByDescending(x => x.DisbursementId).First();
            string dno = "D" + DepartmentId + (Convert.ToInt32(disbursement.DisbursementNo.Substring(5, 5)) + 1).ToString("00000");
            return dno;
        }

        public static List<RequestByReqIdView> GetRequestQuery()
        {
            LogicDB context = new LogicDB();
            List<RequestByReqIdView> model = context.RequestByReqIdView.ToList();
            var query = context.DisbByDept.ToList();
            foreach (var i in query)
            {
                var reqt = model.Where(x => x.DepartmentId == i.DepartmentId && x.ItemId == i.ItemId).FirstOrDefault();
                if (reqt != null)
                {
                    if (reqt.Quantity > i.Quantity)
                    {
                        reqt.Quantity = reqt.Quantity - (int)i.Quantity;
                    }
                    else if (reqt.Quantity == i.Quantity)
                    {
                        model.Remove(reqt);
                    }
                    else
                    {
                        i.Quantity -= reqt.Quantity;
                        while (i.Quantity > 0)
                        {
                            model.Remove(reqt);
                            reqt = model.Where(x => x.DepartmentId == i.DepartmentId && x.ItemId == i.ItemId).FirstOrDefault();
                            if (reqt != null)
                            {
                                if (reqt.Quantity > i.Quantity)
                                {
                                    reqt.Quantity = reqt.Quantity - (int)i.Quantity;
                                }
                                else if (reqt.Quantity == i.Quantity)
                                {
                                    model.Remove(reqt);
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
            return model;
        }
    }
}
