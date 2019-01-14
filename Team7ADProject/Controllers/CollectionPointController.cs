using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;

//Author: Sam Jing Wen

namespace Team7ADProject.Controllers
{
    //For department rep to change collection point
    public class CollectionPointController : Controller
    {
        #region Author: Sam Jing Wen
        // GET: CollectionPoint
        public ActionResult Index()
        {

            LogicDB context = new LogicDB();
            var list = context.CollectionPoint.ToList();
            return View(list);
        }
        #endregion

    }
}