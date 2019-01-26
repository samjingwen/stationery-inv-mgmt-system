using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Team7ADProjectApi.ViewModels;

// Author : Zan Tun Khine

  
namespace Team7ADProjectApi.Controllers
{
    using Entities;
    [Authorize(Roles ="Store Manager")]
    public class PurchaseOrderController : ApiController
    {

        #region Zan Tun Khine

        #region List all the POs   

        [HttpGet]
        [Route("api/allpo")]
        public IEnumerable<PendingPO> ListAllPO()
        {
            GlobalClass gc = new GlobalClass();
            return gc.AllPOList();
        }

        #endregion

        #region List all the pending POs
        [HttpGet]
        [Route("api/pendingpo")]
        public IEnumerable<PendingPO> ListAllPendingPO()
        {
            GlobalClass gc = new GlobalClass();
            return gc.PendingPOList();
        }
        #endregion

        #region Display selected PO details
        [HttpGet]
        [Route("api/pendingpo/{*poNo}")]
        public IEnumerable<PendingPODetails> ListAllPendingPODetails(string poNo)
        {
            GlobalClass gc = new GlobalClass();
            return gc.SelectedPODetails(poNo);
        }

        #endregion

        #region Approve PO
        [HttpPost]
        [Route("api/pendingpo/approve")]
        public bool ApprovePO([FromBody] PurchaseOrder po)
        {
            GlobalClass gc = new GlobalClass();
            return gc.ApprovePO(po);
        }
        #endregion

        #region Reject PO
        [HttpPost]
        [Route("api/pendingpo/reject")]
        public bool RejectPO([FromBody] PurchaseOrder po)
        {
            GlobalClass gc = new GlobalClass();
            return gc.RejectPO(po);
        }
        #endregion

        #endregion
    }
}
