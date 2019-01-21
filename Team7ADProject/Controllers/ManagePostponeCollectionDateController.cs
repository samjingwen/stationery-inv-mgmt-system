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
    public class ManagePostponeCollectionDateController : Controller
    {
        LogicDB _context;
        #region Author:Lynn Lynn Oo
        public ManagePostponeCollectionDateController()
        {
            _context = new LogicDB();
        }
        // GET: ManagePostponeCollectionDate
        [Authorize(Roles = "Store Supervisor")]
        public ActionResult Index()
        {
            //string userid = User.Identity.GetUserId();
            //string depId = _context.AspNetUsers.Where(x => x.Id == userid).Select(x => x.DepartmentId).First();
            //Store Supervisor can see all the Pending Disbursement of all depts
            List<StationeryRequest> pendingDisbursements = _context.StationeryRequest.Where(x => x.Status == "Pending Disbursement" /*&& x.DepartmentId==depId*/).ToList();
            return View(pendingDisbursements);

        }

        //GET: ManagePostponeCollectionDate/Details
       
        public ActionResult Details(string id)
        {
            List<TransactionDetail> ItemsByID = _context.TransactionDetail.Where(x => x.TransactionRef == id).ToList();
            return View(ItemsByID);
        }

        public ActionResult Postpone(string id)
        {
            StationeryRequest current = _context.StationeryRequest.First(m => m.RequestId == id);
            current.CollectionDate = current.CollectionDate.Value.AddDays(7);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
    }
}



