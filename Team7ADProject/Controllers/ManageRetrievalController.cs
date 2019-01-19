using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    #region Sam Jing Wen

    //For SC to generate retrieval and make amendments
    public class ManageRetrievalController : Controller
    {
        // GET: ManageRetrieval
        public ActionResult Index()
        {
            List<RequestByItemViewModel> model = new List<RequestByItemViewModel>();
            LogicDB context = new LogicDB();
            var query = context.RequestByItemView.OrderBy(x => x.ItemId).ToList();
            foreach(var i in query)
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
            return View(model);
        }

        [HttpPost]
        public ActionResult GenerateDisbursement(List<RequestByItemViewModel> model)
        {
            //Create new Retrieval

            //string rid;
            LogicDB context = new LogicDB();
            //var ret = context.StationeryRetrieval.OrderByDescending(x => x.Date).FirstOrDefault();
            //if (ret.Date.Year == DateTime.Now.Year)
            //{
            //    rid = "R" + DateTime.Now.Year.ToString() + "-" + (Convert.ToInt32(ret.RetrievalId.Substring(6, 4)) + 1).ToString("0000");
            //}
            //else
            //{
            //    rid = "R" + DateTime.Now.Year.ToString() + "-" + "0001";
            //}

            //StationeryRetrieval retrieval = new StationeryRetrieval();
            //retrieval.RetrievalId = rid;
            //retrieval.RetrievedBy = User.Identity.GetUserId();
            //retrieval.Date = DateTime.Now;

            ////Save to database
            //foreach (var sr in model)
            //{
            //    if (sr.requestList.Sum(x => x.RetrievedQty) > 0)
            //    {
            //        TransactionDetail detail = new TransactionDetail();
            //        detail.ItemId = sr.ItemId;
            //        detail.Quantity = sr.requestList.Sum(x => x.RetrievedQty);
            //        detail.TransactionDate = DateTime.Now;
            //        detail.Remarks = "Retrieved";
            //        detail.TransactionRef = rid;
            //        retrieval.TransactionDetail.Add(detail);
            //    }
            //}

            //context.StationeryRetrieval.Add(retrieval);
            //context.SaveChanges();

            //Generate Disbursement

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
                            BreakdownByItemViewModel breakdown = new BreakdownByItemViewModel();
                            breakdown.ItemId = model[i].ItemId;
                            breakdown.Description = model[i].Description;
                            breakdown.RetrievedQty = model[i].requestList[j].RetrievedQty;
                            disb.requestList.Add(breakdown);
                        }
                    }
                    else
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


            //Create new disbursement

            List<DisbursementByDeptViewModel> modDisbList = new List<DisbursementByDeptViewModel>(disbList);

            var query = CreateDisbHelpers.GetRequestQuery();

            IEnumerator<RequestByReqIdView> iter = query.GetEnumerator();

            string currentReqId = "";
            string prevReqId = "";
            Disbursement newDisb = new Disbursement();
            string disbNo = "";

            while (iter.MoveNext())
            {
                RequestByReqIdView currentReq = iter.Current;

                currentReqId = currentReq.RequestId;
                string DeptId = currentReq.DepartmentId;

                if (currentReqId != prevReqId)
                {
                    context.SaveChanges();
                    newDisb = new Disbursement();
                    disbNo = CreateDisbHelpers.GetNewDisbNo(DeptId);
                }

                

                DisbursementByDeptViewModel getDeptDisb = modDisbList.Where(x => x.DepartmentId == DeptId).FirstOrDefault();
                if (getDeptDisb == null)
                    continue;
                BreakdownByItemViewModel getItem = getDeptDisb.requestList.Where(x => x.ItemId == currentReq.ItemId).FirstOrDefault();
                if (getItem == null)
                    continue;

                newDisb.DisbursementId = CreateDisbHelpers.GetNewDisbId();
                newDisb.DisbursementNo = disbNo;
                newDisb.DepartmentId = DeptId;
                newDisb.Date = DateTime.Now;
                newDisb.DisbursedBy = User.Identity.GetUserId();
                newDisb.RequestId = currentReqId;
                newDisb.Status = "In Transit";
                if (getItem.Quantity >= currentReq.Quantity)
                {
                    TransactionDetail newDetail = new TransactionDetail();
                    newDetail.ItemId = getItem.ItemId;
                    newDetail.Quantity = currentReq.Quantity;
                    newDetail.Remarks = "In Transit";
                    newDetail.TransactionRef = newDisb.DisbursementId;
                    newDetail.TransactionDate = DateTime.Now;
                    newDetail.UnitPrice = getItem.UnitPrice;
                    newDisb.TransactionDetail.Add(newDetail);

                    if (getItem.Quantity == currentReq.Quantity)
                    {
                        getDeptDisb.requestList.Remove(getItem);
                    }
                    else
                    {
                        getItem.Quantity -= currentReq.Quantity;
                    }
                    if (getDeptDisb.requestList.Count <= 0)
                    {
                        modDisbList.Remove(getDeptDisb);
                    }
                }
                else
                {
                    //Partially fulfilled
                    TransactionDetail newDetail = new TransactionDetail();
                    newDetail.ItemId = getItem.ItemId;
                    newDetail.Quantity = getItem.Quantity;
                    var getReq = context.StationeryRequest.Where(x => x.RequestId == currentReqId).FirstOrDefault();
                    getReq.Status = "Partially Fulfilled";







                }


                if (modDisbList.Count == 0)
                {
                    break;
                }

                prevReqId = currentReqId;

            }





            




            return View(disbList);
        }
        


    }
    #endregion

}