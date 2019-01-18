using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    //Author: Teh Li Heng 15/1/2019
    //CRUD operation for stationery completed with validation

    #region Teh Li Heng
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
            var stationeries = _context.Stationery.Where(m=>m.ActiveState==true).ToList();
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
            return View("StationeryForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Stationery stationery)
        {
            string a = stationery.ItemId;
            if (!ModelState.IsValid)
            {
                var viewModel = new StationeryFormViewModel(stationery)
                {
                    Suppliers = _context.Supplier.ToList(),
                    Categories = _context.Stationery.Select(m => m.Category).Distinct().ToList(),
                    Units = _context.Stationery.Select(m => m.UnitOfMeasure).Distinct().ToList()
                };
                return View("StationeryForm", viewModel);
            }

            //for Create New Stationery operation
            if (stationery.ItemId == null)
            {
                stationery.ItemId = GenerateItemId(stationery.Description);
                stationery.ActiveState = true;
                _context.Stationery.Add(stationery);
            }


            //for Update Stationery operation
            else
            {
                var stationeryInDb = _context.Stationery.Single(m => m.ItemId == stationery.ItemId);
                stationeryInDb.Category = stationery.Category;
                stationeryInDb.Description = stationery.Description;
                stationeryInDb.ReorderLevel = stationery.ReorderLevel;
                stationeryInDb.ReorderQuantity = stationery.ReorderQuantity;
                stationeryInDb.UnitOfMeasure = stationery.UnitOfMeasure;
                stationeryInDb.QuantityWarehouse = stationery.QuantityWarehouse;
                stationeryInDb.Location = stationery.Location;
                stationeryInDb.FirstSupplierId = stationery.FirstSupplierId;
                stationeryInDb.FirstSuppPrice = stationery.FirstSuppPrice;
                stationeryInDb.SecondSupplierId = stationery.SecondSupplierId;
                stationeryInDb.SecondSuppPrice = stationery.SecondSuppPrice;
                stationeryInDb.ThirdSupplierId = stationery.ThirdSupplierId;
                stationeryInDb.ThirdSuppPrice = stationery.ThirdSuppPrice;
            }

            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }
            return RedirectToAction("Index", "ManageStationery");
        }

        public ActionResult New()
        {
            var viewModel = new StationeryFormViewModel()
            {
                Suppliers = _context.Supplier.ToList(),
                Categories = _context.Stationery.Select(m => m.Category).Distinct().ToList(),
                Units = _context.Stationery.Select(m => m.UnitOfMeasure).Distinct().ToList()
            };

            return View("StationeryForm", viewModel);
        }

        public string GenerateItemId(string description)
        {
            description += " ";
            //P024 P is first letter, 02 is category, and 4 is specs
            string firstCharacter = description.Substring(0, 1).ToUpper();


            //Check for item sub-type
            //parsing first letter
            string category = description.Substring(0, description.IndexOf(" "));
            List<Stationery> stationeries = _context.Stationery.Where(m => m.Description.StartsWith(category)).ToList();

            //if sub-type exist take 2nd and 3rd character, else count unique sub-type
            string secondAndThirdCharacter = null;
            string fourthCharacter = null;
            Stationery subStationeries;
            if (stationeries.Count != 0)
            {
                secondAndThirdCharacter = stationeries[0].ItemId.Substring(1, 2);
                int number = int.Parse(stationeries.OrderByDescending(m => m.ItemId).First().ItemId.Substring(3, 1)) +
                             1;
                fourthCharacter = number.ToString();
            }
            else
            {
                if (_context.Stationery.Any(m => m.ItemId.Contains(firstCharacter)))
                {
                    subStationeries =
                        _context.Stationery.Where(m => m.ItemId.StartsWith(firstCharacter)).OrderByDescending(m => m.ItemId).First();
                    int number = int.Parse(subStationeries.ItemId.Substring(1, 2)) + 1;
                    secondAndThirdCharacter = number.ToString().PadLeft(2, '0');
                }
                else
                    secondAndThirdCharacter = "00";
                fourthCharacter = "1";
            }

            return firstCharacter + secondAndThirdCharacter + fourthCharacter;
        }
    }
    #endregion
}