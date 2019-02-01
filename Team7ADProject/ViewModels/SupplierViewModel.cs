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
        public string SupplierId { get; set; }

        [Required]
        [Display(Name = "Suppiler")]
        public string SupplierName { get; set; }

        [Required]
        public string ContactName { get; set; }

        [Required]
        [StringLength(8)]
        public string ContactNo { get; set; }

        [StringLength(8)]
        public string FaxNo { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [StringLength(25)]
        public string GSTRegNo { get; set; }

        [Required]
        [StringLength(25)]
        public string Status { get; set; }

        public string Email { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryOrder> DeliveryOrder { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoice { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrder> PurchaseOrder { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stationery> Stationery { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stationery> Stationery1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stationery> Stationery2 { get; set; }
    }
}