using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    [RoleAuthorize(Roles = "Store Supervisor")]
    public class ManagePostponeCollectionDateController : Controller
    {
        LogicDB _context;
        #region Author:Lynn Lynn Oo
        public ManagePostponeCollectionDateController()
        {
            _context = new LogicDB();
        }

        public ActionResult Index()
        {
            List<Department> deptList = _context.Department.ToList();
            return View(deptList);
        }

        public ActionResult Postpone(string id = null)
        {
            try
            {
                Department dept = _context.Department.FirstOrDefault(x => x.DepartmentId == id);
                dept.NextAvailableDate = ((DateTime)dept.NextAvailableDate).AddDays(7);
                _context.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", _context.Department.ToList()), message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}



