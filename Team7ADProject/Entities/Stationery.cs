namespace Team7ADProject.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Stationery")]
    public partial class Stationery
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Stationery()
        {
            TransactionDetail = new HashSet<TransactionDetail>();
        }

        [Key]
        [StringLength(4)]
        [Display(Name = "Item ID")]
        public string ItemId { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Reorder level Jiaxue")]
        [RegularExpression(@"^\d+", ErrorMessage = "Please enter a valid quantity.")]
        public int ReorderLevel { get; set; }

        [Display(Name = "Reorder quantity")]
        [RegularExpression(@"^\d+", ErrorMessage = "Please enter a valid quantity.")]
        public int ReorderQuantity { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "Unit of measure")]
        public string UnitOfMeasure { get; set; }

        public int QuantityTransit { get; set; }

        [Display(Name = "Quantity at warehouse")]
        [RegularExpression(@"^\d+", ErrorMessage = "Please enter a valid quantity.")]
        public int QuantityWarehouse { get; set; }

        [Required]
        [StringLength(50)]
        public string Location { get; set; }

        [Required]
        [StringLength(4)]
        [Display(Name = "#1 Supplier")]
        public string FirstSupplierId { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "#1 Price")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Please enter a valid price.")]
        public decimal FirstSuppPrice { get; set; }

        [Required]
        [StringLength(4)]
        [Display(Name = "#2 Supplier")]
        public string SecondSupplierId { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "#2 Price")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Please enter a valid price.")]
        public decimal SecondSuppPrice { get; set; }

        [Required]
        [StringLength(4)]
        [Display(Name = "#3 Supplier")]
        public string ThirdSupplierId { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "#3 Price")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Please enter a valid price.")]
        public decimal ThirdSuppPrice { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual Supplier Supplier1 { get; set; }

        public virtual Supplier Supplier2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionDetail> TransactionDetail { get; set; }
    }
}
