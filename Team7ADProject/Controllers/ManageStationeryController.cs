using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    //For SC to change stationeries information
    public class ManageStationeryController : Controller
    {
        private LogicDB _context;

        public ManageStationeryController()
        {
            _context = new LogicDB();
        }
        // GET: ManageStationery
        public ActionResult Index()
        {
            var stationeries = _context.Stationery.ToList();
            return View(stationeries);
        }

        public ActionResult Edit(string id)
        {
            var stationery = _context.Stationery.SingleOrDefault(c => c.ItemId == id);

            if (stationery == null)
                return HttpNotFound();

            var viewModel = new StationeryFormViewModel(stationery);
            viewModel.Suppliers = _context.Supplier.ToList();
            viewModel.Categories = _context.Stationery.Select(m => m.Category).Distinct().ToList();
            viewModel.Units = _context.Stationery.Select(m => m.UnitOfMeasure).Distinct().ToList();
            return View("StationeryForm",viewModel);
        }

        [HttpPost]

        public ActionResult Save()
        {
            if (!ModelState.IsValid)
            throw new NotImplementedException();
        }
    }
}