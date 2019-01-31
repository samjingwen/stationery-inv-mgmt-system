using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
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

        [Route("~/api/returntowarehouse/return")]
        [HttpPost]
        [Authorize(Roles = RoleName.StoreClerk)]
        public IHttpActionResult ReturnItem(ReturnToWarehouseSetApiModel apiModel)
        {
            string status = "fail";
            StationeryRequest stationeryRequestInDb =
                _context.StationeryRequest.FirstOrDefault(m => m.RequestId == apiModel.RequestId);

            TransactionDetail transactionDetailInDb =
                stationeryRequestInDb.TransactionDetail.FirstOrDefault(m =>
                    m.ItemId == apiModel.ItemId && m.Quantity == apiModel.Quantity);
            transactionDetailInDb.Remarks = "Returned";
            _context.SaveChanges();
            status = "Successfully returned";
            stationeryRequestInDb = _context.StationeryRequest.FirstOrDefault(m => m.RequestId == apiModel.RequestId);

            //Checking if all items are returned
            bool allReturned = true;
            foreach (TransactionDetail current in stationeryRequestInDb.TransactionDetail)
            {
                if (current.Remarks != "Void")
                {
                    allReturned = false;
                    break;
                }
            }

            if (allReturned)
            {
                stationeryRequestInDb.Status = "Returned";

                Disbursement disbursementInDb =
                    _context.Disbursement.FirstOrDefault(m => m.RequestId == apiModel.RequestId);
                disbursementInDb.Status = "Returned";
                _context.SaveChanges();
            }
            return Ok(status);
        }

        [Route("~/api/returntowarehouse/returnall")]
        [HttpPost]
        [Authorize(Roles = RoleName.StoreClerk)]
        public IHttpActionResult ReturnAllItem(List<ReturnToWarehouseSetApiModel> apiModels)
        {
            List<string> statusList = new List<string>();
            foreach (ReturnToWarehouseSetApiModel apiModel in apiModels)
            {
                string status = "fail";
                StationeryRequest stationeryRequestInDb =
                    _context.StationeryRequest.FirstOrDefault(m => m.RequestId == apiModel.RequestId);

                TransactionDetail transactionDetailInDb =
                    stationeryRequestInDb.TransactionDetail.FirstOrDefault(m =>
                        m.ItemId == apiModel.ItemId && m.Quantity == apiModel.Quantity);
                transactionDetailInDb.Remarks = "Returned";
                _context.SaveChanges();
                status = "Successfully added";
                statusList.Add(status);
                stationeryRequestInDb = _context.StationeryRequest.FirstOrDefault(m => m.RequestId == apiModel.RequestId);

                //Checking if all items are returned
                bool allReturned = true;
                foreach (TransactionDetail current in stationeryRequestInDb.TransactionDetail)
                {
                    if (current.Remarks != "Void")
                    {
                        allReturned = false;
                        break;
                    }
                }

                if (allReturned)
                {
                    stationeryRequestInDb.Status = "Returned";

                    Disbursement disbursementInDb =
                        _context.Disbursement.FirstOrDefault(m => m.RequestId == apiModel.RequestId);
                    disbursementInDb.Status = "Returned";
                    _context.SaveChanges();
                }           
            }
            return Ok(statusList);
        }
        #endregion
    }
}
