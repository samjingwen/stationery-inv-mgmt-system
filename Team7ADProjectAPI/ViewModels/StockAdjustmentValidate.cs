using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProjectApi.ViewModels
{
    public class StockAdjustmentValidate
    {
        public int RequestedQty { get; set; }
        public int RetrievedQty { get; set; }
        public int WarehouseQty { get; set; }
        public string Remarks { get; set; }

        public StockAdjustmentValidate()
        {
            RequestedQty = 0;
            RetrievedQty = 0;
            WarehouseQty = 0;
            Remarks = "";
        }
    }
}