using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
//Authors: Lynn Lynn Oo
namespace Team7ADProject.Controllers
{
    public class RaiseRequestController : Controller
    {
        private Stationery context = new Stationery();
        #region Lynn Lynn Oo
        // GET: RaiseRequest
        //[Authorize(Roles = "Employee,Department Representative")]

        public ActionResult Index()
        {
            return View(context);
        }
        #endregion
    }
}