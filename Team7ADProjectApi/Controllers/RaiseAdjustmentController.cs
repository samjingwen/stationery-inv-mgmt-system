using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Team7ADProjectApi.Entities;

namespace Team7ADProjectApi.Controllers
{
    public class RaiseAdjustmentController : ApiController
    {
        public LogicDB context = new LogicDB();

        [HttpGet]
        [Route("api/categories")]
        public IHttpActionResult getCategories()
        {
            List<string> results = context.Stationery.Select(x => x.Category).Distinct().ToList();
            return Ok(results);
        }
    }
}
