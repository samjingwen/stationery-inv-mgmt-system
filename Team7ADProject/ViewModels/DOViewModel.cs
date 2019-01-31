using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Team7ADProject.ViewModels
{
    public class DOViewModel
    {
        [Required]
        public string InvoiceNo { get; set; }
        [Required]
        public string SupplierId { get; set; }
        [Required]
        public decimal InvoiceAmount { get; set; }
        [Required]
        public string DelOrderNo { get; set; }
    }

    public class ItemsAndQtyViewModel
    {
        public string ItemId { get; set; }
        public int Quantity { get; set; }
    }

}