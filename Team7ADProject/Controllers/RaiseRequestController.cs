using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    //Author: Teh Li Heng 17/1/2019
    //Raise new requests into a list of editable object and save all at once (implemented js and ajax)
    [RoleAuthorize(Roles = "Employee, Department Representative")]
    public class RaiseRequestController : Controller
    {
        #region Teh Li Heng
        static private LogicDB _context;

        public RaiseRequestController()
        {
            _context=new LogicDB();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            List<StationeryRequest> stationeryRequests = _context.StationeryRequest.Where(m => m.RequestedBy == userId && m.RequestDate==DateTime.Today).ToList();
            RaiseRequestWrapperViewModel viewModel = new RaiseRequestWrapperViewModel();

            //adding to raise request view model list
            List<RaiseRequestViewModel> viewResults = new List<RaiseRequestViewModel>();

            //if there are request today
            if (stationeryRequests.Count!=0)
            {
                foreach (var currentStationeryRequest in stationeryRequests)
                {
                    foreach (TransactionDetail current in currentStationeryRequest.TransactionDetail)
                    {
                        RaiseRequestViewModel entry = new RaiseRequestViewModel
                        {
                            Quantity = current.Quantity,
                            UnitOfMeasure = current.Stationery.UnitOfMeasure,
                            //Category = current.Stationery.Category,
                            Description = current.Stationery.Description
                        };
                        viewResults.Add(entry);
                    }

                    viewModel.Status = stationeryRequests[0].Status;
                    viewModel.Entries = viewResults;
                    viewModel.RequestDate = stationeryRequests[0].RequestDate;
                }
            }

            else
            {
                viewModel.Entries = new List<RaiseRequestViewModel>();
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Save(RaiseRequestViewModel[] requests)
        {
            string result = "Error! Request is incomplete!";
            bool validQuantity = false;
            if (requests!=null)
            {
                for (int i = 0; i < requests.Length; i++)
                {
                    validQuantity = requests[i].Quantity > 0;
                    if (validQuantity != true)
                        break;
                }
                if (!validQuantity)
                {
                    result = "Invalid input! Kindly raise a valid request.";
                }

                else
                {
                    string currentUserId = User.Identity.GetUserId();
                    AspNetUsers currentUser = _context.AspNetUsers.First(m => m.Id == currentUserId);


                    string newStationeryRequestId = GenerateRequestId();
                    if (requests != null)
                    {
                        StationeryRequest stationeryRequestInDb = new StationeryRequest
                        {
                            RequestId = newStationeryRequestId,
                            RequestedBy = currentUserId,
                            ApprovedBy = null,
                            DepartmentId = currentUser.DepartmentId,
                            Status = "Pending Approval",
                            Comment = null,
                            RequestDate = DateTime.Today,
                            CollectionDate = null
                        };

                        _context.StationeryRequest.Add(stationeryRequestInDb);
                        _context.SaveChanges();

                        foreach (var item in requests)
                        {
                            decimal itemPrice = _context.Stationery.Single(m => m.ItemId == item.Description).FirstSuppPrice;
                            TransactionDetail transactionDetailInDb = new TransactionDetail
                            {
                                TransactionId = GenerateTransactionDetailId(),
                                ItemId = item.Description,
                                Quantity = item.Quantity,
                                Remarks = "Pending Approval",
                                TransactionRef = newStationeryRequestId,
                                TransactionDate = DateTime.Today,
                                UnitPrice = itemPrice
                            };
                            _context.TransactionDetail.Add(transactionDetailInDb);
                        }
                        _context.SaveChanges();

                        //Sending email to department head to approve the request
                        //string depRepEmail = _context.Department.FirstOrDefault(m => m.DepartmentId == pair.Key).AspNetUsers1.Email;
                        string recipient = "team7logicdb@gmail.com"; //dummy email used
                        string title = "A request by " + currentUser.EmployeeName+" is raised";
                        string body = "Kindly check the details for the request.";
                        Email.Send(recipient, title, body);

                        result = "Success! Request is complete!";
                    }
                }
            }
            else
            {
                result = "List is empty.";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string GenerateRequestId()
        {
            StationeryRequest lastItem = _context.StationeryRequest.OrderByDescending(m => m.RequestId).First();
            string lastRequestIdWithoutPrefix = lastItem.RequestId.Substring(3, 7);
            int newRequestIdWithoutPrefixInt = Int32.Parse(lastRequestIdWithoutPrefix)+1;
            string newRequestIdWithoutPrefixString = newRequestIdWithoutPrefixInt.ToString().PadLeft(7, '0');
            string requestId = "Req" + newRequestIdWithoutPrefixString;
            return requestId;
        }

        public int GenerateTransactionDetailId()
        {
            TransactionDetail lastItem = _context.TransactionDetail.OrderByDescending(m => m.TransactionId).First();
            int lastRequestId = lastItem.TransactionId;
            int newRequestId = lastRequestId + 1;
            return newRequestId;
        }
        #endregion
    }
}