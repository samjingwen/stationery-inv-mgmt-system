using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProjectApi.ViewModels
{
    public class StationeryRetrieveDto
    {
        public string ItemName { get; set; }
        public List<DepartmentDto> Department { get; set; }
    }
}