using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Models;
using Team7ADProject.Service;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    public class ManageDelegateController : Controller
    {
        // GET: ManageDelegate
        GeneralMgmtService gmService = GeneralMgmtService.Instance;

        // GET: DelegateHead
        [HttpGet]
        public ActionResult Index()
        {
            ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationDbContext context = new ApplicationDbContext();

            string userId = User.Identity.GetUserId();
            DelegateHeadViewModel viewModel = new DelegateHeadViewModel(userId);
            string[] delHead = gmService.GetDelegatedHead(userId);

            ApplicationUser user = manager.FindById(delHead[0]);

            if (delHead != null && manager.IsInRole(user.Id, "Acting Department Head"))
            {
                ViewBag.DelHead = delHead;
            }
            ViewBag.DepName = viewModel.DepartmentName;
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult Delegate(DelegateHeadViewModel model)
        {
            ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationDbContext context = new ApplicationDbContext();

            manager.AddToRole(model.SelectedUser, "Acting Department Head");

            string userId = User.Identity.GetUserId();
            model.DeptHeadId = userId;

            gmService.AssignDelegateHead(userId, model.SelectedUser, model.DepartmentId, model.StartDate, model.EndDate);

            return RedirectToAction("Index", "Home");

        }
    }
}