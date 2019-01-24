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
using Team7ADProjectApi.Models;

namespace Team7ADProject.Controllers
{
    //For DH to delegate ADH
    public class DelegateHeadController : Controller
    {
        private LogicDB _context;

        public DelegateHeadController()
        {
            _context = new LogicDB();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #region Author: Kay Thi Swe Tun
        // GET: DelegateHead
        [Authorize(Roles = RoleName.DepartmentHead)]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            DelegateHeadViewModel viewModel = new DelegateHeadViewModel(userId);

            ViewBag.DepName = viewModel.DepartmentName;
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.DepartmentHead)]
        public string Delegate(DelegateHeadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return "fail";
            }

            else
            {
                string userId = User.Identity.GetUserId();
                var selectedEmployee = model.SelectedUser;
                DelegationOfAuthority doaInDb = new DelegationOfAuthority
                {
                    DelegatedBy = userId,//"b36a58f3-51f9-47eb-8601-bcc757a8cadb";//selected Employee ID;
                    DelegatedTo = selectedEmployee,
                    StartDate = model.StartDate,//new DateTime(2017,3,5);
                    EndDate = model.EndDate,//new DateTime(2017, 5, 5);
                    DepartmentId = model.DepartmentID
                };

                //model.CurrentUser = userId;

                ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                userManager.RemoveFromRole(doaInDb.DelegatedTo, RoleName.Employee);
                userManager.AddToRole(doaInDb.DelegatedTo, RoleName.ActingDepartmentHead);
                _context.DelegationOfAuthority.Add(doaInDb);
                _context.SaveChanges();
                return "success";
            }
        }
    }
    #endregion
}