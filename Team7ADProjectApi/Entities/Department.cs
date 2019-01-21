namespace Team7ADProjectApi.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Department")]
    public partial class Department
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Department()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
            DelegationOfAuthority = new HashSet<DelegationOfAuthority>();
            Disbursement = new HashSet<Disbursement>();
            StationeryRequest = new HashSet<StationeryRequest>();
        }

        [StringLength(4)]
        public string DepartmentId { get; set; }

        [Required]
        [StringLength(50)]
        public string DepartmentName { get; set; }

        public int CollectionPointId { get; set; }

        [Required]
        [StringLength(128)]
        public string DepartmentRepId { get; set; }

        [Required]
        [StringLength(8)]
        public string FaxNo { get; set; }

        [Required]
        [StringLength(8)]
        public string ContactNo { get; set; }

        [Required]
        [StringLength(128)]
        public string DepartmentHeadId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }

        public virtual AspNetUsers AspNetUsers1 { get; set; }

        public virtual AspNetUsers AspNetUsers2 { get; set; }

        public virtual CollectionPoint CollectionPoint { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DelegationOfAuthority> DelegationOfAuthority { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Disbursement> Disbursement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StationeryRequest> StationeryRequest { get; set; }
    }
}
