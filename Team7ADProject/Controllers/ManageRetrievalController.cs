using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.Service;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    #region Sam Jing Wen


    //For SC to generate retrieval and make amendments
    [Authorize(Roles = "Store Clerk")]
    public class ManageRetrievalController : Controller
    {
        StationeryRequestService srService = StationeryRequestService.Instance;

        // GET: ManageRetrieval
        public ActionResult Index()
        {
            return View(srService.GetListRequestByItem());
        }

        [HttpPost]
        public ActionResult GenerateDisbursement(List<RequestByItemViewModel> model, string RetrievedBy)
        {
            //Create new Retrieval
            string retrievedBy = User.Identity.GetUserId();
            List<RequestByItemViewModel> modModel = srService.SaveRetrieval(model, retrievedBy);

            //****
            LogicDB context = new LogicDB();

            //Generate Disbursement
            List<DisbursementByDeptViewModel> disbList = srService.GenerateDisbursement(modModel);


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