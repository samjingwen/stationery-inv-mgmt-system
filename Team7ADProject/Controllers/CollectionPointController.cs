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
            int cpId = context.AspNetUsers.FirstOrDefault(x => x.Id == userId).Department.CollectionPointId;
            string cpName = context.AspNetUsers.FirstOrDefault(x => x.Id == userId).Department.CollectionPoint.CollectionDescription;
            ViewBag.cpId = cpId;
            ViewBag.cpName = cpName;
            return View(model);
        }

        [HttpPost]
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