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
    [DataContract]
    public class GenerateReportViewModel
    {

        [DataMember(Name = "label")]
        public string Label = "";

        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
        private string deptID;
        private decimal? totalAmt;

        public GenerateReportViewModel(string deptID, decimal? totalAmt)
        {
            this.deptID = deptID;
            this.totalAmt = totalAmt;
        }
    }
}