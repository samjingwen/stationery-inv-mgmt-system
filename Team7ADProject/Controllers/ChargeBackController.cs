using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;
using ClosedXML.Excel;
using Team7ADProject.ViewModels.ChargeBack;
using System.Data;
using System.IO;
using Team7ADProject.Service;

namespace Team7ADProject.Controllers
{
    //Author: Elaine Chan
    //For generating charge back and viewing charge back
    [RoleAuthorize(Roles = "Department Head")]
    public class ChargeBackController : Controller
    {
        #region Author:Elaine
        DisbursementService dService = DisbursementService.Instance;

        // GET: ChargeBack
        public ActionResult ChargeBack()
        {
            String userId = User.Identity.GetUserId();

            return View(dService.GetDepartmentChargeBackViewModels(userId));
        }

        [HttpPost]
        public ActionResult ChargeBack(DateTime? fromDTP, DateTime? toDTP)
        {
            if ((fromDTP == null) || (toDTP == null))
            {
                return HttpNotFound();
            }

            String userId = User.Identity.GetUserId();

            var chargeback = dService.GetDepartmentChargeBackByDate(fromDTP, toDTP, userId);

            return View(chargeback);
        }

        public ActionResult ChargeBackDetails(string id)
        {
            String userId = User.Identity.GetUserId();

            var disbursement = dService.GetChargeBackDetails(id, userId);

            if (disbursement != null)
            {
                return View(disbursement);
            }
            else { return HttpNotFound(); }
        }

        [HttpPost]
        public FileResult ExportRpt(DateTime? fromDTP, DateTime? toDTP)
        {
            String userId = User.Identity.GetUserId();

            DataTable dt = dService.GetReportData(fromDTP, toDTP, userId);

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream st = new MemoryStream())
                {
                    wb.SaveAs(st);
                    return File(st.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Chargeback_Report.xlsx");
                }
            }

        }

        #endregion

    }
}