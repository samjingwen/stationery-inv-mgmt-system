using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    public class SupplierViewModel
    {
        [Required]
        [StringLength(4)]
        [Display(Name = "Supplier ID")]
        public string SupplierId { get; set; }

        [Required]
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        [Required]
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }

        [Required]
        [StringLength(8)]
        [Display(Name = "Contact Number")]
        public string ContactNo { get; set; }

        [StringLength(8)]
        [Display(Name = "Fax Number")]
        public string FaxNo { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "GST Registration Number")]
        public string GSTRegNo { get; set; }

        [Required]
        [StringLength(25)]
        public string Status { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}