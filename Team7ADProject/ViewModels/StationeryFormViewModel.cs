using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;
using Team7ADProject.Validation;

namespace Team7ADProject.ViewModels
{
    public class StationeryFormViewModel
    {
        [Key]
        [StringLength(4)]
        [Display(Name = "Item ID")]
        public string ItemId { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Reorder level")]
        [RegularExpression(@"^\d+", ErrorMessage = "Please enter a valid quantity.")]
        public int ReorderLevel { get; set; }

        [Display(Name = "Reorder quantity")]
        [RegularExpression(@"^\d+", ErrorMessage = "Please enter a valid quantity.")]
        public int ReorderQuantity { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "Unit of measure")]
        public string UnitOfMeasure { get; set; }

        //[Display(Name = "Quantity in transit")]
        //public int QuantityTransit { get; set; }

        [Display(Name = "Quantity at warehouse")]
        [RegularExpression(@"^\d+",ErrorMessage = "Please enter a valid quantity.")]
        public int QuantityWarehouse { get; set; }

        [Required]
        [StringLength(50)]
        public string Location { get; set; }

        [Required]
        [StringLength(4)]
        [Display(Name = "#1 Supplier")]
        public string FirstSupplierId { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "#1 Price")]
        [Range(0.0, Double.MaxValue,ErrorMessage = "Please enter a valid price.")]
        public decimal FirstSuppPrice { get; set; }

        [Required]
        [StringLength(4)]
        [Display(Name = "#2 Supplier")]
        public string SecondSupplierId { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "#2 Price")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Please enter a valid price.")]
        public decimal SecondSuppPrice { get; set; }

        [Required]
        [StringLength(4)]
        [Display(Name = "#3 Supplier")]
        public string ThirdSupplierId { get; set; }

        [Display(Name = "#3 Price")]
        [Column(TypeName = "numeric")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Please enter a valid price.")]
        public decimal ThirdSuppPrice { get; set; }
        public IEnumerable<Supplier> Suppliers { get; set; }

        [Display(Name = "Category")]
        public IEnumerable<String> Categories { get; set; }

        [Display(Name = "Unit")]
        public IEnumerable<String> Units { get; set; }

        public string Title
        {
            get
            {
                return ItemId != null ? "Edit Stationery" : "New Stationery";
            }
        }
        public StationeryFormViewModel(Stationery stationery)
        {
            ItemId = stationery.ItemId;
            Category = stationery.Category;
            Description = stationery.Description;
            ReorderLevel = stationery.ReorderLevel;
            ReorderQuantity = stationery.ReorderQuantity;
            UnitOfMeasure = stationery.UnitOfMeasure;
            QuantityWarehouse = stationery.QuantityWarehouse;
            Location = stationery.Location;
            FirstSupplierId = stationery.FirstSupplierId;
            FirstSuppPrice = stationery.FirstSuppPrice;
            SecondSupplierId = stationery.SecondSupplierId;
            SecondSuppPrice = stationery.SecondSuppPrice;
            ThirdSupplierId = stationery.ThirdSupplierId;
            ThirdSuppPrice = stationery.ThirdSuppPrice;
        }

        public StationeryFormViewModel()
        {
            ItemId = "";
        }
    }
}