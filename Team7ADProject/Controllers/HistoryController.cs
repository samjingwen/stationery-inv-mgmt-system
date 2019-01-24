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
        private LogicDB _context = new LogicDB();
        // GET: History
        [Authorize(Roles = "Department Head, Department Representative")]
        public ActionResult Index(DateTime? fromDTP, DateTime? toDTP)
        {
            string userId = User.Identity.GetUserId();
            string departmentId = _context.AspNetUsers.Single(x => x.Id == userId).DepartmentId;
            if(fromDTP == null && toDTP == null)
            {
                return View(_context.StationeryRequest.Where(x=>x.DepartmentId == departmentId).ToList());
            }
            else if(fromDTP == null)
            {
                return View(_context.StationeryRequest.Where(x => x.DepartmentId == departmentId).Where(x => x.RequestDate <= toDTP).ToList());
            }
            else if(toDTP == null)
            {
                return View(_context.StationeryRequest.Where(x => x.DepartmentId == departmentId).Where(x => x.RequestDate >= fromDTP).ToList());
            }
            else
            {
                return View(_context.StationeryRequest.Where(x => x.DepartmentId == departmentId).Where(x => x.RequestDate >= fromDTP && x.RequestDate <= toDTP).ToList());
            }
        }

        [HttpGet]
        [Authorize(Roles = "Department Head, Department Representative")]
        public ActionResult Detail(string id)
        {
            StationeryRequest stationeryRequest = _context.StationeryRequest.Single(x => x.RequestId == id);
            string userId = User.Identity.GetUserId();
            AspNetUsers user = _context.AspNetUsers.FirstOrDefault(m => m.Id == userId);

            if (user.DepartmentId == stationeryRequest.DepartmentId)
            {
                HistoryDetailViewModel detailModel = new HistoryDetailViewModel()
                {
                    Request = stationeryRequest,
                    Details = _context.TransactionDetail
                        .Where(x => x.StationeryRequest.RequestId == stationeryRequest.RequestId).ToList()
                };
                return View(detailModel);
            }
            else
            {
                return View();
            }
        }


    }
}