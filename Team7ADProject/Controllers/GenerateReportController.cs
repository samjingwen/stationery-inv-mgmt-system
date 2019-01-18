using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels.GenerateReport;
using Newtonsoft.Json;

namespace Team7ADProject.Controllers
{
    //For SS to generate reports
    //Author: Elaine Chan
    public class GenerateReportController : Controller
    {
        #region Author: Elaine Chan
        // GET: GenerateReport
        [Authorize(Roles = "Store Manager, Store Supervisor")]
        public ActionResult GenerateDashboard()
        {
            LogicDB context = new LogicDB();

            #region Disbursement by DeptID
            List<StringDoubleDPViewModel> deptdataPoints = new List<StringDoubleDPViewModel>();

            var gendeptRpt = context.TransactionDetail.GroupBy(x => new { x.Disbursement.DepartmentId }).
                Select(y => new { DeptID = y.Key.DepartmentId, TotalAmt = y.Sum(z => (z.Quantity * z.UnitPrice)) });

            foreach (var i in gendeptRpt)
            {
                deptdataPoints.Add(new StringDoubleDPViewModel(i.DeptID, (double)i.TotalAmt));
            }

            ViewBag.deptDataPoints = JsonConvert.SerializeObject(deptdataPoints);
            #endregion

            #region Disbursement by Stationery Category

            List<StringDoubleDPViewModel> statdataPoints = new List<StringDoubleDPViewModel>();

            var genstatRpt = context.TransactionDetail.Where(x => x.Disbursement.AcknowledgedBy != null).
                    GroupBy(y => new { y.Stationery.Category }).
                    Select(z => new { itemCat = z.Key.Category, totalAmt = z.Sum(a => (a.Quantity * a.UnitPrice)) });

            foreach (var i in genstatRpt)
            {
                statdataPoints.Add(new StringDoubleDPViewModel(i.itemCat, (double)i.totalAmt));
            }

            ViewBag.statDataPoints = JsonConvert.SerializeObject(statdataPoints);

            #endregion

            #region Disbursements over time

            List<StringDoubleDPViewModel> timedataPoints = new List<StringDoubleDPViewModel>();

            var timeRpt = context.TransactionDetail.Where(x => x.Disbursement.AcknowledgedBy != null).
                OrderBy(x => x.TransactionDate).
                GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).ToArray().
                Select(y => new { dateval = string.Format("{0} {1}", Enum.Parse(typeof(EnumMonth),y.Key.Month.ToString()), y.Key.Year), totalAmt = y.Sum(z => (z.Quantity * z.UnitPrice)) });

            foreach(var i in timeRpt)
            {
                timedataPoints.Add(new StringDoubleDPViewModel(i.dateval, (double)i.totalAmt));
            }

            ViewBag.timedataPoints = JsonConvert.SerializeObject(timedataPoints);
            #endregion

            return View();
        }

        [Authorize(Roles = "Store Manager, Store Supervisor")]
        [HttpPost]
        public ActionResult GenerateDashboard(DateTime? fromDateTP, DateTime? toDateTP)
        {
            LogicDB context = new LogicDB();

            #region Disbursement by DeptId
            List<StringDoubleDPViewModel> deptdataPoints = new List<StringDoubleDPViewModel>();
            var gendeptRpt = context.TransactionDetail.
                Where(x => x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP).
                GroupBy(y => new { y.Disbursement.DepartmentId }).
                Select(z => new { DeptID = z.Key.DepartmentId, TotalAmt = z.Sum(a => (a.Quantity * a.UnitPrice)) });

            foreach (var i in gendeptRpt)
            {
                deptdataPoints.Add(new StringDoubleDPViewModel(i.DeptID, (double)i.TotalAmt));
            }

            ViewBag.deptDataPoints = JsonConvert.SerializeObject(deptdataPoints);
            #endregion

            #region Disbursement by Stationery Category

            List<StringDoubleDPViewModel> statdataPoints = new List<StringDoubleDPViewModel>();
            var genstatRpt = context.TransactionDetail.
                Where(x => x.Disbursement.AcknowledgedBy != null && x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP).
                    GroupBy(y => new { y.Stationery.Category }).
                    Select(z => new { itemCat = z.Key.Category, totalAmt = z.Sum(a => (a.Quantity * a.UnitPrice)) });

            foreach (var i in genstatRpt)
            {
                statdataPoints.Add(new StringDoubleDPViewModel(i.itemCat, (double)i.totalAmt));
            }

            ViewBag.statDataPoints = JsonConvert.SerializeObject(statdataPoints);

            #endregion

            #region Disbursements over time

            List<StringDoubleDPViewModel> timedataPoints = new List<StringDoubleDPViewModel>();

            var timeRpt = context.TransactionDetail.Where(x => x.Disbursement.AcknowledgedBy != null && x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP).
                OrderBy(x => x.TransactionDate).
                GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).ToArray().
                Select(y => new { dateval = string.Format("{0} {1}", Enum.Parse(typeof(EnumMonth), y.Key.Month.ToString()), y.Key.Year), totalAmt = y.Sum(z => (z.Quantity * z.UnitPrice)) });

            foreach (var i in timeRpt)
            {
                timedataPoints.Add(new StringDoubleDPViewModel(i.dateval, (double)i.totalAmt));
            }

            ViewBag.timedataPoints = JsonConvert.SerializeObject(timedataPoints);

            #endregion

            return PartialView("_Charts");
        }


        #endregion
    }
}