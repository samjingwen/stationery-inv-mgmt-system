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
            List<StringDoubleDPViewModel> dataPoints = new List<StringDoubleDPViewModel>();

            LogicDB context = new LogicDB();
            var genRpt = context.TransactionDetail.GroupBy(x => new { x.Disbursement.DepartmentId }).
                Select(y => new { DeptID = y.Key.DepartmentId, TotalAmt = y.Sum(z => (z.Quantity * z.UnitPrice)) });

            foreach (var i in genRpt)
            {
                dataPoints.Add(new StringDoubleDPViewModel(i.DeptID, (double)i.TotalAmt));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }


        #endregion
    }
}