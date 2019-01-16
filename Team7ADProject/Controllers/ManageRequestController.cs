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

        #region Author : Kay Thi Swe Tun
        // GET: ManageRequest
        [Authorize(Roles = "Department Head, Acting Department Head")]
        public ActionResult Index()
        {
            return View();
        }



        #endregion
    }
}