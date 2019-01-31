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
                                        PurchaseOrder = purchaseOrder, PODetails = transactionDetail
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
                //recipientEmail = thisPo.AspNetUsers1.Email;
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


        public List<DelOrderDetailsViewModel> GetSupplierDelOrder(string supplierId)
        {
            var query = (from x in context.DeliveryOrder
                         where x.SupplierId == supplierId && x.Status != "BILLED"
                         orderby x.DelOrderId
                         select new DelOrderDetailsViewModel
                         {
                             DelOrderNo = x.DelOrderNo,
                             Date = x.Date,
                         }).ToList();
            return query;
        }

        
       
        

        public void CreateInvoice()
        {
            



        }


        //Generate Invoiceid
        public string GenerateInvoiceId()
        {
            Invoice lastItem = context.Invoice.OrderByDescending(m => m.InvoiceId).First();
            string lastRequestIdWithoutPrefix = lastItem.InvoiceId.Substring(3);
            int newRequestIdWithoutPrefixInt = Int32.Parse(lastRequestIdWithoutPrefix) + 1;
            string newRequestIdWithoutPrefixString = newRequestIdWithoutPrefixInt.ToString();
            string requestId = "SID0000" + newRequestIdWithoutPrefixString;
            return requestId;
        }
    }
}