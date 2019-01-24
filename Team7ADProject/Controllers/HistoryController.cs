using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;
using Microsoft.AspNet.Identity;

//Author Cheng Zongpei
namespace Team7ADProject.Controllers
{
    //For department head to view history and for employee to view history
    public class HistoryController : Controller
    {
        private LogicDB context = new LogicDB();
        // GET: History
        [Authorize(Roles = "Department Head, Department Representative")]
        public ActionResult Index(DateTime? fromDTP, DateTime? toDTP)
        {
            string UID = User.Identity.GetUserId();
            string DID = context.AspNetUsers.Single(x => x.Id == UID).DepartmentId;
            if(fromDTP == null && toDTP == null)
            {
                return View(context.StationeryRequest.Where(x=>x.DepartmentId == DID).ToList());
            }
            else if(fromDTP == null)
            {
                return View(context.StationeryRequest.Where(x => x.DepartmentId == DID).Where(x => x.RequestDate <= toDTP).ToList());
            }
            else if(toDTP == null)
            {
                return View(context.StationeryRequest.Where(x => x.DepartmentId == DID).Where(x => x.RequestDate >= fromDTP).ToList());
            }
            else
            {
                return View(context.StationeryRequest.Where(x => x.DepartmentId == DID).Where(x => x.RequestDate >= fromDTP && x.RequestDate <= toDTP).ToList());
            }
        }

        [Authorize(Roles = "Department Head, Department Representative")]
        public ActionResult Detail(string id)
        {
            StationeryRequest stationeryRequest = context.StationeryRequest.Single(x => x.RequestId == id);
            HistoryDetailViewModel detailModel = new HistoryDetailViewModel()
            {
                Request = stationeryRequest,
                Details = context.TransactionDetail.Where(x => x.StationeryRequest.RequestId == stationeryRequest.RequestId).ToList()
            };
            return View(detailModel);
        }


    }
}