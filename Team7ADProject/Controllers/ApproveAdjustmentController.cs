using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.Models;
using Team7ADProject.Service;
using Team7ADProject.ViewModels;

//Author Zan Tun Khine
namespace Team7ADProject.Controllers
{


    #region Zan Tun Khine

    //For SS or SM to approve adjustment

    [RoleAuthorize(Roles = "Store Manager, Store Supervisor")]
    public class ApproveAdjustmentController : Controller
    {
        InventoryMgmtService imService = InventoryMgmtService.Instance;

        private LogicDB _context = new LogicDB();

        #region pending Stock Adjustment Requests

        // GET: ApproveAdjustment
        public ActionResult Index()
        {
            List<StockAdjustment> adjList = imService.GetListStockAdjustment();
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
            AdjustmentDetailsViewModel stockAdjViewModel = imService.GetAdjustmentViewModel(stockAdjId);
            if (stockAdjViewModel == null)
            {
                return HttpNotFound();
            }
            return View(stockAdjViewModel);
        }
        #endregion

        #region Approve Stock Adjustment Request
        public ActionResult Approve(string stockAdjId)
        {
            // to get the user ID of the current user
            string userId = User.Identity.GetUserId();

            imService.UpdateAdjustment(stockAdjId, userId, "Approved");

            return RedirectToAction("Index", "ApproveAdjustment");

        }
        #endregion

        #region Reject Stock Adjustment Request
        public ActionResult Reject(string stockAdjId)
        {
            // to get the user ID of the current user
            string userId = User.Identity.GetUserId();

            imService.UpdateAdjustment(stockAdjId, userId, "Rejected");

            return RedirectToAction("Index", "ApproveAdjustment");
        }
        #endregion

    }
    #endregion
}