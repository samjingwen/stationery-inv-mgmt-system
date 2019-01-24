using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.Service;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    #region Author:Gao Jiaxue
    [RoleAuthorize(Roles = "Store Manager, Acting Department Head")]
    public class ApproveOrderController : Controller
    {
        ProcurementService pService = ProcurementService.Instance;

        //Retrieve All  PO
        public ActionResult ViewPORecord()
        {
            return View(pService.GetPurchaseOrders());
        }
        //Get PoDetails
        [HttpGet]
        public ActionResult PODetails(string poNo)
        {
            PoDetailsViewModel poDetailsViewModel = pService.GetPoDetailsViewModel(poNo);
            if (poDetailsViewModel == null)
            {
                return HttpNotFound();
            }

            return View(poDetailsViewModel);
        }

        //Approve Po
        public ActionResult Approve(string poNo)
        {
            string userId = User.Identity.GetUserId();

            if (pService.UpdatePurchaseOrder(userId, poNo, true))
            {
                return RedirectToAction("ViewPORecord", "ApproveOrder");
            }
            else
            {
                return HttpNotFound();
            }
        }

        //Reject PO
        public ActionResult Reject(string poNo)
        {
            string userId = User.Identity.GetUserId();

            if (pService.UpdatePurchaseOrder(userId, poNo, false))
            {
                return RedirectToAction("ViewPORecord", "ApproveOrder");
            }
            else
            {
                return HttpNotFound();
            }
        }
        #endregion
    }
}