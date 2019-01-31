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
    public class ManagePostponeCollectionDateDepartmentController : Controller
    {
        #region Author:Lynn Lynn Oo

        LogicDB _context;
        public ManagePostponeCollectionDateDepartmentController()
        {
            _context = new LogicDB();
        }
        // GET: ManagePostponeCollectionDateDepartment
        [RoleAuthorize(Roles = "Department Head, Department Representative")]
        [HttpGet]
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId();
            string depId = _context.AspNetUsers.Where(x => x.Id == userid).Select(x => x.DepartmentId).First();
            var showinlist = _context.StationeryRequest.Where(x => x.DepartmentId == depId && x.Status == "Pending Disbursement").ToList();
            List<StationeryRequest> stationery = new List<StationeryRequest>();
            stationery.AddRange(showinlist);
            List<PostponeCollectionDateDepartmentViewModel> viewModel = new List<PostponeCollectionDateDepartmentViewModel>();
            foreach (StationeryRequest current in stationery)
            {
                PostponeCollectionDateDepartmentViewModel viewModeltwo = new PostponeCollectionDateDepartmentViewModel();
                viewModeltwo.DepartmentID = current.DepartmentId;
                viewModeltwo.RequestBy = current.AspNetUsers1.EmployeeName;
                viewModeltwo.RequestID = current.RequestId;
                viewModeltwo.Status = current.Status;
                viewModeltwo.CollectionDate = (DateTime)current.CollectionDate;
                viewModel.Add(viewModeltwo);
            }
            return View(viewModel);
        }
        //weeksPostponeDuration

        [RoleAuthorize(Roles = "Department Head, Department Representative")]
        [HttpPost]
        public ActionResult Postpone(int weeksPostponeDuration)
        {
            //code here
            return View();
        }

        // GET: ManagePostponeCollectionDateDepartment/Details
        [RoleAuthorize(Roles = "Department Head, Department Representative")]
        public ActionResult Details(string id)
        {
            List<TransactionDetail> ItemsByID = _context.TransactionDetail.Where(x => x.TransactionRef == id).ToList();
            return View(ItemsByID);
        }

        
    }
    #endregion
}
