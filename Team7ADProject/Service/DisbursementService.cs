using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;
using Team7ADProject.ViewModels.ChargeBack;

namespace Team7ADProject.Service
{
    public sealed class DisbursementService
    {
        #region Singleton Design Pattern

        private static readonly DisbursementService instance = new DisbursementService();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static DisbursementService()
        {
        }

        private DisbursementService()
        {
        }

        public static DisbursementService Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        LogicDB context = new LogicDB();

        public int GetDeptCpId(string userId)
        {
            return context.AspNetUsers.FirstOrDefault(x => x.Id == userId).Department.CollectionPointId;
        }

        public string GetDeptCpName(string userId)
        {
            return context.AspNetUsers.FirstOrDefault(x => x.Id == userId).Department.CollectionPoint.CollectionDescription;
        }

        public bool UpdateCollectionPoint(string userId, int cpId)
        {
            var query = context.AspNetUsers.FirstOrDefault(x => x.Id == userId);
            var dept = query.Department;
            var collPoint = context.CollectionPoint.FirstOrDefault(x => x.CollectionPointId == cpId);
            dept.CollectionPoint = collPoint;
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public IEnumerable<DepartmentChargeBackViewModel> GetDepartmentChargeBackViewModels(string userId)
        {
            LogicDB context = new LogicDB();

            String DID = context.Department.
                        Where(x => x.DepartmentHeadId == userId).
                        First().DepartmentId;

            var chargeback = context.Disbursement.
            Where(x => x.DepartmentId == DID).ToList().
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

            return chargeback;
        }

        public IEnumerable<DepartmentChargeBackViewModel> GetDepartmentChargeBackByDate(DateTime? fromDTP, DateTime? toDTP, string userId)
        {
            if ((fromDTP == null) || (toDTP == null))
            {
                return null;
            }

            String DID = context.Department
                        .Where(x => x.DepartmentHeadId == userId)
                        .First().DepartmentId;

            var chargeback = context.Disbursement
                            .Where(x => x.DepartmentId == DID && x.Date >= fromDTP && x.Date <= toDTP).ToList()
                            .Select(x => new DepartmentChargeBackViewModel()
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

            return chargeback;
        }


        public IEnumerable<DisbursementDetailsViewModel> GetChargeBackDetails(string disNo, string userId)
        {
            String DID = context.Department
                        .Where(x => x.DepartmentHeadId == userId)
                        .First().DepartmentId;

            String QueryDeptID = context.Disbursement.Where(y => y.DisbursementId == disNo).FirstOrDefault().DepartmentId;

            if (DID == QueryDeptID)
            {
                var disbursement = context.TransactionDetail.Where(x => x.TransactionRef == disNo).ToList().
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
                return disbursement;
            }
            else { return null; }
        }

        public DataTable GetReportData(DateTime? fromDTP, DateTime? toDTP, string userId)
        {
            String DID = context.Department
                        .Where(x => x.DepartmentHeadId == userId)
                        .First().DepartmentId;

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
                Select(y => new { y.DisbursementId, y.AspNetUsers1.EmployeeName, y.Date, y.RequestId, y.Status });

            foreach (var i in chargeback)
            {
                dt.Rows.Add(i.DisbursementId, i.EmployeeName, i.Date, i.RequestId, i.Status);
            }

            return dt;
        }

        public List<BriefDept> GetBriefDept()
        {
            List<BriefDept> deptList = (from x in context.Department
                                        select new BriefDept
                                        {
                                            DepartmentId = x.DepartmentId,
                                            DepartmentName = x.DepartmentName
                                        }).ToList();
            return deptList;
        }

        public BriefDept GetBriefDept(string deptId)
        {
            BriefDept deptList = (from x in context.Department
                                where x.DepartmentId == deptId
                                select new BriefDept
                                {
                                    DepartmentId = x.DepartmentId,
                                    DepartmentName = x.DepartmentName
                                }).FirstOrDefault();
            return deptList;
        }

    }
}