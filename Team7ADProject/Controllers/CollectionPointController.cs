using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.Service;
using Team7ADProject.ViewModels;

//Author: Sam Jing Wen

namespace Team7ADProject.Controllers
{
    //For department rep to change collection point
    [Authorize(Roles = "Department Head, Department Representative")]
    public class CollectionPointController : Controller
    {
        #region Author: Sam Jing Wen
        DisbursementService disbursementService = DisbursementService.Instance;
        
        public ActionResult Index()
        {
            LogicDB context = new LogicDB();
            CollectionPointViewModel model = new CollectionPointViewModel();

            string userId = User.Identity.GetUserId();

            int cpId = disbursementService.GetDeptCpId(userId);
            string cpName = disbursementService.GetDeptCpName(userId);

            ViewBag.cpId = cpId;
            ViewBag.cpName = cpName;
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateCollectionPoint(CollectionPointViewModel model)
        {
            string userId = User.Identity.GetUserId();

            int cpId = Convert.ToInt32(model.SelectedCP);

            disbursementService.UpdateCollectionPoint(userId, cpId);

            return RedirectToAction("Index");
        }




        #endregion

    }
}