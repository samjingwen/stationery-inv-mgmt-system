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
                String DID = context.Department.Where(x=>x.DepartmentHeadId==UID).First().DepartmentId;
            
                var chargeback = context.Disbursement.Where(x=>x.DepartmentId == DID).ToList().Select(x => new DepartmentChargeBackViewModel()
                {
                    DisbursementId = x.DisbursementId,
                    DisbursementNo = x.DisbursementNo,
                    DepartmentId = x.DepartmentId,
                    Department = x.Department,
                    AcknowledgedBy = x.AcknowledgedBy,
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
        #endregion

    }
}