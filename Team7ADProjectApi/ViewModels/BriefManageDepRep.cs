using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProjectApi.ViewModels
{
    public class BriefManageDepRep
    {
        public BriefDepartment depinfo { get; set; }
        public List<DepEmp> depEmps { get; set; }

    }
}