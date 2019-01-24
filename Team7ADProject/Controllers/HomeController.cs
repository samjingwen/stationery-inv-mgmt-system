using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<SideBarViewModel> menulist = SideBarViewModel.GenList();
            return View(menulist);

        }

        [RoleAuthorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HandleError]
        public ActionResult Error401()
        {
            return View();
        }

        [HandleError]
        public ActionResult Error404()
        {
            return View();
        }


    }
}