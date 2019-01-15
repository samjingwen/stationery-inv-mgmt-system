using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    //For store clerk to add new collection point

    #region Sam Jing Wen

    [Authorize(Roles = "Store Clerk, Store Manager, Store Supervisor")]
    public class ManagePointController : Controller
    {
        public ActionResult Index()
        {
            LogicDB context = new LogicDB();
            var cpList = (from x in context.CollectionPoint select new ManagePointViewModel
                {
                    CollectionPointId = x.CollectionPointId,
                    CollectionDescription = x.CollectionDescription,
                    Time = x.Time
                }).ToList();
            return View(cpList);
        }


        //public ActionResult GetData()
        //{
        //    LogicDB context = new LogicDB();
        //    var cpList = (from x in context.CollectionPoint select new { x.CollectionPointId, x.CollectionDescription, x.Time }).ToList();
        //    return Json(new { data = cpList }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult SaveCP(int id, string propertyName, string value)
        {
            var status = false;
            var message = "";
            //Update data to database
            using (LogicDB context = new LogicDB())
            {
                var CollPoint = context.CollectionPoint.Find(id);

                object updateValue = value;
                
                if(propertyName == "CollectionDescription")
                {

                }
                else if(propertyName == "Time")
                {
                    //DateTime time;
                    //if (DateTime.TryParse)
                }

                if (CollPoint != null)
                {
                    context.Entry(CollPoint).Property(propertyName).CurrentValue = value;
                    context.SaveChanges();
                    status = true;
                }
                else
                {
                    message = "Error";
                }
            }
                var response = new { value = value, status = status, message = message };
            JObject obj = JObject.FromObject(response);
            return Content(obj.ToString());

        }
    }
    #endregion

}