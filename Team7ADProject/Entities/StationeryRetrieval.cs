namespace Team7ADProject.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StationeryRetrieval")]
    public partial class StationeryRetrieval
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StationeryRetrieval()
        {
            TransactionDetail = new HashSet<TransactionDetail>();
        }

        [Key]
        [StringLength(10)]
        public string RetrievalId { get; set; }

        [Required]
        [StringLength(128)]
        public string RetrievedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionDetail> TransactionDetail { get; set; }
    }
}
