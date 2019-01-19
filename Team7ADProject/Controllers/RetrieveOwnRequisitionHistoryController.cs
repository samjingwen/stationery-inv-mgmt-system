using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Team7ADProject.ViewModels;
using Team7ADProject.ViewModels.Api;

//Author: Teh Li Heng 19/1/2019
//Edit requests that are still pending for approval (implemented js and ajax)

//Author: Lynn Lynn Oo
//Display list of request
namespace Team7ADProject.Controllers
{
    //For employee/department rep can view their own requisition histories
    [Authorize(Roles = "Employee,Department Representative")]
    public class RequisitionHistoryController : Controller
    {
        
        private LogicDB _context;

        public RequisitionHistoryController()
        {
            _context = new LogicDB();
        }
        #region Author:Lynn Lynn Oo || Teh Li Heng
        [Authorize(Roles = "Employee,Department Representation")]

        public ActionResult Index()
        {
            
            string userId = User.Identity.GetUserId();
            List<string> requestId = _context.StationeryRequest.Where(m => m.RequestedBy == userId).OrderByDescending(m => m.RequestDate).Select(m => m.RequestId).ToList();
            List<TransactionDetail> transactionDetails = new List<TransactionDetail>();
            foreach (string current in requestId)
            {
                List<TransactionDetail> itemsInEachRequest = _context.TransactionDetail.Where(m => m.TransactionRef == current).ToList();
                transactionDetails.AddRange(itemsInEachRequest);
            }
            List<OwnRequisitionHistoryViewModel> viewModel = new List<OwnRequisitionHistoryViewModel>();
            foreach (TransactionDetail current in transactionDetails)
            {
                OwnRequisitionHistoryViewModel tempModel = new OwnRequisitionHistoryViewModel();
                tempModel.RequestId = current.TransactionRef;
                tempModel.ItemId = current.Stationery.ItemId;
                tempModel.ItemDescription = current.Stationery.Description;
                tempModel.ItemQuantity = current.Quantity;
                tempModel.UnitOfMeasure = current.Stationery.UnitOfMeasure;
                tempModel.RequestDate = current.StationeryRequest.RequestDate;
                tempModel.Status = current.StationeryRequest.Status;
                viewModel.Add(tempModel);
            }
            return View(viewModel);
            
        }
        #endregion

        #region Author:Teh Li Heng
        [HttpPost]
        public ActionResult Save(string requestId, string itemId, int oldItemQuantity, int newItemQuantity)
        {
            
            string result = "Error! Saving Error!";
            if (String.IsNullOrWhiteSpace(requestId)||String.IsNullOrWhiteSpace(itemId)||oldItemQuantity<=0||newItemQuantity<=0)
            {
                result = "Invalid input! Kindly amend your input.";
            }

            else
            {
                TransactionDetail transactionDetail = _context.TransactionDetail.First(m =>
                    m.TransactionRef == requestId && m.ItemId == itemId && m.Quantity == oldItemQuantity);
                transactionDetail.Quantity = newItemQuantity;
                _context.SaveChanges();
                result = "Entry successfully updated!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
            
        }
        #endregion

    }
}
