using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels.GenerateReport;

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

            var genRpt = context.TransactionDetail.ToList().Select(x => new GenerateReportViewModel()
            {
                DepartmentId = x.Disbursement.DepartmentId,
                Date = x.TransactionDate,
                Amount = (x.Quantity * x.UnitPrice)
            });

            return View(genRpt);
        }


        #endregion
    }
}