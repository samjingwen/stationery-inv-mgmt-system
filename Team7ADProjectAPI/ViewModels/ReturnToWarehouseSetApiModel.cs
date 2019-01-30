using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProjectApi.ViewModels
{
    public class ReturnToWarehouseSetApiModel
    {
        public string RequestId { get; set; }
        public string ItemId { get; set; }
        public int Quantity { get; set; }
    }
}