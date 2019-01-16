using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
//using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
//Authors:Lynn Lynn Oo
namespace Team7ADProject.Controllers
{
    //For employee/department rep can view their own requisition histories
    public class RequisitionHistoryController : Controller
    {
        #region Lynn Lynn Oo
        private LogicDB _context;

        public RequisitionHistoryController()
        {
            _context = new LogicDB();
        }

        // GET: RequisitionHistory
        // To view the Requisition History
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId();
            string depId = _context.AspNetUsers.Where(x => x.Id == userid).Select(x => x.DepartmentId).First();
            var stationery = _context.StationeryRequest.Where(x => x.DepartmentId==depId).ToList();
            return View(stationery);
        }

        //RequisitionHistory/detail
        //public ActionResult Detail(string id)
        //{
        //    var stationerydetail = _context.StationeryRequest.Include(y => y.RequestId);
        //    return View(stationerydetail);

        //}
        //public async Task<ActionResult> Index(string searchString)
        //{
        //    var request = from m in _context.StationeryRequest
        //                 select m;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        request = request.Where(s => s.DepartmentId.Contains(searchString));
        //    }

        //    return View(await request.ToListAsync());
        //}
        #endregion  
    }
}
