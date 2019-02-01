using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;
using Team7ADProjectApi.Models;

namespace Team7ADProject.Controllers
{
    //For DH or ADH to approve or reject request
    [RoleAuthorize(Roles = "Department Head, Acting Department Head")]
    public class ManageRequestController : Controller
    {
        private readonly LogicDB _context;

        public ManageRequestController()
        {
            _context = new LogicDB();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #region Author : Kay Thi Swe Tun
        // GET: ManageRequest
        public ActionResult Index()
        {
            List<RequestedItemViewModel> models = new List<RequestedItemViewModel>();

            ViewBag.ShowItems = false;
            LogicDB context = new LogicDB();
            string userId = User.Identity.GetUserId();
            string depId = context.AspNetUsers.FirstOrDefault(x => x.Id == userId).DepartmentId;


            List<StationeryRequest> requests = context.StationeryRequest.Where(x => x.Status == "Pending Approval" && x.DepartmentId == depId).ToList();

            if (requests.Count > 0)
            {
                ViewBag.ShowItems = true;

                foreach (var c in requests)
                {
                    RequestedItemViewModel reqItem = new RequestedItemViewModel();
                    reqItem.RequestDate = String.Format("{0:dd/MM/yyyy}", c.RequestDate);
                    AspNetUsers emp = context.AspNetUsers.FirstOrDefault(x => x.Id == c.RequestedBy);
                    reqItem.Empname = emp.EmployeeName;
                    var requestId = c.RequestId;
                    reqItem.RequestID = c.RequestId;
                    List<TransactionDetail> transactionDetails = context.TransactionDetail.Where(x => x.TransactionRef == requestId).ToList();
                    List<Stationery> items = new List<Stationery>();
                    foreach (var i in transactionDetails)
                    {
                        Stationery item = context.Stationery.FirstOrDefault(x => x.ItemId == i.ItemId);
                        item.QuantityTransit = i.Quantity;
                        items.Add(item);

                    }
                    reqItem.Itemlist = items;
                    models.Add(reqItem);
                }

            }
            return View(models);
        }

        [HttpPost]
        public ActionResult Edit(StationeryRequest requests)
        {
            string result = null;
            
            if (requests != null)
            {
                result = "data include";
            
            string userId = User.Identity.GetUserId();

            var stationery = _context.StationeryRequest.Find(requests.RequestId);

               

            var stationerytran = _context.TransactionDetail.Where(x=> x.TransactionRef == requests.RequestId).ToList();
            if (stationery == null)
                return HttpNotFound();

            else
            {
                    //for collection date check
                    {
                        var dep = _context.Department.Where(x => x.DepartmentId == stationery.DepartmentId).FirstOrDefault();
                        if (dep.NextAvailableDate > DateTime.Now)
                        {
                            _context.Entry(stationery).Property("CollectionDate").CurrentValue = dep.NextAvailableDate;
                        }
                        else
                        {
                            DateTime nextMon = Next(DateTime.Today, DayOfWeek.Monday);
                            _context.Entry(stationery).Property("CollectionDate").CurrentValue = nextMon;
                        }
                        
                    }
                _context.Entry(stationery).Property("Status").CurrentValue = requests.Status;
                _context.Entry(stationery).Property("ApprovedBy").CurrentValue = userId;
                _context.Entry(stationery).Property("Comment").CurrentValue = requests.Comment;
                _context.SaveChanges();
            }
            if (stationerytran == null)
                return HttpNotFound();

            else
            {
                foreach(TransactionDetail detail in stationerytran)
                {
                    _context.Entry(detail).Property("Remarks").CurrentValue = requests.Status;

                }

                _context.SaveChanges();
            }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public DateTime Next(DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }
        #endregion
    }
}