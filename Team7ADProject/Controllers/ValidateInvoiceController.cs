using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.Service;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    #region Author:Gao Jiaxue
    [RoleAuthorize(Roles = "Store Clerk")]
    public class ValidateInvoiceController : Controller
    {
        ProcurementService pService = ProcurementService.Instance;
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
            var suppliers = pService.GetListSupplier();
            if (suppliers == null)
            {
                return HttpNotFound();
            }
            var invoiceDetailsViewModel = new InvoiceDetailsViewModel
            { Suppliers = suppliers };
            return View(invoiceDetailsViewModel);
        }

        //Create new Invoice and TransactionDetails
        [HttpPost]
        public ActionResult Create(InvoiceViewModel[] invoice, TransactionDetail[] requests)
        {
            string result = "Error! Invoice is not complete.";
            if (requests == null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            bool isValid = pService.CheckInvoice(invoice, requests);
            if (isValid)
            {
                pService.CreateInvoice(invoice, requests);
                result = "Invoice successfully created.";
            }
            else
            {
                result = "Invoice detaiils are incorrect. Please check again.";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        #endregion
    }
}
