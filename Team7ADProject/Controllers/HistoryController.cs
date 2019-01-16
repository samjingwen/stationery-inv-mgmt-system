using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

//Author Cheng Zongpei
namespace Team7ADProject.Controllers
{
    //For department head to view history and for employee to view history
    public class HistoryController : Controller
    {
        private LogicDB context = new LogicDB();
        // GET: History
        public ActionResult Index()
        {
            return View(context.StationeryRequest.ToList());
        }

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