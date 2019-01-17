namespace Team7ADProject.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RequestByDeptView")]
    public partial class RequestByDeptView
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(4)]
        public string DepartmentId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string DepartmentName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(4)]
        public string ItemId { get; set; }

        [Key]
        [Column(Order = 3)]
        public string Description { get; set; }

        public int? Quantity { get; set; }
    }
}
