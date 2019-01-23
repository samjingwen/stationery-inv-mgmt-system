using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProjectApi.ViewModels
{
    public class StationeryRetrievalApiModel
    {
        public string itemName { get; set; }
        public List<StationeryRetrieveDto> StationeryRetrieveDtos { get; set; }
    }
}