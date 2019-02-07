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
    public class PurchaseOrderController : ApiController
    {

        #region Zan Tun Khine

        #region List all the POs   
        [Authorize(Roles = "Store Manager, Store Supervisor")]
        [HttpGet]
        [Route("api/allpo")]
        public IEnumerable<PendingPO> ListAllPO()
        {
            GlobalClass gc = new GlobalClass();
            return gc.AllPOList();
        }

        #endregion

        #region List all the pending POs
        [Authorize(Roles = "Store Manager, Store Supervisor")]
        [HttpGet]
        [Route("api/pendingpo")]
        public IEnumerable<PendingPO> ListAllPendingPO()
        {
            GlobalClass gc = new GlobalClass();
            return gc.PendingPOList();
        }

        #endregion

        #region Display selected PO details
        [Authorize(Roles = "Store Manager, Store Clerk, Store Supervisor")]
        [HttpGet]
        [Route("api/pendingpo/{*poNo}")]
        public IEnumerable<PendingPODetails> ListAllPendingPODetails(string poNo)
        {
            GlobalClass gc = new GlobalClass();
            return gc.SelectedPODetails(poNo);
        }

        #endregion

        #region Approve PO
        [Authorize(Roles = "Store Manager, Store Supervisor")]
        [HttpPost]
        [Route("api/pendingpo/approve")]
        public IHttpActionResult ApprovePO([FromBody] PurchaseOrder po)
        {
            GlobalClass gc = new GlobalClass();
            return Ok(gc.ApprovePO(po));
        }

        #endregion

        #region Reject PO
        [Authorize(Roles = "Store Manager, Store Supervisor")]
        [HttpPost]
        [Route("api/pendingpo/reject")]
        public IHttpActionResult RejectPO([FromBody] PurchaseOrder po)
        {
            GlobalClass gc = new GlobalClass();
            return Ok(gc.RejectPO(po));
        }

        #endregion

        #endregion
    }
}
