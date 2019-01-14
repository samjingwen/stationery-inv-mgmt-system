using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Team7ADProject.Controllers
{
    //For DH or ADH to approve or reject request
    public class ManageRequestController : Controller
    {
        // GET: ManageRequest
        public ActionResult Index()
        {
            return View();
        }
    }
}