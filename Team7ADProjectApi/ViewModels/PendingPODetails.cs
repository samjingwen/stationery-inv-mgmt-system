using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Author Zan Tun Khine
namespace Team7ADProjectApi.ViewModels
{
    public class PendingPODetails
    {
        #region From Stationery
        public string Description { get; set; }

        #endregion

        #region From TrasactionDetail

        public int TransactionId { get; set; }
        public string ItemId { get; set; }
        public int Quantity { get; set; }
        public string Remarks { get; set; }
        public string TransactionRef { get; set; }
        public decimal? UnitPrice { get; set; }

        #endregion

        #region Pending PO
        public string PONo { get; set; }
        public string SupplierId { get; set; }
        public string Status { get; set; }
        public string OrderedBy { get; set; }
        public DateTime Date { get; set; }
        public decimal? Amount { get; set; }
        public decimal? UnitAmount { get; set; }
        #endregion


    }
}