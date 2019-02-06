using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Service
{
    public sealed class ProcurementService
    {
        #region Singleton Design Pattern

        private static readonly ProcurementService instance = new ProcurementService();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ProcurementService()
        {
        }

        private ProcurementService()
        {
        }

        public static ProcurementService Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        LogicDB context = new LogicDB();

        public List<PurchaseOrder> GetPurchaseOrders()
        {
            return context.PurchaseOrder.ToList();
        }

        public PoDetailsViewModel GetPoDetailsViewModel(string poNo)
        {
            PurchaseOrder purchaseOrder = context.PurchaseOrder.FirstOrDefault(x => x.PONo == poNo);
            List<TransactionDetail> transactionDetail = context.TransactionDetail.Where(c => c.TransactionRef == poNo).ToList();
            if (purchaseOrder == null)
            {
                return null;
            }
            var poDetailsViewModel = new PoDetailsViewModel
            {
                PurchaseOrder = purchaseOrder,
                PODetails = transactionDetail
            };
            return poDetailsViewModel;
        }

        public bool UpdatePurchaseOrder(string userId, string poNo, bool isApproved)
        {
            var query = context.AspNetUsers.FirstOrDefault(x => x.Id == userId);
            var thisPo = context.PurchaseOrder.FirstOrDefault(x => x.PONo == poNo);
            if (query == null || thisPo == null)
            {
                return false;
            }

            if (isApproved)
            {
                thisPo.Status = "Pending Delivery";
                thisPo.ApprovedBy = query.Id;

                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }

                #region Send Email
                string recipientEmail, subject, content;
                recipientEmail = thisPo.AspNetUsers1.Email;
                subject = " PO approved!";
                content = "I am very happy to inform you, your PO has been approved ";
                Email.Send(recipientEmail, subject, content);
                #endregion

                return true;
            }
            else
            {
                thisPo.Status = "Rejected";
                thisPo.ApprovedBy = query.Id;
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }

                #region SendEmail
                string recipientEmail, subject, content;
                recipientEmail = thisPo.AspNetUsers1.Email;
                recipientEmail = "gaojiaxue@outlook.com";
                subject = " PO rejected!";
                content = "Unfortunately, your PO was rejected";
                Email.Send(recipientEmail, subject, content);
                #endregion

                return true;
            }
        }

        public List<Supplier> GetListSupplier()
        {
            return context.Supplier.Distinct().ToList();
        }


        public ValidateInvoiceViewModel GetSupplierDelOrder(string supplierId)
        {
            var query = (from x in context.DeliveryOrder
                         where x.SupplierId == supplierId && x.Status != "BILLED"
                         orderby x.DelOrderId
                         select new DelOrderDetailsViewModel
                         {
                             DelOrderNo = x.DelOrderNo,
                             Date = x.Date,
                         }).ToList();
            ValidateInvoiceViewModel model = new ValidateInvoiceViewModel
            {
                DelOrderDetails = query
            };

            return model;
        }

        public string GetDelOrderId(string DelOrderNo)
        {
            return context.DeliveryOrder.FirstOrDefault(x => x.DelOrderNo == DelOrderNo).DelOrderId;
        }



        public bool CreateInvoice(ValidateInvoiceViewModel model)
        {
            bool isSuccess;
            string invoiceNo = model.InvoiceNo;
            DateTime invoiceDate = DateTime.Now;
            string supplierId = model.SupplierId;
            decimal invoiceAmt = model.InvoiceAmt;
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in model.DelOrderDetails)
                    {
                        if (item.isSelected)
                        {
                            string invoiceId = GenerateInvoiceId();
                            string delOrderId = GetDelOrderId(item.DelOrderNo);
                            var query = context.DeliveryOrder.FirstOrDefault(x => x.DelOrderId == delOrderId);
                            query.Status = "BILLED";
                            Invoice newInvoice = new Invoice();
                            newInvoice.InvoiceId = invoiceId;
                            newInvoice.InvoiceNo = invoiceNo;
                            newInvoice.SupplierId = supplierId;
                            newInvoice.InvoiceAmount = invoiceAmt;
                            newInvoice.InvoiceDate = invoiceDate;
                            newInvoice.DelOrderId = delOrderId;
                            context.Invoice.Add(newInvoice);
                            context.SaveChanges();
                        }
                    }
                    dbContextTransaction.Commit();
                    isSuccess = true;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    isSuccess = false;
                }
            }

            return isSuccess;
        }


        //Generate Invoiceid
        public string GenerateInvoiceId()
        {
            Invoice lastItem = context.Invoice.OrderByDescending(m => m.InvoiceId).First();
            string lastRequestIdWithoutPrefix = lastItem.InvoiceId.Substring(3);
            int newRequestIdWithoutPrefixInt = Int32.Parse(lastRequestIdWithoutPrefix) + 1;
            string newRequestIdWithoutPrefixString = newRequestIdWithoutPrefixInt.ToString("0000000");
            string requestId = "SID" + newRequestIdWithoutPrefixString;
            return requestId;
        }
    }
}