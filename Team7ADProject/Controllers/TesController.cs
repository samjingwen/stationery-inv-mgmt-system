using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Team7ADProject.Controllers
{
    public class TesController : ApiController
    {
        public static List<string> name = new List<string> { "kk", "tt" };
        [HttpGet]
        public IEnumerable<string> GetNames()
        {
            return name;
        }
    }
}
