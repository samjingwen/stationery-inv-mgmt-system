using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProjectApi.ViewModels
{
    //Gao Jiaxue
    public class RequestTransactionDetailApiModel
    {
        public int TransactionId { get; set; }
        public String ItemId { get; set; }
        public int Quantity { get; set; }
        public String TransactionRef{ get; set; }
        public decimal? UnitPrice { get; set; }

    }
}