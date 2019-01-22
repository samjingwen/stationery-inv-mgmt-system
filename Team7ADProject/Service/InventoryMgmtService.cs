using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Service
{
    public sealed class InventoryMgmtService
    {
        #region Singleton Design Pattern

        private static readonly InventoryMgmtService instance = new InventoryMgmtService();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static InventoryMgmtService()
        {
        }

        private InventoryMgmtService()
        {
        }

        public static InventoryMgmtService Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        LogicDB context = new LogicDB();

        public List<StockAdjustment> GetListStockAdjustment()
        {
            List<StockAdjustment> adjList = context.StockAdjustment.Where(x => x.ApprovedBy == null).ToList();
            return adjList;
        }


        public AdjustmentDetailsViewModel GetAdjustmentViewModel(string stockAdjId)
        {
            StockAdjustment stockAdjustment = context.StockAdjustment.FirstOrDefault(x => x.StockAdjId == stockAdjId);
            List<TransactionDetail> transactionDetail = context.TransactionDetail.Where(x => x.TransactionRef == stockAdjId).ToList();
            if (stockAdjustment == null)
            { return null; }
            var stockAdjViewModel = new AdjustmentDetailsViewModel
            {
                StockAdjustment = stockAdjustment,
                AdjustmentDetails = transactionDetail
            };
            return stockAdjViewModel;




        }

    }
}