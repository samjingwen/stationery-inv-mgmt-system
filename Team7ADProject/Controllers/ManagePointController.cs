using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
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
        public ActionResult Create([Bind(Include = "CollectionPointId,CollectionDescription,Time")] ManagePointViewModel collectionPoint)
        {
            if (ModelState.IsValid)
            {
                CollectionPoint newCP = new CollectionPoint()
                {
                    CollectionDescription = collectionPoint.CollectionDescription,
                    Time = DateTime.Parse(collectionPoint.Time)
                };

                db.CollectionPoint.Add(newCP);
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
            if (collectionPoint == null)
            {
                return HttpNotFound();
            }
            var model = new ManagePointViewModel
                        {
                            CollectionPointId = collectionPoint.CollectionPointId,
                            CollectionDescription = collectionPoint.CollectionDescription,
                            Time = collectionPoint.Time.ToString("hh:mm tt")
                        };


            return View(model);
        }

        // POST: ManagePoint/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CollectionPointId,CollectionDescription,Time")] ManagePointViewModel collectionPoint)
        {
            if (ModelState.IsValid)
            {
                var oldCP = db.CollectionPoint.Find(collectionPoint.CollectionPointId);
                oldCP.CollectionDescription = collectionPoint.CollectionDescription;
                oldCP.Time = DateTime.Parse(collectionPoint.Time);
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
    }
}
