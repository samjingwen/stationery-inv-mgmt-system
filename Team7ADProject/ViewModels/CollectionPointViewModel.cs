using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

//Authors: Sam Jing Wen
namespace Team7ADProject.ViewModels
{
    #region Sam Jing Wen
    public class CollectionPointViewModel
    {
        public string SelectedCP { get; set; }
        public List<CollectionPoint> CollectionPoints
        {
            get
            {
                LogicDB context = new LogicDB();
                return context.CollectionPoint.ToList();
            }
        }
    }

    public class ManagePointViewModel
    {
        [Required]
        public int CollectionPointId { get; set; }

        [Required]
        [StringLength(50)]
        public string CollectionDescription { get; set; }

        [Required]
        [RegularExpression(pattern:"\\d{1,2}:\\d{2}\\s*(AM|PM)",ErrorMessage = "Please enter a valid time.")]
        public string Time { get; set; }
    }
    #endregion

}