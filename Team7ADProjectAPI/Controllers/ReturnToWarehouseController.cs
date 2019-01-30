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
    public class ReturnToWarehouseController : ApiController
    {
        LogicDB _context;

        public ReturnToWarehouseController()
        {
            _context = new LogicDB();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #region Author: Teh Li Heng

        [Route("~/api/returntowarehouse/getitemlist")]
        [HttpGet]
        [Authorize(Roles = RoleName.StoreClerk)]
        public IHttpActionResult GetItemList()
        {
            //GlobalClass gc = new GlobalClass();
            //return Ok(gc.GetItemList());
            List<ReturntoWarehouseApiModel> apiModels = new List<ReturntoWarehouseApiModel>();
            List<StationeryRequest> requests = _context.StationeryRequest.Where(m => m.Status == "Void").ToList();
            foreach (StationeryRequest current in requests)
            {
                ReturntoWarehouseApiModel apiModel = new ReturntoWarehouseApiModel
                {
                    RequestId = current.RequestId,
                    DepartmentName = current.Department.DepartmentName
                };
                foreach (TransactionDetail transactionDetail in current.TransactionDetail)
                {
                    if (transactionDetail.Remarks == "Void")
                    {
                        ReturnItemViewModel rv = new ReturnItemViewModel
                        {
                            Category = transactionDetail.Stationery.Category,
                            Description = transactionDetail.Stationery.Description,
                            ItemId = transactionDetail.ItemId,
                            Location = transactionDetail.Stationery.Location,
                            Quantity = transactionDetail.Quantity
                        };
                        apiModel.itemViewModels.Add(rv);
                    }
                }
                apiModels.Add(apiModel);
            }
            return Ok(apiModels);
        }

        //[HttpPost]
        //[Route("api/returntowarehouse/return")]
        //public IHttpActionResult ReturnItem([FromBody]ReturntoWarehouseApiModel apiModel)
        //{
        //    GlobalClass gc = new GlobalClass();
        //    string status = gc.ReturnTo(apiModel);
        //    return Ok(status);
        //}

        #endregion
    }
}
