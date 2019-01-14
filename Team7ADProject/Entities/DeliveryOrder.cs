namespace Team7ADProject.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DeliveryOrder")]
    public partial class DeliveryOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryOrder()
        {
            Invoice = new HashSet<Invoice>();
            TransactionDetail = new HashSet<TransactionDetail>();
        }

        [Key]
        [StringLength(10)]
        public string DelOrderId { get; set; }

        [Required]
        [StringLength(10)]
        public string DelOrderNo { get; set; }

        [Required]
        [StringLength(4)]
        public string SupplierId { get; set; }

        [Required]
        [StringLength(128)]
        public string AcceptedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(10)]
        public string PONo { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public virtual Supplier Supplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoice { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionDetail> TransactionDetail { get; set; }
    }
}
