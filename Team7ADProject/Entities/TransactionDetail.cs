namespace Team7ADProject.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TransactionDetail")]
    public partial class TransactionDetail
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        [StringLength(4)]
        public string ItemId { get; set; }

        public int Quantity { get; set; }

        [Display(Name = "Status")]
        public string Remarks { get; set; }

        [Required]
        [StringLength(10)]
        public string TransactionRef { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime TransactionDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnitPrice { get; set; }

        public virtual DeliveryOrder DeliveryOrder { get; set; }

        public virtual Disbursement Disbursement { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public virtual Stationery Stationery { get; set; }

        public virtual StationeryRequest StationeryRequest { get; set; }

        public virtual StationeryRetrieval StationeryRetrieval { get; set; }

        public virtual StockAdjustment StockAdjustment { get; set; }
    }
}
