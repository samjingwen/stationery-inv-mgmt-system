namespace Team7ADProject.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Disbursement")]
    public partial class Disbursement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Disbursement()
        {
            TransactionDetail = new HashSet<TransactionDetail>();
        }

        [StringLength(10)]
        public string DisbursementId { get; set; }

        [Required]
        [StringLength(10)]
        public string DisbursementNo { get; set; }

        [Required]
        [StringLength(4)]
        public string DepartmentId { get; set; }

        [StringLength(128)]
        public string AcknowledgedBy { get; set; }

        [StringLength(128)]
        public string DisbursedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(10)]
        public string RequestId { get; set; }

        [Required]
        [StringLength(25)]
        public string Status { get; set; }

        [StringLength(4)]
        public string OTP { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual AspNetUsers AspNetUsers1 { get; set; }

        public virtual Department Department { get; set; }

        public virtual StationeryRequest StationeryRequest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionDetail> TransactionDetail { get; set; }
    }
}
