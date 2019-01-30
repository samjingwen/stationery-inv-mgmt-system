using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProjectApi.ViewModels
{
    public class ReturnItemViewModel
    {
        public string Description { get; set; }
        public string ItemId { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }

        public ReturnItemViewModel()
        {
            Description = null;
            ItemId = null;
            Quantity = 0;
            Category = null;
            Location = null;
        }
    }
}