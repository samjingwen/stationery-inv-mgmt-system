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
            //    else
            //    {
            //        model.Remove(sr);
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

            for(int i = 0; i < disbList.Count; i++)
            {
                string currentDeptId = disbList[i].DepartmentId;
                string disbNo = CreateDisbHelpers.GetNewDisbNo(currentDeptId);
                string OTP;
                do
                {
                    Random rand = new Random();
                    OTP = rand.Next(10000).ToString("0000");
                } while (context.Disbursement.Where(x => x.OTP == OTP).FirstOrDefault() != null);
                
                for(int j = 0; j < disbList[i].requestList.Count; j++)
                {
                    var currentItem = disbList[i].requestList[j];
                    var reqt = query.OrderBy(x => x.RequestId)
                                    .Where(x => x.DepartmentId == currentDeptId 
                                    && x.ItemId == currentItem.ItemId).FirstOrDefault();
                    if (reqt != null)
                    {
                        if (currentItem.RetrievedQty < reqt.Quantity)
                        {
                            Disbursement newDisb = new Disbursement();
                            TransactionDetail newDetail = new TransactionDetail();
                            newDisb.DisbursementId = CreateDisbHelpers.GetNewDisbId();
                            newDisb.DisbursementNo = disbNo;
                            newDisb.DepartmentId = currentDeptId;
                            newDisb.DisbursedBy = User.Identity.GetUserId();
                            newDisb.Date = DateTime.Now;
                            newDisb.RequestId = reqt.RequestId;
                            newDisb.Status = "In Transit";
                            newDisb.OTP = OTP;
                            newDetail.ItemId = currentItem.ItemId;
                            newDetail.Quantity = currentItem.RetrievedQty;
                            newDetail.TransactionRef = newDisb.DisbursementId;
                            newDetail.TransactionDate = DateTime.Now;
                            newDetail.UnitPrice = currentItem.UnitPrice;
                            newDisb.TransactionDetail.Add(newDetail);
                            var statReq = context.StationeryRequest.Where(x => x.RequestId == reqt.RequestId).FirstOrDefault();
                            if (statReq != null)
                            {
                                statReq.Status = "Partially Fulfilled";
                            }
                            reqt.Quantity -= currentItem.RetrievedQty;
                            context.Disbursement.Add(newDisb);
                            context.SaveChanges();
                        }
                        else if (currentItem.RetrievedQty == reqt.Quantity)
                        {
                            Disbursement newDisb = new Disbursement();
                            TransactionDetail newDetail = new TransactionDetail();
                            newDisb.DisbursementId = CreateDisbHelpers.GetNewDisbId();
                            newDisb.DisbursementNo = disbNo;
                            newDisb.DepartmentId = currentDeptId;
                            newDisb.DisbursedBy = User.Identity.GetUserId();
                            newDisb.Date = DateTime.Now;
                            newDisb.RequestId = reqt.RequestId;
                            newDisb.Status = "In Transit";
                            newDisb.OTP = OTP;
                            newDetail.ItemId = currentItem.ItemId;
                            newDetail.Quantity = currentItem.RetrievedQty;
                            newDetail.TransactionRef = newDisb.DisbursementId;
                            newDetail.TransactionDate = DateTime.Now;
                            newDetail.UnitPrice = currentItem.UnitPrice;
                            newDisb.TransactionDetail.Add(newDetail);
                            var reqCheck = query.Where(x => x.RequestId == reqt.RequestId).ToList();
                            if (reqCheck.Count > 1)
                            {
                                var statReq = context.StationeryRequest.Where(x => x.RequestId == reqt.RequestId).FirstOrDefault();
                                if (statReq != null)
                                {
                                    statReq.Status = "Partially Fulfilled";
                                }
                            }
                            else
                            {
                                var statReq = context.StationeryRequest.Where(x => x.RequestId == reqt.RequestId).FirstOrDefault();
                                if (statReq != null)
                                {
                                    statReq.Status = "Completed";
                                }
                            }
                            query.Remove(reqt);
                            context.Disbursement.Add(newDisb);
                            context.SaveChanges();



                        }
                        else
                        {
                            Disbursement newDisb = new Disbursement();
                            TransactionDetail newDetail = new TransactionDetail();
                            newDisb.DisbursementId = CreateDisbHelpers.GetNewDisbId();
                            newDisb.DisbursementNo = disbNo;
                            newDisb.DepartmentId = currentDeptId;
                            newDisb.DisbursedBy = User.Identity.GetUserId();
                            newDisb.Date = DateTime.Now;
                            newDisb.RequestId = reqt.RequestId;
                            newDisb.Status = "In Transit";
                            newDisb.OTP = OTP;
                            newDetail.ItemId = currentItem.ItemId;
                            newDetail.Quantity = reqt.Quantity;
                            newDetail.TransactionRef = newDisb.DisbursementId;
                            newDetail.TransactionDate = DateTime.Now;
                            newDetail.UnitPrice = currentItem.UnitPrice;
                            newDisb.TransactionDetail.Add(newDetail);
                            context.Disbursement.Add(newDisb);
                            context.SaveChanges();
                            currentItem.RetrievedQty -= reqt.Quantity;
                            query.Remove(reqt);
                            while (currentItem.RetrievedQty > 0)
                            {
                                reqt = query.OrderBy(x => x.RequestId)
                                            .Where(x => x.DepartmentId == currentDeptId
                                            && x.ItemId == currentItem.ItemId).FirstOrDefault();
                                if (reqt != null)
                                {
                                    newDisb = new Disbursement();
                                    newDetail = new TransactionDetail();
                                    newDisb.DisbursementId = CreateDisbHelpers.GetNewDisbId();
                                    newDisb.DisbursementNo = disbNo;
                                    newDisb.DepartmentId = currentDeptId;
                                    newDisb.DisbursedBy = User.Identity.GetUserId();
                                    newDisb.Date = DateTime.Now;
                                    newDisb.RequestId = reqt.RequestId;
                                    newDisb.Status = "In Transit";
                                    newDisb.OTP = OTP;
                                    newDetail.ItemId = currentItem.ItemId;
                                    newDetail.Quantity = currentItem.RetrievedQty > reqt.Quantity ? reqt.Quantity : currentItem.RetrievedQty;
                                    newDetail.TransactionRef = newDisb.DisbursementId;
                                    newDetail.TransactionDate = DateTime.Now;
                                    newDetail.UnitPrice = currentItem.UnitPrice;
                                    newDisb.TransactionDetail.Add(newDetail);
                                    context.Disbursement.Add(newDisb);
                                    context.SaveChanges();
                                    if (currentItem.RetrievedQty < reqt.Quantity)
                                    {
                                        reqt.Quantity -= currentItem.RetrievedQty;
                                        break;
                                    }
                                    currentItem.RetrievedQty -= reqt.Quantity;

                                }
                                else
                                {
                                    throw new Exception("Something went wrong");
                                }



                            }
                        }









                    }





                }



            }





            return View(disbList);
        }
        


    }
    #endregion

}