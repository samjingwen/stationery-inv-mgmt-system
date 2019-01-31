using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.Models;
using Team7ADProject.Service;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    [RoleAuthorize(Roles = "Department Head")]
    public class ManageDelegateController : Controller
    {

        #region Author:Kay Thi Swe Tun
        // GET: ManageDelegate
        GeneralMgmtService gmService = GeneralMgmtService.Instance;

        // GET: DelegateHead
        [HttpGet]
        public ActionResult Index()
        {


            string userId = User.Identity.GetUserId();
            DelegateHeadViewModel viewModel = new DelegateHeadViewModel(userId);
            ViewBag.DepName = viewModel.DepartmentName;


            ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationDbContext context = new ApplicationDbContext();
            string[] delHead = gmService.GetDelegatedHead(userId);

            if (delHead != null)
            {
                ApplicationUser user = manager.FindById(delHead[0]);
                if (manager.IsInRole(user.Id, "Acting Department Head"))
                {
                    ViewBag.DelHead = delHead;
                }
            }
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
            string email = gmService.GetUserEmail(userId);
            string subject = "You have been delegated as Acting Department Head";
            string content = string.Format("Your appointment will start on {0} and end on {1}", model.StartDate.ToShortDateString(), model.EndDate.ToShortDateString());
            Email.Send(email, subject, content);

            return RedirectToAction("Index");
        }

        public ActionResult Revoke()
        {
            string userId = User.Identity.GetUserId();
            string[] delHead = gmService.GetDelegatedHead(userId);

            return RedirectToAction("Index");

        }



        #endregion
    }
}