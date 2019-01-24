using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Team7ADProjectApi.ViewModels;
using Team7ADProjectApi.Entities;
using Team7ADProjectApi.Models;

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

        GlobalClass gc = new GlobalClass();

        #region Sam Jing Wen

        //Get List of disbursements in transit
        [HttpGet]
        [Route("api/clerk/disbnolist/{disbno}")]
        public List<DisbursementListViewModel> GetListDisbursement(string disbno)
        {
            var query = (from x in _context.DisbByDisbNoView
                        select new DisbursementListViewModel
                        {
                            DisbursementNo = x.DisbursementNo,
                            DepartmentId = x.DepartmentId,
                            OTP = x.OTP,
                            ItemId = x.ItemId,
                            Description = x.Description,
                            Quantity = x.Quantity
                        }).Where(x => x.DisbursementNo == disbno);
            return query.ToList();
        }


        [HttpGet]
        [Route("api/clerk/disbnolist")]
        public IHttpActionResult GetListDisNo()
        {
            var query = (from x in _context.Disbursement
                         join y in _context.Department
                         on x.DepartmentId equals y.DepartmentId where x.Status=="In Transit"
                         select new
                         {
                             x.DisbursementNo,
                             x.DepartmentId,
                             y.DepartmentName,
                             x.AcknowledgedBy,
                             x.DisbursedBy,
                             x.Date,
                             x.Status,
                             x.OTP
                         }).Distinct().ToList();
            return Ok(query);
        }




        #endregion




        //[Authorize(Roles = "Department Head")]
        //[HttpGet]
        //[Route("api/department/{id}")]
        //public IEnumerable<BriefDepartment> GetDepartments(string id)
        //{
        //    GlobalClass gc = new GlobalClass();
        //    return gc.ListDepartment(id);
        //}


        [HttpGet]
        //[Route("api/Request/Items")]
        public IHttpActionResult ListRequestByItem(int id = 0)
        {
            return Ok(gc.ListRequestByItem());
        }

        #region Teh Li Heng Generate Retrieval Android
        [HttpGet]
        [Authorize(Roles = RoleName.StoreClerk)]
        [Route("api/clerk/getretrievallist")]
        public IHttpActionResult GetRetrievalList()
        {
            //Getting full list of item
            List<RequestByItemView> fullRetrievalList = _context.RequestByItemView.ToList();

            //Getting disbursement related
            List<TransactionDetail> partiallyFulfilledRequest =
                _context.TransactionDetail.Where(m => m.Remarks == "Partially Fulfilled" && m.TransactionRef.StartsWith("Req")).ToList();

            //TO LESS: Partially fulfilled disbursement
            List<TransactionDetail> partiallyFulfilledDisbursement =
                _context.TransactionDetail.Where(m => (m.Remarks == "Partially Fulfilled" && m.TransactionRef.StartsWith("DISB"))||(m.Disbursement.Status=="In Transit" && m.TransactionRef.StartsWith("DISB"))).ToList();

            List<RequestByItemView> itemToLess = new List<RequestByItemView>();
            for (int i = 0; i < partiallyFulfilledRequest.Count; i++)
            {
                foreach (TransactionDetail current in partiallyFulfilledDisbursement)
                {
                    //find if the request has it's disbursement, if yes add to list to less
                    if (current.Disbursement.RequestId == partiallyFulfilledRequest[i].TransactionRef && current.ItemId==partiallyFulfilledRequest[i].ItemId)
                    {
                        RequestByItemView quantityToLess = new RequestByItemView
                        {
                            ItemId = current.ItemId,
                            Description = current.Stationery.Description,
                            DepartmentId = current.Disbursement.DepartmentId,
                            DepartmentName = current.Disbursement.Department.DepartmentName,
                            Quantity = current.Quantity,//this will give the quantity that should be deducted from total
                        };
                        itemToLess.Add(quantityToLess);
                    }
                }
            }

            //go through full retrieval and less it
            for (int i = 0; i < fullRetrievalList.Count; i++)
            {
                for (int j = 0; j < itemToLess.Count; j++)
                {
                    if (itemToLess[j].ItemId == fullRetrievalList[i].ItemId &&
                        itemToLess[j].DepartmentId == fullRetrievalList[i].DepartmentId)
                    {
                        fullRetrievalList[i].Quantity -= itemToLess[j].Quantity;
                    }
                }
            }

            //remove items with 0 quantity
            fullRetrievalList.RemoveAll(m => m.Quantity == 0);

            //put into new model with new quantity(preparing for edit) and location
            List<StationeryRetrievalApiModel> fullRetrievalListWithRetrieve = new List<StationeryRetrievalApiModel>();
            foreach (RequestByItemView current in fullRetrievalList)
            {
                StationeryRetrievalApiModel modelToAdd = new StationeryRetrievalApiModel(current.ItemId,current.Description,current.DepartmentId,current.DepartmentName,current.Quantity);
                fullRetrievalListWithRetrieve.Add(modelToAdd);
            }

            for (int i = 0; i < fullRetrievalListWithRetrieve.Count; i++)
            {
                string itemId = fullRetrievalListWithRetrieve[i].ItemId;
                Stationery stationery = _context.Stationery.FirstOrDefault(m => m.ItemId == itemId);
                fullRetrievalListWithRetrieve[i].Location = stationery.Location;
                fullRetrievalListWithRetrieve[i].Category = stationery.Category;
                fullRetrievalListWithRetrieve[i].QuantityInWarehouse = stationery.QuantityWarehouse;
            }
            return Ok(fullRetrievalListWithRetrieve);
        }


        #endregion

        //when retrieved remember to set to intransit and create a new retrieval record
    }
}
