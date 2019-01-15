using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    public class PoDetailsViewModel
    {
        public PurchaseOrder PurchaseOrder { get; set; }
        public List<TransactionDetail> PODetails { get; set; }
    }
}