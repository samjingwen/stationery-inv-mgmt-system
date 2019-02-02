using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Team7ADProject.ViewModels
{
    public class StationeryViewModel
    {
        [Key]
        [StringLength(4)]
        [Display(Name = "Item ID")]
        public string ItemId { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [Required]
        [Display(Name = "Item Description")]
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

        [Display(Name = "Quantity in transit")]
        [RegularExpression(@"^\d+", ErrorMessage = "Please enter a valid quantity.")]
        public int QuantityTransit { get; set; }

        [Display(Name = "Quantity in warehouse")]
        [RegularExpression(@"^\d+", ErrorMessage = "Please enter a valid quantity.")]
        public int QuantityWarehouse { get; set; }

        [Required]
        [StringLength(50)]
        public string Location { get; set; }

    }
}