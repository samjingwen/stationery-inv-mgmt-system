using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProjectApi.ViewModels
{
    public class DepartmentRetrieveDto
    {
        public IEnumerable<StationeryRetrieveDto> stationeryList { get; set; }
    }
}