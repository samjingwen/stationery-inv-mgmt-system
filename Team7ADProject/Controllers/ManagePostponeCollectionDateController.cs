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
        #region Author:Lynn Lynn Oo

        LogicDB _context;
        public ManagePostponeCollectionDateController()
        {
            _context = new LogicDB();
        }
        // GET: ManagePostponeCollectionDate
        public ActionResult Index()
        {
            List<string> pendingDisbureID = _context.StationeryRequest.Where(x => x.Status == "Pending Disbursement").Select(x => x.RequestId).ToList();
            List<StationeryRequest> Stationeries = new List<StationeryRequest>();
            foreach(string current in pendingDisbureID)
            {
                List<StationeryRequest> itemsineachreq = _context.StationeryRequest.Where(v => v.RequestId == current).ToList();
                Stationeries.AddRange(itemsineachreq);
            }
            List<PostponeCollDateViewModel> viewModel = new List<PostponeCollDateViewModel>();
            foreach(StationeryRequest current in Stationeries)
            {
                PostponeCollDateViewModel viewModeltwo = new PostponeCollDateViewModel();
                viewModeltwo.RequestID = current.RequestId;
                viewModeltwo.CollectionDate = (DateTime)current.CollectionDate;
                viewModel.Add(viewModeltwo);
            }
                return View(viewModel);
        }
    }
    #endregion
}