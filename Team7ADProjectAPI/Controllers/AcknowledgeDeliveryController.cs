using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Team7ADProjectApi.ViewModels;

//Author Zan Tun Khine

namespace Team7ADProjectApi.Controllers
{
    using Entities;
    [Authorize(Roles = "Store Clerk")]
    public class AcknowledgeDeliveryController : ApiController
    {

        GlobalClass gc = new GlobalClass();
        #region Author Zan Tun Khine

        #region Display list of POs where Status==> "Pending Delivery"
        [HttpGet]
        [Route("api/ackdelivery/pendingdelivery")]
        public IEnumerable<String> PendingDeliveryPoList()
        {
            //GlobalClass gc = new GlobalClass();
            return gc.PendingDeliveryPoList();
        }

        #endregion


        #region Display Detail list of items for selected PO
        [HttpGet]
        [Route("api/ackdelivery/pendingdelivery/{*poNo}")]
        public IEnumerable<PendingPODetails> ListAllPendingPODetails(string poNo)
        {
           // GlobalClass gc = new GlobalClass();
            return gc.SelectedPODetails(poNo);
        }
        #endregion


        #region Create DO

        //Create One DO
        [HttpPost] 
        [Route("api/ackdelivery/add")]
        public IHttpActionResult create([FromBody] DeliveryOrder newDO)
        {
            return Ok(new GlobalClass().CreateDO(newDO));
        }

        //Create Multiple DOs
        [HttpPost]
        [Route("api/ackdelivery/addm")]
        public IHttpActionResult createM([FromBody] List<DeliveryOrder> newDO)
        {
            return Ok(new GlobalClass().CreateMDO(newDO));
        }

        //Create DO , Create New Transactions Ref to DOId , Update Warehouse Stock
        [HttpPost]
        [Route("api/ackdelivery/addmm")]
        public IHttpActionResult createM2([FromBody] List<AckDeliveryDetails> newDO)
        {
            return Ok(new GlobalClass().CreateMDO2(newDO));
        }

        #endregion

        #endregion
    }
}
