using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Team7ADProject.Controllers
{
    //For department rep to change collection point
    public class CollectionPointController : Controller
    {
        // GET: CollectionPoint
        public ActionResult Index()
        {
            return View();
        }
    }
}