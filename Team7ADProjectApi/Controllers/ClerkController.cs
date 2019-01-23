using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Team7ADProjectApi.Service;
using Team7ADProjectApi.ViewModels;
using Team7ADProjectApi.Entities;

namespace Team7ADProjectApi.Controllers
{

    public class ClerkController : ApiController
    {
        private readonly LogicDB _context;

        public ClerkController()
        {
            _context = new LogicDB();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        //[Authorize(Roles = "Department Head")]
        //[HttpGet]
        //[Route("api/department/{id}")]
        //public IEnumerable<BriefDepartment> GetDepartments(string id)
        //{
        //    GlobalClass gc = new GlobalClass();
        //    return gc.ListDepartment(id);
        //}

        GlobalClass gc = new GlobalClass();

        [HttpGet]
        //[Route("api/Request/Items")]
        public IHttpActionResult ListRequestByItem(int id = 0)
        {
            return Ok(gc.ListRequestByItem());
        }

        #region Teh Li Heng Generate Retrieval Android
        public IHttpActionResult GetRetrievalList()
        {

            //List<StationeryRequest> partiallyFulfilled =
            //    _context.StationeryRequest.Where(m => m.Status == "Partially  Fulfilled").ToList();
            //List<StationeryRequest> pendingDisbursement =
            //    _context.StationeryRequest.Where(m => m.Status == "Pending Disbursement").ToList();

            ////combining list of items for different request
            //List<TransactionDetail> combinedListPartiallyFulfilled = new List<TransactionDetail>();
            //foreach (StationeryRequest current in partiallyFulfilled)
            //{
            //    combinedListPartiallyFulfilled.AddRange(current.TransactionDetail.ToList());
            //}

            //List<TransactionDetail> combinedListPendingDisrbursement = new List<TransactionDetail>();
            //foreach (StationeryRequest current in pendingDisbursement)
            //{
            //    combinedListPendingDisrbursement.AddRange(current.TransactionDetail.ToList());
            //}








            //StationeryRetrievalApiModel retrievalApiModel = new StationeryRetrievalApiModel();
            //retrievalApiModel.
            return Ok();
        }


        #endregion

    }
}
