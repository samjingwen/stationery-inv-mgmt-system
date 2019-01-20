using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.ViewModels.GenerateReport
{
    public class ChartViewModel
    {
        public List<StringDoubleDPViewModel> datapoint { get; set; }
        public string title { get; set; }
        public string label { get; set; }

        public ChartViewModel(string title, string label, List<StringDoubleDPViewModel> datapoint)
        {
            this.title = title;
            this.label = label;
            this.datapoint = datapoint;
        }
    }
}