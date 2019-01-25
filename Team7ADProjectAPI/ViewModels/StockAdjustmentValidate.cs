using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.ViewModels
{
    public class StockAdjustmentCompare
    {
        public int RequestedQty { get; set; }
        public int RetrievedQty { get; set; }
        public int WarehouseQty { get; set; }
    }
}