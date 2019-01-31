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
        public List<string> DelOrderNo { get; set; }
    }

    
}