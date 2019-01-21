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
        [Authorize(Roles = "Store Clerk")]
        public ActionResult Index()
        {
            var suppliers = _context.Supplier.Distinct().ToList();
            if (suppliers == null)
            {
                return HttpNotFound();
            }
            var invoiceDetailsViewModel = new InvoiceDetailsViewModel
            { Suppliers = suppliers };
            return View(invoiceDetailsViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Store Clerk")]
        public ActionResult Create(Invoice[] invoice, TransactionDetail[] requests)
        {
            string result = "Error! Invoice is not complete.";
            string invoiceID= GenerateInvoiceId();
            var delID = _context.DeliveryOrder.Select(x => x.DelOrderId).Contains(invoice[0].DelOrderId);
            if(!delID&&invoice != null && requests != null)
            { result = "Your DelOrder No is wrong!"; }
            if (invoice != null && requests != null&&delID)
            {
                Invoice newInvoice = new Invoice();
                newInvoice.InvoiceId = invoiceID;
                newInvoice.InvoiceNo = invoice[0].InvoiceNo;
                newInvoice.InvoiceAmount = invoice[0].InvoiceAmount;
                newInvoice.SupplierId = invoice[0].SupplierId;
                newInvoice.InvoiceDate = DateTime.Today;
                newInvoice.DelOrderId = invoice[0].DelOrderId;
                _context.Invoice.Add(newInvoice);
                //create TD
                foreach (var item in requests)
                {
                    TransactionDetail transactionDetail = new TransactionDetail();
                    transactionDetail.TransactionId = GenerateTransactionDetailId();
                    transactionDetail.TransactionRef = invoiceID;
                    transactionDetail.TransactionDate = DateTime.Today;
                    transactionDetail.ItemId = item.ItemId;
                    transactionDetail.Quantity = item.Quantity;
                    transactionDetail.Remarks = " ";
                    transactionDetail.UnitPrice = 0;
                    _context.TransactionDetail.Add(transactionDetail);
                }
                _context.SaveChanges();
                result = "Success! Invoice Is Complete!";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Gernerate Invoiceid
        public string GenerateInvoiceId()
        {
           Invoice lastItem = _context.Invoice.OrderByDescending(m => m.InvoiceId).First();
            string lastRequestIdWithoutPrefix = lastItem.InvoiceId.Substring(3);
            int newRequestIdWithoutPrefixInt = Int32.Parse(lastRequestIdWithoutPrefix) + 1;
            string newRequestIdWithoutPrefixString = newRequestIdWithoutPrefixInt.ToString();
            string requestId = "SID0000" + newRequestIdWithoutPrefixString;
            return requestId;
        }
        //Gernerate TDid
        public int GenerateTransactionDetailId()
        {
            TransactionDetail lastItem = _context.TransactionDetail.OrderByDescending(m => m.TransactionId).First();
            int lastRequestId = lastItem.TransactionId;
            int newRequestId = lastRequestId + 1;
            return newRequestId;
        }
        #endregion
    }
}
