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

        public ManagePostponeCollectionDateController()
        {
            _context = new LogicDB();
        }
        // GET: ManagePostponeCollectionDate
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId();
            string depId = _context.AspNetUsers.Where(x => x.Id == userid).Select(x => x.DepartmentId).First();
            List<StationeryRequest> pendingDisbursements = _context.StationeryRequest.Where(x => x.Status == "Pending Disbursement" && x.DepartmentId==depId).ToList();
            return View(pendingDisbursements);

        }

        //GET: ManagePostponeCollectionDate/D
        public ActionResult Details(string id)
        {
            List<TransactionDetail> ItemsByID = _context.TransactionDetail.Where(x => x.TransactionRef == id).ToList();
            return View(ItemsByID);
        }

        public ActionResult Postpone(string id, PostponeCollDateViewModel model)
        {
            string i = id;
            LogicDB context = new LogicDB();
            StationeryRequest dd = new StationeryRequest();
            dd.CollectionDate = model.CollectionDate;//new DateTime(2017,3,5)
            StationeryRequest current = _context.StationeryRequest.First(m => m.RequestId == id);
            current.CollectionDate = current.CollectionDate.Value.AddDays(7);
            _context.SaveChanges();
            return View();
        }
    }

//    public string ChangeDate(PostponeCollDateViewModel model)
//    {

//        LogicDB context = new LogicDB();
//        StationeryRequest dd = new StationeryRequest();       
//        dd.CollectionDate = model.CollectionDate;//new DateTime(2017,3,5)
//        return "success";

//    }
//}
}

