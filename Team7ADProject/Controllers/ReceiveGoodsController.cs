using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Team7ADProject.Controllers
{
    //For DR to acknowledge receipt of goods
    public class ReceiveGoodsController : Controller
    {
        // GET: ReceiveGoods
        public ActionResult Index()
        {
            return View();
        }
    }
}