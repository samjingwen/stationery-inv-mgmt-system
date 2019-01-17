namespace Team7ADProject.Entities
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

        [Required]
        [StringLength(10)]
        [Display(Name = "Invoice ID")]
        public string InvoiceId { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Invoice No")]
        public string InvoiceNo { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Invoice Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [StringLength(4)]
        [Display(Name = "Supplier ID")]
        public string SupplierId { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "Amount")]
        public decimal InvoiceAmount { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "DeliveryOrder ID")]
        public string DelOrderId { get; set; }

        public virtual DeliveryOrder DeliveryOrder { get; set; }

        public virtual Supplier Supplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionDetail> TransactionDetail { get; set; }
    }
}
