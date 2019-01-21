using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Team7ADProjectApi.ViewModels;

namespace Team7ADProjectApi.Controllers
{
    public class ClerkController : ApiController
    {
        [Authorize(Roles = "Department Head")]
        [HttpGet]
        [Route("api/department/{id}")]
        public IEnumerable<BriefDepartment> GetDepartments(string id)
        {
            GlobalClass gc = new GlobalClass();
            return gc.ListDepartment(id);
        }

    }
}
