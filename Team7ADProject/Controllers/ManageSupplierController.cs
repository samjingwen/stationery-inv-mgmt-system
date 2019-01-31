using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;
using System.Data.Entity;
using System.Net;

//Author: Zan Tun Khine


namespace Team7ADProject.Controllers
{
    //For SM to update supplier listing and preferred suppliers

    [RoleAuthorize(Roles = "Store Manager")]
    public class ManageSupplierController : Controller
    {
        private LogicDB context = new LogicDB();

        #region Author: Zan Tun Khine

        // GET: ManageSupplier
        public ActionResult Index()
        {
            return View(context.Supplier.ToList());
        }


        // GET: Suppliers/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SupplierId,SupplierName,ContactName,ContactNo,FaxNo,Address,GSTRegNo,Status,Email")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                if (!(CheckSupplier(supplier.SupplierId)))
                {
                    context.Supplier.Add(supplier);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error1 = "The Supplier ID Already Exists!";
                    return View();
                }
            }

            return View(supplier);
        }

        // to check whether the newly added supplier already exists or not
        public bool CheckSupplier(string id)
        {
            Supplier supplier = context.Supplier.Find(id);

            if (supplier != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        // GET: Suppliers/Edit/id
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = context.Supplier.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }


        // POST: Suppliers/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SupplierId,SupplierName,ContactName,ContactNo,FaxNo,Address,GSTRegNo,Status,Email")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                context.Entry(supplier).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }


        // GET: Suppliers/Details/id
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = context.Supplier.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        #endregion
    }
}