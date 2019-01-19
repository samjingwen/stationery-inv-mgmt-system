using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
        private LogicDB db = new LogicDB();

        // GET: ManagePoint
        public ActionResult Index()
        {
            return View(db.CollectionPoint.ToList());
        }

        // GET: ManagePoint/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManagePoint/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CollectionPointId,CollectionDescription,Time")] CollectionPoint collectionPoint)
        {
            if (ModelState.IsValid)
            {
                db.CollectionPoint.Add(collectionPoint);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(collectionPoint);
        }

        // GET: ManagePoint/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollectionPoint collectionPoint = db.CollectionPoint.Find(id);
            ManagePointViewModel model = new ManagePointViewModel
            {
                CollectionPointId = collectionPoint.CollectionPointId,
                CollectionDescription = collectionPoint.CollectionDescription,
                Time = collectionPoint.Time.ToString("hh:mm tt")
        };

            if (collectionPoint == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: ManagePoint/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CollectionPointId,CollectionDescription,Time")] CollectionPoint collectionPoint)
        {
            if (ModelState.IsValid)
            {
                db.Entry(collectionPoint).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(collectionPoint);
        }

        // GET: ManagePoint/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollectionPoint collectionPoint = db.CollectionPoint.Find(id);
            if (collectionPoint == null)
            {
                return HttpNotFound();
            }
            return View(collectionPoint);
        }

        // POST: ManagePoint/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CollectionPoint collectionPoint = db.CollectionPoint.Find(id);
            db.CollectionPoint.Remove(collectionPoint);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        //public ActionResult Index()
        //{
        //    LogicDB context = new LogicDB();
        //    var cpList = (from x in context.CollectionPoint select new ManagePointViewModel
        //    {
        //        CollectionPointId = x.CollectionPointId,
        //        CollectionDescription = x.CollectionDescription,
        //        Time = x.Time.ToString()
        //        }).AsEnumerable();
        //    return View(cpList);
        //}


        ////public ActionResult GetData()
        ////{
        ////    LogicDB context = new LogicDB();
        ////    var cpList = (from x in context.CollectionPoint select new { x.CollectionPointId, x.CollectionDescription, x.Time }).ToList();
        ////    return Json(new { data = cpList }, JsonRequestBehavior.AllowGet);
        ////}

        //public ActionResult SaveCP(int id, string propertyName, string value)
        //{
        //    var status = false;
        //    var message = "";

        //    //Update data to database
        //    using (LogicDB context = new LogicDB())
        //    {
        //        var CollPoint = context.CollectionPoint.Find(id);

        //        object updateValue = value;
                
        //        if(propertyName == "Time")
        //        {
        //            string date = DateTime.Now.ToString("yyyy/M/dd ");
        //            DateTime time = DateTime.ParseExact(date + value, "yyyy/M/dd HH:mm", null);
        //            updateValue = time;
        //        }

        //        if (CollPoint != null)
        //        {
        //            context.Entry(CollPoint).Property(propertyName).CurrentValue = updateValue;
        //            context.SaveChanges();
        //            status = true;
        //        }
        //        else
        //        {
        //            message = "Error";
        //        }
        //    }
        //    var response = new { value = value, status = status, message = message };
        //    JObject obj = JObject.FromObject(response);
        //    return Content(obj.ToString());

        //}

        //public ActionResult AddCP()
        //{
        //    return View(new ManagePointViewModel());

        //}


    }
    #endregion

}