using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.ViewModels.Api
{
    public class ReturntoWarehouseApiViewModel
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
    }
}