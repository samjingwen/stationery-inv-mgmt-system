using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    public class RaisePOViewModel
    {
        [Key]
        [StringLength(10)]
        public string PONo { get; set; }

        [Required]
        [StringLength(4)]

        //need to change to display user
        public string SupplierId { get; set; }

        [Required]
        [StringLength(128)]
        public string OrderedBy { get; set; }

        [StringLength(128)]
        public string ApprovedBy { get; set; }

        //to be initialized at backend to ""
        //public string ApprovedBy { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(25)]
        public string Status { get; set; }

        public IEnumerable<Supplier> Suppliers { get; set; }
        public IEnumerable<PurchaseOrder> PurchaseOrders { get; set; }
        public IEnumerable<TransactionDetail> TransactionDetail { get; set; }
        public IEnumerable<Stationery> Stationeries { get; set; }

        public IEnumerable<String> SupplierAndPrice { get; set; }

        public RaisePOViewModel()
        {

        }

        [Required]
        [StringLength(4)]
        public string ItemId { get; set; }

        public int Quantity { get; set; }

        public string Remarks { get; set; }


        [Required]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "Unit of measure")]
        public string UnitOfMeasure { get; set; }

        [Required]
        [StringLength(10)]
        public string TransactionRef { get; set; }

        [Column(TypeName = "date")]
        public DateTime TransactionDate { get; set; }


        [Column(TypeName = "numeric")]
        public decimal? UnitPrice { get; set; }

        [Display(Name = "Category")]
        public IEnumerable<String> Categories { get; set; }


    }
}