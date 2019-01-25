using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProjectApi.ViewModels
{
    public class StationeryRetrievalApiModel
    {
        public string ItemId { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string Category { get; set; }

        public string DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int? NeededQuantity { get; set; }

        public int? NewQuantity { get; set; }

        public int QuantityInWarehouse { get; set; }

        public string Remarks { get; set; }

        public StationeryRetrievalApiModel(string itemId, string description, string departmentId,string departmentName, int? neededQuantity)
        {
            ItemId = itemId;
            Description = description;
            DepartmentId = departmentId;
            DepartmentName = departmentName;
            NeededQuantity = neededQuantity;
            NewQuantity = 0;
            Remarks = "";
        }
    }
}