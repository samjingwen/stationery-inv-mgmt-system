using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    public class RequestedItemViewModel
    {
        public string Empname
        {
            get;
            set;
        }
        public string RequestID
        {
            get;
            set;
        }
        public string RequestDate
        {
            get;
            set;
        }
        public List<Stationery> Itemlist
        {
            get;
            set;
        }
    }
}