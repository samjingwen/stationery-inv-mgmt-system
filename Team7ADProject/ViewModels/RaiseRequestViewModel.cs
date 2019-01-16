using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    public class RaiseRequestViewModel
    {
        [Required]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }

        
        [RegularExpression(@"^\d+", ErrorMessage = "Please enter a valid quantity.")]
        public int Quantity { get; set; }

        public  List<RaiseRequestViewModel> transactionDetails { get; set; }

    }
}