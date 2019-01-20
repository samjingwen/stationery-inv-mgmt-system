using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace Team7ADProject.ViewModels.GenerateReport
{
    //Author: Elaine Chan

    public class GenerateReportViewModel
    {
        [Required]
        public DateTime fDate { get; set; }
        [Required]
        public DateTime tDate { get; set; }

        public string module { get; set; }

        public List<string> statcategory { get; set; }
        public List<string> entcategory { get; set; }
        public List<string> selectstatcategory { get; set; }
        public List<string> selectentcategory { get; set; }
        public List<string> supplier { get; set; }
        public List<string> employee { get; set; }

        public ChartViewModel stattimeDP { get; set; }
        public ChartViewModel statDP { get; set; }
        public ChartViewModel deptDP { get; set; }

    }

}