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
    [RoleAuthorize(Roles = "Store Supervisor, Store Manager")]
    public class ManagePostponeCollectionDateController : Controller
    {
        LogicDB _context;
        DisbursementService disbService = DisbursementService.Instance;

        #region Author:Lynn Lynn Oo
        public ManagePostponeCollectionDateController()
        {
            _context = new LogicDB();
        }

        public ActionResult Index()
        {
            List<BriefDept> deptList = disbService.GetBriefDept();
            return View(deptList);
        }

        public ActionResult Postpone(string id = null)
        {
            try
            {
                if (id == "ALL")
                {
                    var deptList = _context.Department.ToList();
                    foreach(var dept in deptList)
                    {
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
                    }
                    _context.SaveChanges();
                    return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", disbService.GetBriefDept()), message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Department dept = _context.Department.FirstOrDefault(x => x.DepartmentId == id);
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
                    return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", disbService.GetBriefDept()), message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}



