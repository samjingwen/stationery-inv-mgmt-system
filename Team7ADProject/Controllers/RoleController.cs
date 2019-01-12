using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Models;

namespace Team7ADProject.Controllers
{
    public class ManageRoleViewModel
    {
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
        public IEnumerable<SelectListItem> UserList { get; set; }
    }

    public class RoleController : Controller
    {

        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageRole()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roles = context.Roles.Select(r => new SelectListItem
            {
                Value = r.Id,
                Text = r.Name
            });
            var users = context.Users.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.UserName
            });
            ManageRoleViewModel manageRoleViewModel = new ManageRoleViewModel() { RoleList = roles, UserList = users };
            return View(manageRoleViewModel);
        }

        [HttpPost]
        public ActionResult ManageRole2(ManageRoleViewModel model)
        {
            #region Adding Roles to database
            //ApplicationDbContext context = new ApplicationDbContext();
            //string[] roles = { "Acting Department Head", "Department Representative", "Department Head", "Store Clerk", "Store Supervisor", "Store Manager" };
            //foreach (var role in roles)
            //{
            //    context.Roles.Add(new IdentityRole() { Name = role });
            //}
            //context.SaveChanges();
            #endregion
            #region Assign users to role using DDL

            //string roleId = model.RoleId;
            //string userId = model.UserId;
            //ApplicationDbContext context = new ApplicationDbContext();
            //var role = context.Roles.FirstOrDefault(x => x.Id == roleId).Name;
            //ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //manager.AddToRole(userId, role);
            return RedirectToAction("Index", "Home");
            #endregion
        }

        #region AssignDept
        //public ActionResult AssignDept()
        //{
        //    string line;
        //    System.IO.StreamReader file = new System.IO.StreamReader(Server.MapPath(@"~/App_Data/userbydept.csv"));
        //    while ((line = file.ReadLine()) != null)
        //    {
        //        string[] lines = line.Split(',');
        //        LogicDB context = new LogicDB();
        //        string id = lines[0];
        //        var query = context.AspNetUsers.Where(x => x.Id == id).FirstOrDefault();
        //        if (query != null)
        //        {
        //            query.EmployeeName = lines[1];
        //            query.DepartmentId = lines[2];
        //            context.SaveChanges();
        //        }
        //    }
        //    return RedirectToAction("Index", "Home");
        //}
        #endregion

        #region PopulateUsers

        //public ActionResult CreateUsers()
        //{
        //    //string username;
        //    string email;
        //    int counter = 0;
        //    //System.IO.StreamReader file = new System.IO.StreamReader(Server.MapPath(@"~/App_Data/usernames.txt"));
        //    System.IO.StreamReader file2 = new System.IO.StreamReader(Server.MapPath(@"~/App_Data/useremails.txt"));
        //    while ((email = file2.ReadLine()) != null)
        //    {
        //        var user = new ApplicationUser { UserName = email, Email = email };
        //        var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //        userManager.Create(user, "Password1!");
        //        counter++;
        //    }
        //    return RedirectToAction("Index", "Home");
        //}
        #endregion

        public ActionResult AssignRole()
        {
            //string line;
            //System.IO.StreamReader file = new System.IO.StreamReader(Server.MapPath(@"~/App_Data/userbyroles.csv"));
            //while((line = file.ReadLine()) != null)
            //{
            //    string[] lines = line.Split(',');
            //    ApplicationDbContext context = new ApplicationDbContext();
            //    //var role = context.Roles.FirstOrDefault(x => x.Id == lines[1]).Name;
            //    //var userId = context.Users.FirstOrDefault(x => x.Id == lines[0]).Id;
            //    ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //    manager.AddToRole(lines[0], lines[1]);
            //    context.SaveChanges();
            //}
            return RedirectToAction("Index", "Home");
        }

    }
}