using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Team7ADProject.ViewModels.GenerateReport
{
    //Author: Elaine Chan
    public class GenerateReportViewModel
    {
        [Required]
        [StringLength(4)]
        public string DepartmentId { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Amount { get; set; }
    }
}