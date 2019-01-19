using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.ViewModels.PostponeCollectionDate
{
    public class PostponeCollDateDetailsViewModel
    {
        #region Author:Lynn Lynn Oo
        public string transactionID { get; set; }
        public string itemID { get; set; }
        public string itemName { get; set; }
        public string quantity { get; set; }
        public string remarks { get; set; }
        public string transactionRef { get; set; }
        public DateTime Date { get; set; }
        #endregion
    }
}