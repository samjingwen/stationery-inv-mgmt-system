namespace Team7ADProject.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StationeryRequest")]
    public partial class StationeryRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StationeryRequest()
        {
            Disbursement = new HashSet<Disbursement>();
            TransactionDetail = new HashSet<TransactionDetail>();
        }

        [Key]
        [StringLength(10)]
        public string RequestId { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Request from")]
        public string RequestedBy { get; set; }

        [StringLength(128)]
        [Display(Name = "Approved by")]
        public string ApprovedBy { get; set; }

        [Required]
        [StringLength(4)]
        public string DepartmentId { get; set; }

        [Required]
        [StringLength(25)]
        public string Status { get; set; }

        public string Comment { get; set; }

        [Display(Name = "Request Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyy}")]
        public DateTime RequestDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CollectionDate { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual AspNetUsers AspNetUsers1 { get; set; }

        public virtual Department Department { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Disbursement> Disbursement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionDetail> TransactionDetail { get; set; }
    }
}
