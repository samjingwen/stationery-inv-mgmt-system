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

//Author: Sam Jing Wen
namespace Team7ADProject.Controllers
{
    [RoleAuthorize(Roles = "Department Head")]
    public class ManageRepController : Controller
    {
        GeneralMgmtService gmService = GeneralMgmtService.Instance;


        #region Sam Jing Wen
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            string deptId = gmService.GetDeptIdOfUser(userId);

            string empRep = gmService.GetCurrentRepName(deptId);
            ViewBag.empRep = empRep;

            ManageRepViewModel model = gmService.GetManageRepViewModel(deptId);
            return View(model);
        }

        [HttpPost]
        public ActionResult ManageRep(ManageRepViewModel model)
        {
            ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationDbContext context = new ApplicationDbContext();

            LogicDB database = new LogicDB();
            //Retrieve department head
            string depHeadId = User.Identity.GetUserId();
            string deptId = gmService.GetDeptIdOfUser(depHeadId);

            //Get previous employee rep
            string oldEmpRepId = gmService.GetCurrentRepId(deptId);

            //Update to new employee rep
            gmService.UpdateDepartmentRep(model.UserId, deptId);

            //Change previous Department Rep to employee
            manager.RemoveFromRole(oldEmpRepId, "Department Representative");
            manager.AddToRole(oldEmpRepId, "Employee");
            //Assign new employee to Department Rep
            manager.RemoveFromRole(model.UserId, "Employee");
            manager.AddToRole(model.UserId, "Department Representative");
            return RedirectToAction("Index");
        }

        #endregion

    }
}