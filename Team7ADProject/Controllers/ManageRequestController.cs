using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    //For DH or ADH to approve or reject request
    public class ManageRequestController : Controller
    {

        #region Author : Kay Thi Swe Tun
        // GET: ManageRequest
        [Authorize(Roles = "Department Head, Acting Department Head")]
        public ActionResult Index()
        {

            List<RequestedItemViewModel> modellist = new List<RequestedItemViewModel>();

            ViewBag.ShowItems = false;
            LogicDB context = new LogicDB();
            string userId = User.Identity.GetUserId();
            string dep_id = context.AspNetUsers.Where(x => x.Id == userId).Select(x => x.DepartmentId).First();


           List<StationeryRequest> reqlist = context.StationeryRequest.Where(x => x.Status == "Pending Approval" && x.DepartmentId == dep_id).ToList();

            try
            {
                if (reqlist.Count > 0)
                {
                    ViewBag.ShowItems = true;

                    foreach (var c in reqlist)
                    {
                        RequestedItemViewModel req_item = new RequestedItemViewModel();
                        req_item.RequestDate = String.Format("{0:dd/MM/yyyy}", c.RequestDate);
                        AspNetUsers emp = context.AspNetUsers.Where(x => x.Id == c.RequestedBy).First();
                        req_item.Empname = emp.EmployeeName;
                        var reqid = c.RequestId;
                        req_item.RequestID = c.RequestId;
                        List<TransactionDetail> tran = context.TransactionDetail.Where(x => x.TransactionRef == reqid).ToList();
                        List<Stationery> items = new List<Stationery>();
                        foreach (var i in tran)
                        {
                            Stationery it = context.Stationery.Where(x => x.ItemId == i.ItemId).First();
                           it.QuantityTransit = i.Quantity;
                            items.Add(it);

                        }

                        req_item.Itemlist = items;
                        modellist.Add(req_item);

                    }

                }

                return View(modellist);
            }
            catch (Exception e)
            {
                Console.WriteLine("KTST " + e.Message);
            }

            return View(modellist);
        }

        [HttpPost]
        public ActionResult Edit(StationeryRequest requests)
        {
            LogicDB _context = new LogicDB();
            string result=null;
            if (requests != null)
            {
                result = "data include";
            }
            string userId = User.Identity.GetUserId();

            var stationery = _context.StationeryRequest.Find(requests.RequestId);
            if (stationery == null)
                return HttpNotFound();

            else
            {
                
                _context.Entry(stationery).Property("Status").CurrentValue = requests.Status;

                _context.Entry(stationery).Property("ApprovedBy").CurrentValue = userId;

                _context.Entry(stationery).Property("Comment").CurrentValue = requests.Comment;
                _context.SaveChanges();
            }
         
            return Json(result, JsonRequestBehavior.AllowGet);
        
    }


        #endregion
    }
}