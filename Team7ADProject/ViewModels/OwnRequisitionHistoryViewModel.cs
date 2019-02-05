using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    
    public class OwnRequisitionHistoryViewModel
    {
        #region Author:Lynn Lynn Oo||Teh Li Heng
        public string RequestId { get; set; }
        public string ItemId { get; set; }
        public string ItemDescription { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "Invalid number entered")]
        public int ItemQuantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        #endregion
    }

}
