using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Team7ADProject.Controllers
{
    //For SM to update supplier listing and preferred suppliers
    public class ManageSupplierController : Controller
    {
        // GET: ManageSupplier
        public ActionResult Index()
        {
            return View();
        }
    }
}