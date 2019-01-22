using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Author Zan Tun Khine
namespace Team7ADProjectApi.ViewModels
{
    public class PendingPO
    {

        #region From PurchaseOrder
        public string PONo { get; set; }
        public string SupplierId { get; set; }
        public string Status { get; set; }
        public string OrderedBy { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        #endregion

    }
}