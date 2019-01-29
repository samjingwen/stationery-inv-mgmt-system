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
        
        public ActionResult Index(int id = 0)
        {
            LogicDB context = new LogicDB();
            CollectionPointViewModel model = new CollectionPointViewModel();

            string userId = User.Identity.GetUserId();

            int cpId = disbursementService.GetDeptCpId(userId);
            string cpName = disbursementService.GetDeptCpName(userId);

            ViewBag.cpId = cpId;
            ViewBag.cpName = cpName;

            if (id == 1)
                ViewBag.successHandler = 1;
            else if (id == 2)
                ViewBag.successHandler = 2;

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateCollectionPoint(CollectionPointViewModel model)
        {
            string userId = User.Identity.GetUserId();

            int cpId = Convert.ToInt32(model.SelectedCP);

            bool isSuccess = disbursementService.UpdateCollectionPoint(userId, cpId);

            if (isSuccess)
            {
                return RedirectToAction("Index", new { id = 1 });
            }
            else
            {
                return RedirectToAction("Index", new { id = 2 });
            }

        }
        #endregion

    }
}