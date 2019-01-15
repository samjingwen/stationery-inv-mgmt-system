using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.Models;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    //For DH to delegate ADH
    public class DelegateHeadController : Controller
    {
        #region Author: Kay Thi Swe Tun
        // GET: DelegateHead
        public ActionResult Index()
        {
            LogicDB context = new LogicDB();
            DelegateHeadViewModel vmodel = new DelegateHeadViewModel();
           
            return View(vmodel);
           
        }


        [HttpPost]
        public string Delegate(DelegateHeadViewModel model)
        {
            string userId = User.Identity.GetUserId();
            // the value is received in the controller.
            var selectedEmp = model.SelectedUser;

            Console.WriteLine(model.StartDate);



            LogicDB context = new LogicDB();
            DelegationOfAuthority dd = new DelegationOfAuthority();

          //  AspNetUsers c = context.AspNetUsers.Where(x => x.Id == selectedGender).First();//validate lote yan
            dd.DelegatedBy = "3fa4edc7-5192-4e80-99ce-13cfc8afe749";
            dd.DelegatedTo = "7e44a54b-98a7-427f-9b05-f72d173a7adf";//"selectedEmp;// "b36a58f3-51f9-47eb-8601-bcc757a8cadb";//selected Employee ID;
            dd.StartDate = new DateTime(2017,3,5);
            dd.EndDate = new DateTime(2017, 5, 5);
            dd.DepartmentId = "FINC";

            //AspNetUserRoles r = new AspNetUserRoles();
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
