using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    public class StationeryDisbursementViewModel
    {
        public StationeryDisbursementViewModel(StationeryRetrieval retrieval)
        {

        }
        public string DepartmentId { get; set; }
        public List<StationeryItemViewModel> StationeryList { get; set; }

    }

    public class StationeryItemViewModel
    {
        public string ItemId { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public string CollectionPoint { get; set; }

        public string CollectionTime { get; set; }
    }
}