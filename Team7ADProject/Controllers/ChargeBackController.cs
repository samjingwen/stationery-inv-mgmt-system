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

namespace Team7ADProject.Controllers
{
    //Author: Elaine Chan
    //For generating charge back and viewing charge back
    public class ChargeBackController : Controller
    {
        #region Author:Elaine
        LogicDB context = new LogicDB();

        // GET: ChargeBack
        [Authorize(Roles = "Department Head")]
        public ActionResult ChargeBack()
        {             
                LogicDB context = new LogicDB();
                String UID = User.Identity.GetUserId();
                String DID = context.Department.
                Where(x=>x.DepartmentHeadId==UID).
                First().DepartmentId;
            
                var chargeback = context.Disbursement.
                Where(x=>x.DepartmentId == DID).ToList().
                Select(x => new DepartmentChargeBackViewModel()
                {
                    DisbursementId = x.DisbursementId,
                    DisbursementNo = x.DisbursementNo,
                    DepartmentId = x.DepartmentId,
                    Department = x.Department,
                    AcknowledgedBy = context.AspNetUsers.Where(y=>y.Id== x.AcknowledgedBy).First().EmployeeName,                    
                    DisbursedBy = x.DisbursedBy,
                    Date = x.Date,
                    RequestId = x.RequestId,
                    StationeryRequest = x.StationeryRequest,
                    OTP = x.OTP,
                    Status = x.Status,
                    AspNetUsers = x.AspNetUsers,
                    AspNetUsers1 = x.AspNetUsers1,
                    TransactionDetail = x.TransactionDetail
                });

                return View(chargeback);

        }

        [Authorize(Roles = "Department Head")]
        [HttpPost]
        public ActionResult ChargeBack(DateTime? fromDTP, DateTime? toDTP)
        {if ((fromDTP== null) || (toDTP == null))
            {
                return HttpNotFound();
            }
            else
            {
                LogicDB context = new LogicDB();
                String UID = User.Identity.GetUserId();
                String DID = context.Department.
                Where(x => x.DepartmentHeadId == UID).
                First().DepartmentId;

                var chargeback = context.Disbursement.
                Where(x => x.DepartmentId == DID && x.Date >= fromDTP && x.Date <= toDTP).ToList().
                Select(x => new DepartmentChargeBackViewModel()
                {
                    DisbursementId = x.DisbursementId,
                    DisbursementNo = x.DisbursementNo,
                    DepartmentId = x.DepartmentId,
                    Department = x.Department,
                    AcknowledgedBy = context.AspNetUsers.Where(y => y.Id == x.AcknowledgedBy).First().EmployeeName,
                    DisbursedBy = x.DisbursedBy,
                    Date = x.Date,
                    RequestId = x.RequestId,
                    StationeryRequest = x.StationeryRequest,
                    OTP = x.OTP,
                    Status = x.Status,
                    AspNetUsers = x.AspNetUsers,
                    AspNetUsers1 = x.AspNetUsers1,
                    TransactionDetail = x.TransactionDetail
                });

                return View(chargeback);
            }
        }

        [Authorize(Roles = "Department Head")]
        public ActionResult ChargeBackDetails(String id)
        {
            LogicDB context = new LogicDB();
            String UID = User.Identity.GetUserId();
            String DID = context.Department.
            Where(x => x.DepartmentHeadId == UID).
            First().DepartmentId;

            String QueryDeptID = context.Disbursement.Where(y => y.DisbursementId == id).First().DepartmentId;

            if (DID == QueryDeptID)
            {

                var disbursement = context.TransactionDetail.Where(x => x.TransactionRef == id).ToList().
                    Select(x => new DisbursementDetailsViewModel()
                    {
                        DisbursementId = x.TransactionRef,
                        DisbursementNo = x.Disbursement.DisbursementNo,
                        DisbAcknowledgedBy = x.Disbursement.AspNetUsers1.EmployeeName,
                        DisbDate = x.Disbursement.Date,
                        DisbStatus = x.Disbursement.Status,
                        RequestId = x.Disbursement.RequestId,
                        RequestedBy = x.Disbursement.StationeryRequest.AspNetUsers1.EmployeeName,
                        RequestDepartmentId = x.Disbursement.StationeryRequest.DepartmentId,
                        ReqStatus = x.Disbursement.StationeryRequest.Status,
                        RequestDate = x.Disbursement.StationeryRequest.RequestDate,
                        ItemDesc = context.Stationery.Where(y => y.ItemId == x.ItemId).First().Description,
                        Quantity = x.Quantity,
                        Amount = (x.Quantity) * (x.UnitPrice)

                    });

                return View(disbursement);
            }
            else { return HttpNotFound(); }
        }
        
        [HttpPost]
        public FileResult ExportRpt(DateTime? fromDTP, DateTime? toDTP)
        {
            LogicDB context = new LogicDB();
            String UID = User.Identity.GetUserId();
            String DID = context.Department.
            Where(x => x.DepartmentHeadId == UID).
            First().DepartmentId;

            if ((fromDTP == null) || (toDTP == null))
            {
                fromDTP = new DateTime(2017, 1, 1);
                toDTP = DateTime.Today; 
            }

            DataTable dt = new DataTable("Report");
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("ChargeBack ID"),
            new DataColumn("Acknowledged By"),
                new DataColumn("Disbursement Date"),
                new DataColumn("RequestID"),
                new DataColumn("Status")});

            var chargeback = context.Disbursement.Where(x => x.DepartmentId == DID && x.Date >= fromDTP && x.Date <= toDTP).ToList().
                Select(y => new { y.DisbursementId, y.AspNetUsers.EmployeeName, y.Date, y.RequestId, y.Status });

            foreach (var i in chargeback)
            {
                dt.Rows.Add(i.DisbursementId, i.EmployeeName, i.Date, i.RequestId, i.Status);
            }

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