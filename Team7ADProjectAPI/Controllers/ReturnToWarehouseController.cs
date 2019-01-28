using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Team7ADProjectApi.ViewModels;

namespace Team7ADProjectApi.Controllers
{
    public class ReturnToWarehouseController : ApiController
    {
        #region Author:Lynn Lynn Oo

        [Route("~/api/returntowarehouse/getitemlist")]
        [HttpGet]
        public IHttpActionResult GetItemList()
        {
            GlobalClass gc = new GlobalClass();
            return Ok(gc.GetItemList());
        }

        [HttpPost]
        [Route("~/api/returntowarehouse/return")]
        public IHttpActionResult ReturnItem(ReturntoWarehouseApiModel apiModel)
        {
            GlobalClass gc = new GlobalClass();
            string status = gc.Return(apiModel);
            return Ok(status);
        }

        #endregion
    }
}
