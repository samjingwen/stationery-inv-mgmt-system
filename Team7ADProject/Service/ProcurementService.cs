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
                thisPo.ApprovedBy = query.UserName;

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
                thisPo.ApprovedBy = query.UserName;
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


        

        public bool CheckInvoice(InvoiceViewModel[] invoice, TransactionDetail[] details)
        {
            //return false if delivery order no does not exists or status is billed
            String delOrderNo = invoice[0].DelOrderNo;
            var deliveryOrder = context.DeliveryOrder.FirstOrDefault(x => x.DelOrderNo == delOrderNo);
            if (deliveryOrder.Status == "BILLED" || deliveryOrder == null)
                return false;

            var delOrder = from x in context.DeliveryOrder
                           join y in context.TransactionDetail
                           on x.DelOrderId equals y.TransactionRef
                           where x.DelOrderNo.Equals(delOrderNo, StringComparison.CurrentCultureIgnoreCase)
                           group y by y.ItemId into g
                           select new ItemsAndQtyViewModel
                           { ItemId = g.Key, Quantity = g.Sum(x => x.Quantity) };

            var modDetails = from x in details
                             group x by x.ItemId into g
                             select new ItemsAndQtyViewModel
                             {
                                 ItemId = g.Key,
                                 Quantity = g.Sum(x => x.Quantity)
                             };

            return CompareItems(modDetails.ToList(), delOrder.ToList());
        }

        
        public bool CompareItems(List<ItemsAndQtyViewModel> detail1, List<ItemsAndQtyViewModel> detail2)
        {
            foreach(var i in detail1)
            {
                var query = detail2.FirstOrDefault(x => x.ItemId == i.ItemId);
                if (query == null)
                {
                    continue;
                }
                else
                {
                    query.Quantity -= i.Quantity;
                    if (query.Quantity == 0)
                    {
                        detail2.Remove(query);
                    }
                    else if (query.Quantity > 0)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            if (detail2.Count() <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateInvoice(InvoiceViewModel[] invoice, TransactionDetail[] details)
        {
            string invoiceId = GenerateInvoiceId();
            string delOrderNo = invoice[0].DelOrderNo;
            var delOrder = context.DeliveryOrder.FirstOrDefault(x => x.DelOrderNo == delOrderNo);
            delOrder.Status = "BILLED";
            string delOrderId = delOrder.DelOrderId;
            //Create Invoice
            Invoice newInvoice = new Invoice();
            newInvoice.InvoiceId = invoiceId;
            newInvoice.InvoiceNo = invoice[0].InvoiceNo;
            newInvoice.InvoiceAmount = invoice[0].InvoiceAmount;
            newInvoice.SupplierId = invoice[0].SupplierId;
            newInvoice.InvoiceDate = DateTime.Today;
            newInvoice.DelOrderId = delOrderId;
            context.Invoice.Add(newInvoice);
            //create TD
            foreach (var item in details)
            {
                TransactionDetail transactionDetail = new TransactionDetail();
                transactionDetail.TransactionId = GenerateTransactionDetailId();
                transactionDetail.TransactionRef = invoiceId;
                transactionDetail.TransactionDate = DateTime.Today;
                transactionDetail.ItemId = item.ItemId;
                transactionDetail.Remarks = "Confirmed Invoice";
                transactionDetail.Quantity = item.Quantity;
                context.TransactionDetail.Add(transactionDetail);
            }
            context.SaveChanges();
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
        //Gernerate TDid
        public int GenerateTransactionDetailId()
        {
            TransactionDetail lastItem = context.TransactionDetail.OrderByDescending(m => m.TransactionId).First();
            int lastRequestId = lastItem.TransactionId;
            int newRequestId = lastRequestId + 1;
            return newRequestId;
        }

    }
}