using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Team7ADProject.Controllers
{
    //For generating charge back and viewing charge back
    public class ChargeBackController : Controller
    {
        // GET: ChargeBack
        public ActionResult Index()
        {
            return View();
        }
    }
}