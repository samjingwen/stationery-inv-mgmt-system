using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.ViewModels
{
    public class RaiserRequestWrapperViewModel
    {
        public string Status { get; set; }
        public DateTime RequestDate { get; set; }
        public List<RaiseRequestViewModel> Entries { get; set; }
    }
}