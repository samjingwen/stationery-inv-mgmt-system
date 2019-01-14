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
        public string ItemId { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }

        public int ReorderLevel { get; set; }

        public int ReorderQuantity { get; set; }

        [Required]
        [StringLength(25)]
        public string UnitOfMeasure { get; set; }

        public int QuantityTransit { get; set; }

        public int QuantityWarehouse { get; set; }

        [Required]
        [StringLength(50)]
        public string Location { get; set; }

        [Required]
        [StringLength(4)]
        public string FirstSupplierId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal FirstSuppPrice { get; set; }

        [Required]
        [StringLength(4)]
        public string SecondSupplierId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal SecondSuppPrice { get; set; }

        [Required]
        [StringLength(4)]
        public string ThirdSupplierId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ThirdSuppPrice { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual Supplier Supplier1 { get; set; }

        public virtual Supplier Supplier2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionDetail> TransactionDetail { get; set; }
    }
}
