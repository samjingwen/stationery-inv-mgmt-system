using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

//Author: Sam Jing Wen

namespace Team7ADProject.Controllers
{
    //For department rep to change collection point
    public class CollectionPointController : Controller
    {
        #region Author: Sam Jing Wen
        [Authorize(Roles = "Department Head, Department Representative")]
        public ActionResult Index()
        {
            LogicDB context = new LogicDB();
            CollectionPointViewModel model = new CollectionPointViewModel();
            string userId = User.Identity.GetUserId();
            string cpName = context.AspNetUsers.FirstOrDefault(x => x.Id == userId).Department.CollectionPoint.CollectionDescription;
            ViewBag.cpName = cpName;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Department Head, Department Representative")]
        public string UpdateCollectionPoint(CollectionPointViewModel model)
        {
            LogicDB context = new LogicDB();
            string userId = User.Identity.GetUserId();
            var query = context.AspNetUsers.FirstOrDefault(x => x.Id == userId);
            var dept = query.Department;
            int cpId = Convert.ToInt32(model.SelectedCP);
            var collPoint = context.CollectionPoint.FirstOrDefault(x => x.CollectionPointId == cpId);
            dept.CollectionPoint = collPoint;
            context.SaveChanges();
            return "Update Successful";
        }

        #endregion

    }
}