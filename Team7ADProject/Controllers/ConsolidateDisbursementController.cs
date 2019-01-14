using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Team7ADProject.Controllers
{
    //For SC to generate disbursement list
    public class ConsolidateDisbursementController : Controller
    {
        // GET: ConsolidateDisbursement
        public ActionResult Index()
        {
            return View();
        }
    }
}