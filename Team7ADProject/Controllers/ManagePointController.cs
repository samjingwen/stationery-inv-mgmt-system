using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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

    [RoleAuthorize(Roles = "Store Clerk, Store Manager, Store Supervisor")]
    public class ManagePointController : Controller
    {
        private LogicDB db = new LogicDB();

        // GET: ManagePoint
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(db.CollectionPoint.ToList());
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            ManagePointViewModel model = new ManagePointViewModel();
            if (id != 0)
            {
                var query = db.CollectionPoint.Where(x => x.CollectionPointId == id).FirstOrDefault();
                model.CollectionPointId = query.CollectionPointId;
                model.CollectionDescription = query.CollectionDescription;
                model.CPImagePath = query.CPImagePath;
                model.Time = query.Time.ToString("hh:mm tt");
            }
            return View(model);
        }


        // POST: ManagePoint/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(ManagePointViewModel collectionPoint)
        {
            try
            {
                if (collectionPoint.ImageUpload == null && collectionPoint.CollectionPointId == 0)
                {
                    throw new Exception("Please select an image.");
                }
                CollectionPoint cp = new CollectionPoint();
                if (collectionPoint.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(collectionPoint.ImageUpload.FileName);
                    string extension = Path.GetExtension(collectionPoint.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    cp.CPImagePath = "/Content/images/CollPoint/" + fileName;
                    collectionPoint.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/CollPoint/"), fileName));
                }

                string date = DateTime.Now.ToString("yyyy/M/dd ");
                cp.CollectionDescription = collectionPoint.CollectionDescription;
                cp.Time = DateTime.ParseExact(date + collectionPoint.Time, "yyyy/M/dd h:mm tt", null);


                if (collectionPoint.CollectionPointId == 0)
                {
                    db.CollectionPoint.Add(cp);
                    db.SaveChanges();
                }
                else
                {
                    var oldCP = db.CollectionPoint.Where(x => x.CollectionPointId == collectionPoint.CollectionPointId).FirstOrDefault();
                    oldCP.CollectionDescription = cp.CollectionDescription;
                    if (collectionPoint.ImageUpload != null)
                        oldCP.CPImagePath = cp.CPImagePath;
                    oldCP.Time = cp.Time;

                    db.SaveChanges();
                }

                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", db.CollectionPoint.ToList()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);

            }


            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                CollectionPoint cp = db.CollectionPoint.Where(x => x.CollectionPointId == id).FirstOrDefault();
                db.CollectionPoint.Remove(cp);
                db.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", db.CollectionPoint.ToList()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }








    }
    #endregion

}