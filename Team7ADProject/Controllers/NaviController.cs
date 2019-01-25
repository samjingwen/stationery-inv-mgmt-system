using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    public class NaviController : Controller
    {
        // GET: Navi
        [RoleAuthorize]
        public ActionResult SideBar()
        {
            List<SideBarViewModel> menulist = SideBarViewModel.GenList();
            return PartialView("_SideBar",menulist);
        }

    }
}