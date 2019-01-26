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
        [Route("api/ackdelivery/{*poNo}")]
        public IEnumerable<PendingPODetails> ListAllPendingPODetails(string poNo)
        {
           // GlobalClass gc = new GlobalClass();
            return gc.SelectedPODetails(poNo);
        }
        #endregion

        #region Update the DO ( in DO table and TD tables)


        #endregion

        #endregion
    }
}
