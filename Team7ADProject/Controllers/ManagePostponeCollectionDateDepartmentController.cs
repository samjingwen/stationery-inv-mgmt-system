using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.Service;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    [RoleAuthorize(Roles = "Department Head, Department Representative")]
    public class ManagePostponeCollectionDateDepartmentController : Controller
    {
        #region Author:Lynn Lynn Oo

        LogicDB _context;
        DisbursementService disbService = DisbursementService.Instance;

        public ManagePostponeCollectionDateDepartmentController()
        {
            _context = new LogicDB();
        }
        // GET: ManagePostponeCollectionDateDepartment
        [HttpGet]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            string deptId = _context.AspNetUsers.FirstOrDefault(x => x.Id == userId).DepartmentId;
            BriefDept dept = disbService.GetBriefDept(deptId);
            return View(dept);
        }

        public ActionResult Postpone()
        {
            try
            {
                string userId = User.Identity.GetUserId();
                var dept = _context.AspNetUsers.FirstOrDefault(x => x.Id == userId).Department;
                DateTime nextMonday = GlobalClass.GetNextWeekDay((DateTime)dept.NextAvailableDate, DayOfWeek.Monday);
                DateTime comingMonday = GlobalClass.GetNextWeekDay(DateTime.Now, DayOfWeek.Monday);
                if (nextMonday > comingMonday)
                {
                    dept.NextAvailableDate = nextMonday.AddDays(7);
                }
                else
                {
                    dept.NextAvailableDate = comingMonday.AddDays(7);
                }

                _context.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", disbService.GetBriefDept(dept.DepartmentId)), message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


    }
    #endregion
}
