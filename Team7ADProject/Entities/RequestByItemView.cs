namespace Team7ADProject.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RequestByItemView")]
    public partial class RequestByItemView
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(4)]
        public string ItemId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string Description { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(4)]
        public string DepartmentId { get; set; }

        public int? Quantity { get; set; }
    }
}
