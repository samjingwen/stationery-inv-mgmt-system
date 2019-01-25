using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProjectApi.ViewModels
{

    #region Author Zan Tun Khne
    public class AckDelivery
    {
        #region From Delivery Order

        public string DelOrderId { get; set; }
        public string DelOrderNo { get; set; }
        public string SupplierId { get; set; }
        public string AcceptedBy { get; set; }
        public DateTime Date { get; set; }
        public string PONo { get; set; }
        public string Status { get; set; }

        #endregion

        #region From Stationery 

        public string ItemId { get; set; }
        public int QuantityWarehouse { get; set; }

        #endregion

        #region From TrasactionDetail

        public int TransactionId { get; set; }
        public int Quantity { get; set; }
        public string Remarks { get; set; }
        public string TransactionRef { get; set; }    

        #endregion



    }

    #endregion
}