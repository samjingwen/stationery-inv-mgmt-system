using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    public class AdjustmentViewModel
    {
        public StockAdjustment Adjustment;

        public List<TransactionDetail> Detail;

        private static LogicDB context = new LogicDB();
        public static List<Stationery> Items = context.Stationery.ToList();

        public AdjustmentViewModel()
        {
            
        }
    }
}