using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProjectApi.ViewModels
{
    public class ReturntoWarehouseApiModel
    {
        public string RequestId { get; set; }
        public string ItemId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }

    }
}