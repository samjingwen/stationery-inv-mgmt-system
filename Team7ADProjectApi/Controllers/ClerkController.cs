using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Team7ADProjectApi.ViewModels;
using Team7ADProjectApi.Entities;
using Team7ADProjectApi.Models;

namespace Team7ADProjectApi.Controllers
{

    public class ClerkController : ApiController
    {
        private readonly LogicDB _context;
        Random rand;

        public ClerkController()
        {
            _context = new LogicDB();
            rand = new Random();
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
                         on x.DepartmentId equals y.DepartmentId
                         join z in _context.CollectionPoint
                         on y.CollectionPointId equals z.CollectionPointId
                         join m in _context.AspNetUsers
                         on y.DepartmentRepId equals m.Id
                         where x.Status == "In Transit"
                         select new
                         {
                             x.DisbursementNo,
                             x.DepartmentId,
                             y.DepartmentName,
                             m.EmployeeName,
                             z.CollectionDescription,
                             x.AcknowledgedBy,
                             x.DisbursedBy,
                             x.Status,
                             x.OTP
                         }).Distinct().ToList();
            return Ok(query);
        }

        //Acknowledge disbursement
        [HttpPost]
        [Route("api/clerk/acknowledgement")]
        public int UpdateDisb([FromBody] AckListViewModel ackListViewModel)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var i in ackListViewModel.AckList)
                    {
                        var query = _context.Disbursement.Where(x => x.DisbursementNo == i.DisbursementNo).ToList();
                        foreach (var disb in query)
                        {
                            var dept = _context.Department.FirstOrDefault(x => x.DepartmentId == i.DepartmentId);
                            disb.AcknowledgedBy = dept.DepartmentRepId;
                            disb.DisbursedBy = i.AcknowledgedBy;
                            disb.Status = "Acknowledged";
                            disb.DisbursedBy = ackListViewModel.DisbursedBy;
                            _context.SaveChanges();
                        }
                    }

                    dbContextTransaction.Commit();
                    return 1;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }





        #endregion




        [Authorize(Roles = "Department Head, Acting Department Head")]
        [HttpGet]
        [Route("api/department/{id}")]
        public IEnumerable<BriefDepartment> GetDepartments(string id)
        {
            GlobalClass gc = new GlobalClass();
            return gc.ListDepartment(id);
        }


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
                _context.TransactionDetail.Where(m => (m.Remarks == "Partially Fulfilled" && m.TransactionRef.StartsWith("DISB")) || (m.Disbursement.Status == "In Transit" && m.TransactionRef.StartsWith("DISB"))).ToList();

            List<RequestByItemView> itemToLess = new List<RequestByItemView>();
            for (int i = 0; i < partiallyFulfilledRequest.Count; i++)
            {
                foreach (TransactionDetail current in partiallyFulfilledDisbursement)
                {
                    //find if the request has it's disbursement, if yes add to list to less
                    if (current.Disbursement.RequestId == partiallyFulfilledRequest[i].TransactionRef && current.ItemId == partiallyFulfilledRequest[i].ItemId)
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
                StationeryRetrievalApiModel modelToAdd = new StationeryRetrievalApiModel(current.ItemId, current.Description, current.DepartmentId, current.DepartmentName, current.Quantity);
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


        [HttpPost]
        [Authorize(Roles = RoleName.StoreClerk)]
        [Route("api/clerk/setretrievallist")]
        public IHttpActionResult SetRetrievalList(SetStationeryRetrievalApiModel apiModelToSet)
        {
            //remove entries that are ignored
            apiModelToSet.ApiModelList.RemoveAll(m => m.NewQuantity == 0 && m.Remarks == "");

            //This controller method will generate stationery retrieval, disbursement, stock adjustment
            string currentUserId = User.Identity.GetUserId(); //need to check if this works
            string newRetrievalId = GenerateRetrievalId();
            //create a new stationery retrieval with pending delivery
            StationeryRetrieval retrievalInDb = new StationeryRetrieval
            {
                RetrievalId = newRetrievalId,
                RetrievedBy = apiModelToSet.UserId,
                Date = DateTime.Today,
            };
            _context.StationeryRetrieval.Add(retrievalInDb);
            _context.SaveChanges();

            int newTransactionId = GenerateTransactionDetailId() - 1;//-1 to get last value, id will be increased in foreach loop
            //check if there is need to raise a stock adjustment, put to list to get total of each item and compare with warehouse
            string newStockAdjustmentId = GenerateStockAdjustmentId();
            List<string> items = apiModelToSet.ApiModelList.Select(m => m.ItemId).Distinct().ToList();
            Dictionary<string, StockAdjustmentValidate> stockAdjustmentDict = new Dictionary<string, StockAdjustmentValidate>();
            foreach (string item in items)
            {
                stockAdjustmentDict.Add(item, new StockAdjustmentValidate());
            }

            foreach (StationeryRetrievalApiModel current in apiModelToSet.ApiModelList)
            {
                stockAdjustmentDict[current.ItemId].RequestedQty += current.NeededQuantity.GetValueOrDefault();
                stockAdjustmentDict[current.ItemId].RetrievedQty += current.NewQuantity.GetValueOrDefault();
                stockAdjustmentDict[current.ItemId].WarehouseQty = _context.Stationery.Where(m => m.ItemId == current.ItemId).Select(m => m.QuantityWarehouse).FirstOrDefault();
                stockAdjustmentDict[current.ItemId].Remarks += current.Remarks;
            }

            bool stockAdjustmentRequired = false;
            foreach (KeyValuePair<string, StockAdjustmentValidate> currentValidate in stockAdjustmentDict)
            {
                //check retrieved<requested and retrieved!=warehouse qty, means there are damaged item, will automatically raise stock adjustment
                if (currentValidate.Value.RetrievedQty < currentValidate.Value.RequestedQty &&
                    currentValidate.Value.RetrievedQty != currentValidate.Value.WarehouseQty)
                {
                    //check if there are any stock adjustment raised, if yes it will skip as to prevent double raise
                    bool existingItemYetApproved = false;
                    List<StockAdjustment> stockAdjustmentNotApproved = _context.StockAdjustment.Where(m => m.ApprovedBy == null).ToList();
                    foreach (StockAdjustment current in stockAdjustmentNotApproved)
                    {
                        foreach (TransactionDetail subDetail in current.TransactionDetail)
                        {
                            if (subDetail.ItemId == currentValidate.Key)
                            {
                                existingItemYetApproved = true;
                                break;
                            }
                        }

                        if (existingItemYetApproved == true)
                        {
                            newTransactionId += 1;
                        }
                        break;
                    }

                    //create a stock adjustment if item does not have a stock adjustment
                    if (existingItemYetApproved == false)
                    {
                        newTransactionId += 1;
                        TransactionDetail transactionStockAdjustmentInDb = new TransactionDetail
                        {
                            TransactionId = newTransactionId,
                            ItemId = currentValidate.Key,
                            Quantity = currentValidate.Value.RetrievedQty - currentValidate.Value.WarehouseQty,
                            Remarks = currentValidate.Value.Remarks.IsNullOrWhiteSpace() ? "Stock adjustment from Mobile" : currentValidate.Value.Remarks,
                            TransactionRef = newStockAdjustmentId,
                            TransactionDate = DateTime.Now,
                            UnitPrice = _context.Stationery.FirstOrDefault(m => m.ItemId == currentValidate.Key).FirstSuppPrice,
                        };
                        _context.TransactionDetail.Add(transactionStockAdjustmentInDb);
                        _context.SaveChanges();
                        stockAdjustmentRequired = true;
                    }
                }
            }

            if (stockAdjustmentRequired)
            {
                StockAdjustment stockAdjustmentInDb = new StockAdjustment
                {
                    StockAdjId = newStockAdjustmentId,
                    PreparedBy = currentUserId,
                    Remarks = "Damage",
                    Date = DateTime.Now
                };
                _context.StockAdjustment.Add(stockAdjustmentInDb);
                _context.SaveChanges();
            }

            //preparing for disbursement
            List<string> departmentId = apiModelToSet.ApiModelList.Select(m => m.DepartmentId).Distinct().ToList();
            Dictionary<string, List<TransactionDetail>> keyTransactionList = new Dictionary<string, List<TransactionDetail>>();
            foreach (string depId in departmentId)
            {
                keyTransactionList.Add(depId, new List<TransactionDetail>());
            }

            foreach (StationeryRetrievalApiModel current in apiModelToSet.ApiModelList)
            {
                newTransactionId += 1;
                //create stationery TransactionDetail for retrieval
                TransactionDetail transactionInDb = new TransactionDetail
                {
                    TransactionId = newTransactionId,
                    ItemId = current.ItemId,
                    Quantity = current.NewQuantity.GetValueOrDefault(),
                    Remarks = "Retrieved",
                    TransactionRef = newRetrievalId,
                    TransactionDate = DateTime.Now,
                    UnitPrice = _context.Stationery.FirstOrDefault(m => m.ItemId == current.ItemId).FirstSuppPrice,
                };
                _context.TransactionDetail.Add(transactionInDb);
                _context.SaveChanges();

                //minus the quantity from stock and put it to in transit
                Stationery stationeryInDb = _context.Stationery.FirstOrDefault(m => m.ItemId == current.ItemId);
                stationeryInDb.QuantityWarehouse -= transactionInDb.Quantity;
                stationeryInDb.QuantityTransit += transactionInDb.Quantity;

                ////calculate amount of stock to raise less the amount pending approval
                //int quantityPendingApproval = _context.TransactionDetail.Where(m =>
                //        m.TransactionRef.StartsWith("SAD-") && m.StockAdjustment.ApprovedBy.IsNullOrWhiteSpace())
                //    .Sum(m => m.Quantity);

                ////automatically raise stock adjustment if quantity different
                //if (current.NewQuantity != current.NeededQuantity)
                //{
                //    newTransactionId += 1;
                //    TransactionDetail transactionStockAdjustmentInDb = new TransactionDetail
                //    {
                //        TransactionId = newTransactionId,
                //        ItemId = current.ItemId,
                //        Quantity = current.NewQuantity.GetValueOrDefault()-stationeryInDb.QuantityWarehouse+quantityPendingApproval,
                //        Remarks = current.Remarks.IsNullOrWhiteSpace()?"Stock adjustment from Mobile":current.Remarks,
                //        TransactionRef = newStockAdjustmentId,
                //        TransactionDate = DateTime.Now,
                //        UnitPrice = _context.Stationery.FirstOrDefault(m => m.ItemId == current.ItemId).FirstSuppPrice,
                //    };
                //    _context.TransactionDetail.Add(transactionStockAdjustmentInDb);
                //    _context.SaveChanges();
                //}

                //issue a disbursement with status in transit
                //for disbursement (transaction ref not initialized yet
                newTransactionId += 1;
                TransactionDetail transactionDisbursementInDb = new TransactionDetail
                {
                    TransactionId = newTransactionId,
                    ItemId = current.ItemId,
                    Quantity = current.NewQuantity.GetValueOrDefault(),
                    Remarks = "In Transit",
                    TransactionDate = DateTime.Now,
                    UnitPrice = _context.Stationery.FirstOrDefault(m => m.ItemId == current.ItemId).FirstSuppPrice,
                };
                keyTransactionList[current.DepartmentId].Add(transactionDisbursementInDb);
            }

            int newDisbIdWithoutPrefixInt = GenerateDisbursementIdSuffixOnly() - 1;//-1 to prepare for for loop, will + 1 for each loop
            int newDisbNoWithoutPrefixInt = GenerateDisbursementNoSuffixOnly() - 1;//-1 to prepare for for loop, will + 1 for each loop
            foreach (KeyValuePair<string, List<TransactionDetail>> pair in keyTransactionList)
            {
                //getting disbursementId
                newDisbIdWithoutPrefixInt += 1;
                string newDisbursementIdWithoutPrefixString = newDisbIdWithoutPrefixInt.ToString().PadLeft(6, '0');
                string disbursementId = "DISB" + newDisbursementIdWithoutPrefixString;

                //getting disbursementNo
                newDisbNoWithoutPrefixInt += 1;
                string newDisbursementNoWithoutPrefixString = newDisbNoWithoutPrefixInt.ToString().PadLeft(5, '0');
                string disbursementNo = "D" + pair.Key + newDisbursementNoWithoutPrefixString;
                StationeryRequest requestLinked = _context.StationeryRequest.OrderByDescending(m => m.RequestId)
                    .FirstOrDefault(m => m.Status == "Pending Disbursement" || m.Status == "Partially Fulfilled");
                Disbursement disbursementInDb = new Disbursement
                {
                    DisbursementId = disbursementId,
                    DisbursementNo = disbursementNo,
                    DepartmentId = pair.Key,
                    AcknowledgedBy = null,
                    DisbursedBy = currentUserId,
                    Date = DateTime.Now,
                    RequestId = requestLinked.RequestId,
                    Status = "In Transit",
                    OTP = GenerateOTP()
                };
                _context.Disbursement.Add(disbursementInDb);
                _context.SaveChanges();

                for (int i = 0; i < pair.Value.Count; i++)
                {
                    pair.Value[i].TransactionRef = disbursementId;
                }

                _context.TransactionDetail.AddRange(pair.Value);
                _context.SaveChanges();
            }

            //changing stationery records that matches to In Transit
            List<StationeryRequest> partiallyAndPendingRequest =
                _context.StationeryRequest.Where(m => m.Status == "Partially Fulfilled" || m.Status == "Pending Disbursement").ToList();
            for (int i = 0; i < apiModelToSet.ApiModelList.Count; i++)
            {
                for (int j = 0; j < partiallyAndPendingRequest.Count; j++)
                {
                    for (int k = 0; k < partiallyAndPendingRequest[j].TransactionDetail.Count; k++)
                    {
                        if (apiModelToSet.ApiModelList[i].DepartmentId == partiallyAndPendingRequest[j].DepartmentId && apiModelToSet.ApiModelList[i].ItemId == partiallyAndPendingRequest[j].TransactionDetail.ElementAt(k).ItemId)
                        {
                            string affectedStationeryRequestId = partiallyAndPendingRequest[j].RequestId;
                            StationeryRequest stationeryRequestedToModify =
                                _context.StationeryRequest.FirstOrDefault(m =>
                                    m.RequestId == affectedStationeryRequestId);
                            stationeryRequestedToModify.Status = "In Transit";
                            _context.SaveChanges();
                        }
                    }
                }
            }
            //foreach (StationeryRetrievalApiModel apiModel in apiModelToSet.ApiModelList)
            //{
            //    foreach (StationeryRequest stationeryRequest in partiallyAndPendingRequest)
            //    {
            //        foreach (TransactionDetail transactionDetail in stationeryRequest.TransactionDetail)
            //        {
            //            apiModelToSet.ApiModelList[i].DepartmentId == quest.DepartmentId && apiModel.ItemId == transactionDetail.ItemId)
            //            stationeryRequest
            //        }
            //    }
            //}
            return Ok();
        }


        //when retrieved remember to set to intransit and create a new retrieval record


        public string GenerateRetrievalId()
        {
            string prefix = "R" + DateTime.Today.Year + "-";
            int entries = _context.StationeryRetrieval.Where(m => m.RetrievalId.StartsWith(prefix)).ToList().Count;
            string suffix = (entries + 1).ToString().PadLeft(4, '0');
            string newRetrievalId = prefix + suffix;
            return newRetrievalId;
        }

        public int GenerateTransactionDetailId()
        {
            TransactionDetail lastItem = _context.TransactionDetail.OrderByDescending(m => m.TransactionId).First();
            int lastRequestId = lastItem.TransactionId;
            int newRequestId = lastRequestId + 1;
            return newRequestId;
        }

        public string GenerateStockAdjustmentId()
        {
            StockAdjustment lastItem = _context.StockAdjustment.OrderByDescending(m => m.StockAdjId).First();
            string lastStockAdjIdWithoutPrefix = lastItem.StockAdjId.Substring(4, 6);
            int newStockAdjIdWithoutPrefixInt = Int32.Parse(lastStockAdjIdWithoutPrefix) + 1;
            string newStockAdjIdWithoutPrefixString = newStockAdjIdWithoutPrefixInt.ToString().PadLeft(6, '0');
            string stockAdjId = "SAD-" + newStockAdjIdWithoutPrefixString;
            return stockAdjId;
        }

        public int GenerateDisbursementIdSuffixOnly()
        {
            Disbursement lastItem = _context.Disbursement.OrderByDescending(m => m.DisbursementId).First();
            string lastDisbursementIdWithoutPrefix = lastItem.DisbursementId.Substring(4, 6);
            int newDisbursementIdWithoutPrefixInt = Int32.Parse(lastDisbursementIdWithoutPrefix) + 1;
            return newDisbursementIdWithoutPrefixInt;
        }

        public int GenerateDisbursementNoSuffixOnly()
        {
            Disbursement lastItem = _context.Disbursement.OrderByDescending(m => m.DisbursementId).First();
            string lastDisbursementNoWithoutPrefix = lastItem.DisbursementNo.Substring(5, 5);
            int newDisbursementNoWithoutPrefixInt = Int32.Parse(lastDisbursementNoWithoutPrefix) + 1;
            return newDisbursementNoWithoutPrefixInt;
        }

        public string GenerateOTP()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int num = rand.Next(0, chars.Length - 1);
            string OTP = chars[num] + rand.Next(0, 1000).ToString().PadLeft(3, '0');
            return OTP;
        }
        #endregion

        #region Author:Kay Thi Swe Tun
        [HttpGet]
        [Authorize(Roles = RoleName.StoreClerk)]
        [Route("api/clerk/voiddisb/{disbno}")]
        public bool GetDisbursementVoid(string disbno)
        {

            GlobalClass gc = new GlobalClass();
            return gc.VoidDisbursement(disbno);



        }

        #endregion

    }
}