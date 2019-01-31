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
        public string DepartmentName { get; set; }
        public List<ReturnItemViewModel> itemViewModels { get; set; }

        public ReturntoWarehouseApiModel()
        {
            RequestId = null;
            DepartmentName = null;
            itemViewModels = new List<ReturnItemViewModel>();
        }

    }
}