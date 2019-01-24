using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProjectApi.ViewModels
{
    public class DisbursementListViewModel
    {
        public string DisbursementNo { get; set; }
        public string DepartmentId { get; set; }
        public string OTP { get; set; }
        public string ItemId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}