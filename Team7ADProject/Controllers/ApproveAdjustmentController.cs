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
    //For SS or SM to approve adjustment
    public class ApproveAdjustmentController : Controller
    {
        //Author Zan Tun Khine
        #region Zan Tun Khine

        private LogicDB _context = new LogicDB();
        // GET: ApproveAdjustment
        public ActionResult Index()
        {
            List<StockAdjustment> adjList = _context.StockAdjustment.ToList();
            return View(adjList);
        }


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

        public ActionResult Approve(string stockAdjId)
        {
            // to get the user ID of the current user
            string userId = User.Identity.GetUserId();
            var query = _context.AspNetUsers.FirstOrDefault(x => x.Id == userId);


            var thisAdj = _context.StockAdjustment.SingleOrDefault(x => x.StockAdjId == stockAdjId);
            thisAdj.ApprovedBy = query.Id;
            _context.SaveChanges();
            return RedirectToAction("Index", "ApproveAdjustment");

        }
        public ActionResult Reject(string stockAdjId)
        {
            var thisAdj = _context.StockAdjustment.SingleOrDefault(x => x.StockAdjId == stockAdjId);
            _context.StockAdjustment.Remove(thisAdj);
            _context.SaveChanges();
            return RedirectToAction("Index", "ApproveAdjustment");
        }

        #endregion

    }
}