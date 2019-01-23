using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
        [HttpGet]
        [Route("api/clerk/getretrievallist")]
        public IHttpActionResult GetRetrievalList()
        {
            //Getting full list of item
            List<RequestByItemView> fullRetrievalList = _context.RequestByItemView.ToList();

            //Getting disbursement related
            List<TransactionDetail> partiallyFulfilledRequest =
                _context.TransactionDetail.Where(m => m.Remarks == "Partially Fulfilled" && m.TransactionRef.StartsWith("Req")).ToList();

            List<TransactionDetail> partiallyFulfilledDisbursement =
                _context.TransactionDetail.Where(m => m.Remarks == "Partially Fulfilled" && m.TransactionRef.StartsWith("DISB")).ToList();

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

            return Ok(fullRetrievalList);
        }


        #endregion

    }
}
