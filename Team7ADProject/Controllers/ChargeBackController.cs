using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;
using Team7ADProject.ViewModels.ChargeBack;

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
        public ActionResult ChargeBackDetails(String id)
        {
            LogicDB context = new LogicDB();
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
                    ItemDesc = context.Stationery.Where(y=>y.ItemId == x.ItemId).First().Description,
                    Quantity = x.Quantity,
                    Amount = (x.Quantity)*(x.UnitPrice)
                    
                });
            return View(disbursement);
        }
        #endregion

    }
}