using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    public class AdjustmentViewModel
    {
        public int ItemId { get; set; }
        public StockAdjustment Adjustment;

        public List<TransactionDetail> Detail;

        public static List<Stationery> Items;

        public AdjustmentViewModel()
        {
            LogicDB context = new LogicDB();
            Detail = new List<TransactionDetail>();
            Items = context.Stationery.ToList();
            Adjustment = new StockAdjustment();
            Adjustment.TransactionDetail = Detail;
        }
    }
    
}