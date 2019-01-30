using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProjectApi.Entities;

namespace Team7ADProjectApi.ViewModels
{
    public class ReturntoWarehouseApiModel
    {
        public string RequestId { get; set; } 
        public string ItemId { get; set; }
        public List<TransactionDetail> TransactionDetails { get; set; }

    }
}