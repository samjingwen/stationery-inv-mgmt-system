using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Team7ADProject.ViewModels.GenerateReport
{
    //Author: Elaine Chan

    public class GenerateReportViewModel
    {
        public List<StringDoubleDPViewModel> statdataPoints { get; set; }
        public List<StringDoubleDPViewModel> timedataPoints { get; set; }
        public List<StringDoubleDPViewModel> deptdataPoints { get; set; }

    }
}