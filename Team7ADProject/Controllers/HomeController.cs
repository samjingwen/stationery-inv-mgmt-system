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
        [Authorize]
        public ActionResult Index()
        {
            List<SideBarViewModel> menulist = SideBarViewModel.GenList();
            return View(menulist);

        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
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