using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProjectApi.ViewModels
{
    public class AckDeliveryDetails
    {
        public string ItemId { get; set; }
        public int Quantity { get; set; }
        public string Remarks { get; set; }
        public decimal? UnitPrice { get; set; }
        public string DelOrderNo { get; set; }
        public string SupplierId { get; set; }
        public string AcceptedBy { get; set; }
        public string PONo { get; set; }
        
    }
}