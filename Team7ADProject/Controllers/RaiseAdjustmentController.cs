using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

//Authors: Cheng Zongpei
namespace Team7ADProject.Controllers
{
    //For SC to raise adjustment
    public class RaiseAdjustmentController : Controller
    {
        // GET: RaiseAdjustment
        public ActionResult Index()
        {
            AdjustmentViewModel adjustment = new AdjustmentViewModel();
            adjustment.Detail.Add(new TransactionDetail());
            return View(adjustment);
        }

        [HttpPost]
        public ActionResult Confirm(AdjustmentViewModel adjustment)
        {
            adjustment.ToString();
            return View(adjustment);
        }
    }
}