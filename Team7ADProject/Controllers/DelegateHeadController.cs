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

namespace Team7ADProject.Controllers
{
    //For DH to delegate ADH
    public class DelegateHeadController : Controller
    {
        #region Author: Kay Thi Swe Tun
        // GET: DelegateHead
        [Authorize(Roles = "Department Head")]
        public ActionResult Index()
        {
            LogicDB context = new LogicDB();
            string userId = User.Identity.GetUserId();
            DelegateHeadViewModel vmodel = new DelegateHeadViewModel(userId);

           List<AspNetUsers> ll= vmodel.DelegateHead;
            ViewBag.DepName = vmodel.DepartmentName;
            return View(vmodel);
           
        }

        [HttpPost]
        [Authorize(Roles = "Department Head")]
        public string Delegate(DelegateHeadViewModel model)
        {
            string userId = User.Identity.GetUserId();
            // the value is received in the controller.
            var selectedGEmployee = model.SelectedUser;
     
            LogicDB context = new LogicDB();
            DelegationOfAuthority dd = new DelegationOfAuthority();

            dd.DelegatedBy = userId;
            dd.DelegatedTo = selectedGEmployee;//"b36a58f3-51f9-47eb-8601-bcc757a8cadb";//selected Employee ID;
            dd.StartDate = model.StartDate;//new DateTime(2017,3,5);
            dd.EndDate = model.EndDate;//new DateTime(2017, 5, 5);

            model.CurrentUser = userId;

            dd.DepartmentId = model.DepartmentID;

            ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            manager.RemoveFromRole(dd.DelegatedTo, "Employee");
            manager.AddToRole(dd.DelegatedTo, "Acting Department Head");


            if (ModelState.IsValid)
            {
                context.DelegationOfAuthority.Add(dd);
                context.SaveChanges();
               return "success";
            }
            return "success";

        }
    }

    






    #endregion
}
