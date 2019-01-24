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

//Author: Sam Jing Wen
namespace Team7ADProject.Controllers
{
    [Authorize(Roles = "Department Head")]
    public class ManageRepController : Controller
    {
        #region Sam Jing Wen
        public ActionResult Index()
        {
            LogicDB context = new LogicDB();
            string userId = User.Identity.GetUserId();
            var user = context.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();
            string deptId = user.DepartmentId;
            string empRep = user.Department.AspNetUsers1.EmployeeName;
            ViewBag.empRep = empRep;
            var query = context.AspNetUsers.Where(x => x.DepartmentId == deptId).ToList();
            var users = query.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.EmployeeName
            });
            ManageRepViewModel model = new ManageRepViewModel() { UserList = users };
            return View(model);
        }

        [HttpPost]
        public string ManageRep(ManageRepViewModel model)
        {
            ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationDbContext context = new ApplicationDbContext();
            LogicDB database = new LogicDB();
            //Retrieve department head
            string depHeadId = User.Identity.GetUserId();
            var user = database.AspNetUsers.Where(x => x.Id == depHeadId).FirstOrDefault();
            //Retrieve department
            var dept = user.Department;
            //Change department rep
            string oldEmpRepId = user.Department.DepartmentRepId;
            string userId = model.UserId;
            dept.DepartmentRepId = userId;
            database.SaveChanges();
            //Change previous Department Rep to employee
            manager.RemoveFromRole(oldEmpRepId, "Department Representative");
            manager.AddToRole(oldEmpRepId, "Employee");
            //Assign new employee to Department Rep
            manager.RemoveFromRole(userId, "Employee");
            manager.AddToRole(userId, "Department Representative");
            return "success";
        }

        #endregion

    }
}