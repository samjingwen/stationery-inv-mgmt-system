using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Team7ADProjectApi.ViewModels;

namespace Team7ADProjectApi.Controllers
{
    public class DepartmentHeadController : ApiController
    {

        [Authorize(Roles = "Department Head")]
        [HttpGet]
        [Route("api/managedepartmentRep/{id}")]
        public BriefManageDepRep GetDepartments(string id)//username
        {
            GlobalClass gc = new GlobalClass();

            BriefDepartment depinfo = gc.DepInfo(id);
            List<DepEmp> emplist = gc.ListEmp(id);
            BriefManageDepRep brief = new BriefManageDepRep();
            brief.depEmps = emplist;
          brief.depinfo = depinfo;
           
            return brief;
        }
        //[HttpGet]
        //[Route("api/managedepartmentRep/{id}")]
        //public BriefManageDepRep GetDepartmentsTest(string id)//username
        //{
        //    GlobalClass gc = new GlobalClass();

        //    BriefDepartment depinfo = gc.DepInfo(id);
        //    List<DepEmp> emplist = gc.ListEmp(id);
        //    BriefManageDepRep brief = new BriefManageDepRep();
        //    brief.depEmps = emplist;
        //    brief.depinfo = depinfo;

        //    return brief;
        //}

    }
}
