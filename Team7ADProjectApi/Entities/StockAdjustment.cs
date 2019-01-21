namespace Team7ADProjectApi.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StockAdjustment")]
    public partial class StockAdjustment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StockAdjustment()
        {
            TransactionDetail = new HashSet<TransactionDetail>();
        }

        [Key]
        [StringLength(10)]
        public string StockAdjId { get; set; }

        [Required]
        [StringLength(128)]
        public string PreparedBy { get; set; }

        [StringLength(128)]
        public string ApprovedBy { get; set; }

        public string Remarks { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual AspNetUsers AspNetUsers1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionDetail> TransactionDetail { get; set; }
    }
}
