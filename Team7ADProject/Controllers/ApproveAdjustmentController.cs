using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

//Author Zan Tun Khine
namespace Team7ADProject.Controllers
{


    #region Zan Tun Khine

    //For SS or SM to approve adjustment

    [Authorize(Roles = "Store Manager , Store Supervisor")]
    public class ApproveAdjustmentController : Controller
    {
        private LogicDB _context = new LogicDB();

        #region pending Stock Adjustment Requests

        // GET: ApproveAdjustment
        public ActionResult Index()
        {
            //List<StockAdjustment> adjList = _context.StockAdjustment.ToList();
            List<StockAdjustment> adjList = _context.StockAdjustment.Where(x => x.ApprovedBy == null).ToList();
            if (adjList.Count() == 0)
            {
                ViewBag.error = "There is no pending request!";
            }
            return View(adjList);
        }
        #endregion

        #region Display per item Details from Transaction Table
        [HttpGet]
        public ActionResult AdjDetails(string stockAdjId)
        {
            StockAdjustment stockAdjustment = _context.StockAdjustment.SingleOrDefault(x => x.StockAdjId == stockAdjId);
            List<TransactionDetail> transactionDetail = _context.TransactionDetail.Where(c => c.TransactionRef == stockAdjId).ToList();
            if (stockAdjustment == null)
            { return HttpNotFound(); }
            var stockAdjViewModel = new AdjustmentDetailsViewModel
            {
                StockAdjustment = stockAdjustment,
                AdjustmentDetails = transactionDetail
            };
            return View(stockAdjViewModel);
        }
        #endregion

        #region Approve Stock Adjustment Request
        public ActionResult Approve(string stockAdjId)
        {

            // to get the user ID of the current user
            string userId = User.Identity.GetUserId();
            var query = _context.AspNetUsers.FirstOrDefault(x => x.Id == userId);
            var thisAdj = _context.StockAdjustment.SingleOrDefault(x => x.StockAdjId == stockAdjId);
            var remarks = _context.TransactionDetail.Where(x => x.TransactionRef == stockAdjId).ToList();

            thisAdj.ApprovedBy = query.Id;
            //thisAdj.Remarks = "Approved";
            foreach (var item in remarks)
            {
                var items = _context.Stationery.Where(x => x.ItemId == item.ItemId).FirstOrDefault();
                item.Remarks = "Approved";
                items.QuantityWarehouse += item.Quantity;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "ApproveAdjustment");

        }
        #endregion

        #region Reject Stock Adjustment Request
        public ActionResult Reject(string stockAdjId)
        {
            // to get the user ID of the current user
            string userId = User.Identity.GetUserId();
            var query = _context.AspNetUsers.FirstOrDefault(x => x.Id == userId);
            var thisAdj = _context.StockAdjustment.SingleOrDefault(x => x.StockAdjId == stockAdjId);
            var remarks = _context.TransactionDetail.Where(x => x.TransactionRef == stockAdjId).ToList();
            thisAdj.ApprovedBy = query.Id;
            foreach (var item in remarks)
            {
                var items = _context.Stationery.Where(x => x.ItemId == item.ItemId).FirstOrDefault();
                item.Remarks = "Rejected";
            }
            //_context.StockAdjustment.Remove(thisAdj);
            _context.SaveChanges();
            return RedirectToAction("Index", "ApproveAdjustment");
        }
        #endregion

    }
    #endregion
}