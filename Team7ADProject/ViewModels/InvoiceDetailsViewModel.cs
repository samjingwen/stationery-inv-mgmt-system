using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    public class InvoiceDetailsViewModel
    {
        [Required]
        public Invoice Invoice { get; set; }
        public List<TransactionDetail> InvoDetails { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public List<DeliveryOrder> DeliveryOrders { get; set; }
    }
}