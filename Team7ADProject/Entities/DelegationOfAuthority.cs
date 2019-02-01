namespace Team7ADProject.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DelegationOfAuthority")]
    public partial class DelegationOfAuthority
    {
        [Key]
        public int DOAId { get; set; }

        [Required]
        [StringLength(128)]
        public string DelegatedBy { get; set; }

        [Required]
        [StringLength(128)]
        public string DelegatedTo { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(4)]
        public string DepartmentId { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual AspNetUsers AspNetUsers1 { get; set; }

        public virtual Department Department { get; set; }
    }
}
