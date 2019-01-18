using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    public class AdjustmentDetailsViewModel
    {
        public StockAdjustment StockAdjustment { get; set; }
        public List<TransactionDetail> AdjustmentDetails { get; set; }
    }
}