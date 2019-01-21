namespace Team7ADProjectApi.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Invoice")]
    public partial class Invoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoice()
        {
            TransactionDetail = new HashSet<TransactionDetail>();
        }

        [StringLength(10)]
        public string InvoiceId { get; set; }

        [Required]
        [StringLength(10)]
        public string InvoiceNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [StringLength(4)]
        public string SupplierId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal InvoiceAmount { get; set; }

        [Required]
        [StringLength(10)]
        public string DelOrderId { get; set; }

        public virtual DeliveryOrder DeliveryOrder { get; set; }

        public virtual Supplier Supplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionDetail> TransactionDetail { get; set; }
    }
}
