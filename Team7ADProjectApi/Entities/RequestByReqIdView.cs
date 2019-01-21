namespace Team7ADProjectApi.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RequestByReqIdView")]
    public partial class RequestByReqIdView
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string RequestId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string DepartmentId { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(4)]
        public string ItemId { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "numeric")]
        public decimal UnitPrice { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Quantity { get; set; }
    }
}
