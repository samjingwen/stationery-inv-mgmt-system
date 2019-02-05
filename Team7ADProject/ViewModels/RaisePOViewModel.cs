using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    public class RaisePOViewModel
    {
        public PurchaseOrder PurchaseOrder { get; set; }

        public List<TransactionDetail> PODetails { get; set; }

        [Display(Name = "Category")]
        public IEnumerable<String> Categories { get; set; }

        [Display(Name = "Stationery")]
        public IEnumerable<Stationery> Stationeries { get; set; }

        [Display(Name = "Supplier")]
        public IEnumerable<Supplier> Suppliers { get; set; }

        #region From TopSupplierAndPriceDTO
        [Required]
        [Display(Name = "SupplierPrice")]
        public decimal SupplierPrice { get; set; }
        #endregion

        #region From Stationery

        [Key]
        [StringLength(4)]
        [Display(Name = "Item ID")]
        public string ItemId { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [Required]
        [Display(Name = "Item Description")]
        public string Description { get; set; }


        [Required]
        [StringLength(25)]
        [Display(Name = "Unit of measure")]
        public string UnitOfMeasure { get; set; }

        #endregion

        #region From PurchaseOrder

        [Key]
        [StringLength(10)]
        public string PONo { get; set; }

        [Required]
        [StringLength(128)]
        public string OrderedBy { get; set; }

        [StringLength(128)]
        public string ApprovedBy { get; set; }

        #endregion

        #region From Trasaction Details

        [Required]
        [RegularExpression(@"^\d*$", ErrorMessage = "Please enter a valid quantity.")]
        public int Quantity { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitPrice { get; set; }

        public string Remarks { get; set; }

        [Column(TypeName = "date")]
        public DateTime TransactionDate { get; set; }

        #endregion

        #region From Supplier 
        [Key]
        [StringLength(4)]
        public string SupplierId { get; set; }

        [Required]
        [Display(Name = "Suppiler")]
        public string SupplierName { get; set; }

        [Required]
        [StringLength(10)]
        public string TransactionRef { get; set; }

        #endregion

        

    }
}