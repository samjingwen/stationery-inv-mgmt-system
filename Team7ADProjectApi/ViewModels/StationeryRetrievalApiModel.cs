using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProjectApi.ViewModels
{
    public class StationeryRetrievalApiModel
    {
        public string ItemName { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
        public int ActualRetrieved { get; set; }
        public bool Collected { get; set; }
    }
}