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
            var suppliers = _context.Supplier.Distinct().ToList();
            if (suppliers== null)
            {
                return HttpNotFound();
            }
            var invoiceDetailsViewModel = new InvoiceDetailsViewModel
            { Suppliers=suppliers };
            return View(invoiceDetailsViewModel);
        }
    public ActionResult Create(Invoice invoice, TransactionDetail[] requests)
    {
       
            string result = "Error! Invoice is not complete.";
            if ( invoice != null || requests!= null)
            {
                Invoice newInvoice = new Invoice();
                newInvoice.InvoiceId = invoice.InvoiceId;
                newInvoice.InvoiceNo = invoice.InvoiceNo;
                newInvoice.InvoiceAmount = invoice.InvoiceAmount;
                newInvoice.SupplierId = invoice.SupplierId;
                newInvoice.InvoiceDate = invoice.InvoiceDate;
                _context.Invoice.Add(newInvoice);
                //create TD
                foreach (var item in requests)
                {
                    var transactionId = Guid.NewGuid();
                    TransactionDetail transactionDetail = new TransactionDetail();
                    transactionDetail.TransactionId =Convert.ToInt32(transactionId);
                    transactionDetail.TransactionRef = item.TransactionRef;
                    transactionDetail.TransactionDate = item.TransactionDate;
                    transactionDetail.ItemId = item.ItemId;
                    transactionDetail.Quantity = item.Quantity;
                    transactionDetail.Remarks = " ";
                    transactionDetail.UnitPrice =0;
                    _context.TransactionDetail.Add(transactionDetail);
                }
                _context.SaveChanges();
                result = "Success! Invoice Is Complete!";
        }

        return Json(result, JsonRequestBehavior.AllowGet);
    }

}
    #endregion
}
