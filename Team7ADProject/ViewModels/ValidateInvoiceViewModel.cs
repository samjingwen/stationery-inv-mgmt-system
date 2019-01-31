using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    public class ValidateInvoiceViewModel
    {
        [Required]
        public string InvoiceNo { get; set; }
        [Required]
        public string SupplierId { get; set; }
        [Required]
        public List<string> DelOrderNo { get; set; }
    }

    public class DelOrderDetailsViewModel
    {
        public string DelOrderNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Date { get; set; }

        public bool isSelected { get; set; }

        public List<ItemAndQty> Details
        {
            get
            {
                if (DelOrderNo != null)
                {
                    LogicDB context = new LogicDB();
                    var query = (from x in context.DeliveryOrder
                                 join y in context.TransactionDetail
                                 on x.DelOrderId equals y.TransactionRef
                                 join z in context.Stationery
                                 on y.ItemId equals z.ItemId
                                 where x.DelOrderNo == DelOrderNo
                                 select new ItemAndQty
                                 {
                                     ItemId = y.ItemId,
                                     Description = z.Description,
                                     Quantity = y.Quantity
                                 }).ToList();
                    return query;
                }
                return null;
            }
        }
    }

    public class ItemAndQty
    {
        public string ItemId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }

    public class BriefSupplier
    {
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
    }

    

}