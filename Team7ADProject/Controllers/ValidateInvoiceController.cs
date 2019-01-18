using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    #region Gao Jiaxue
    public class ValidateInvoiceController : Controller
    {
        //get Data
        private LogicDB _context;

        public ValidateInvoiceController()
        {
            _context = new LogicDB();
        }
        //Clean garbage
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: ValidateInvoice
        public ActionResult Index()
        {
            var suppliers = _context.Supplier.ToList();
            var invoiceDetailsViewModel = new InvoiceDetailsViewModel
            { Suppliers=suppliers };
            return View(invoiceDetailsViewModel);
        }
    }
    #endregion
}
