using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.Models;
using Microsoft.AspNet.Identity.Owin;
using Team7ADProject.ViewModels;
using Team7ADProject.Service;

namespace Team7ADProject.Controllers
{
    #region Author: Kay Thi Swe Tun

    //For DH to delegate ADH
    [RoleAuthorize(Roles = "Department Head")]
    public class DelegateHeadController : Controller
    {
        GeneralMgmtService gmService = GeneralMgmtService.Instance;

        // GET: DelegateHead
        [HttpGet]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            DelegateHeadViewModel viewModel = new DelegateHeadViewModel(userId);
            string[] delHead = gmService.GetDelegatedHead(userId);
            if (delHead != null)
            {
                ViewBag.DelHead = delHead;
            }
            ViewBag.DepName = viewModel.DepartmentName;
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult DelegateHead(DelegateHeadViewModel model)
        {
            return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                string userId = User.Identity.GetUserId();
                string selectedUser = model.SelectedUser;

                gmService.AssignDelegateHead(userId, selectedUser, model.DepartmentId, model.StartDate, model.EndDate);

                return RedirectToAction("About", "Home");
            }
        }
    }
    #endregion
}