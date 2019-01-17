using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
//using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Team7ADProject.ViewModels;
//Authors:Lynn Lynn Oo
namespace Team7ADProject.Controllers
{
    //For employee/department rep can view their own requisition histories
    public class RequisitionHistoryController : Controller
    {
        #region Lynn Lynn Oo
        private LogicDB _context;

        public RequisitionHistoryController()
        {
            _context = new LogicDB();
        }

        // GET: RequisitionHistory
        // To view the Requisition History
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            List<string> requestId = _context.StationeryRequest.Where(m => m.RequestedBy == userId).Select(m=>m.RequestId).ToList();
            List<TransactionDetail> transactionDetails = new List<TransactionDetail>();
            foreach(string current in requestId)
            {
                List<TransactionDetail> itemsInEachRequest = _context.TransactionDetail.Where(m => m.TransactionRef == current).ToList();
                transactionDetails.AddRange(itemsInEachRequest);
            }
            List<OwnRequisitionHistoryViewModel> viewModel = new List<OwnRequisitionHistoryViewModel>();
            foreach(TransactionDetail current in transactionDetails)
            {
                OwnRequisitionHistoryViewModel tempModel = new OwnRequisitionHistoryViewModel();
                tempModel.ItemDescription=current.Stationery.Description;
                tempModel.ItemQuantity=current.Quantity;
                tempModel.UnitOfMeasure=current.Stationery.UnitOfMeasure;
                tempModel.RequestDate=current.StationeryRequest.RequestDate;
                tempModel.Status=current.StationeryRequest.Status;
                viewModel.Add(tempModel);
            }
            return View(viewModel);
        }
        #endregion  
    }
}
