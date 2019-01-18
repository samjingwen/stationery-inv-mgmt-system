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
        public string fDate { get; set; }
        public string tDate { get; set; }

    }

}