namespace Team7ADProject.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DisbByDisbNoView")]
    public partial class DisbByDisbNoView
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string DisbursementNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string DepartmentId { get; set; }

        [StringLength(4)]
        public string OTP { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(4)]
        public string ItemId { get; set; }

        [Key]
        [Column(Order = 3)]
        public string Description { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Quantity { get; set; }
    }
}
