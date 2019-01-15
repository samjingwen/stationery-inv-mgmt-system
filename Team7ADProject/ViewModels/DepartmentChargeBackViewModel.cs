using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
    //Author: Elaine Chan
{
    public class DepartmentChargeBackViewModel
    {
        #region Author: Elaine Chan
        // Use Case 25: View Department Charge Back
        public List<Disbursement> ListDisburse { get; set; }

        //public List<Disbursement> GetDeptChargeBack (String  DeptID, DateTime From, DateTime To)
        //{
        //    List<Disbursement> DChargeBack = new List<Disbursement>();
        //    LogicDB context = new LogicDB();
        //    DChargeBack = context.Disbursement.
        //        Where(x => x.DepartmentId == DeptID 
        //        && x.Date >= From 
        //        && x.Date <= To).ToList();

        //    return DChargeBack;
        //}

        ////testing here
        //public List<Disbursement> GetDisbursement()
        //{
        //    List<Disbursement> All = new List<Disbursement>();
        //    LogicDB context = new LogicDB();

        //    All = context.Disbursement.ToList();
        //    return All;
        //}

        //public List<Disbursement> GetAllChargeBack (DateTime From, DateTime To)
        //{
        //    List<Disbursement> AllChargeBack = new List<Disbursement>();
        //    LogicDB context = new LogicDB();

        //    AllChargeBack = context.Disbursement.Where(x => x.Date >= From && x.Date <= To).ToList();
        //    return AllChargeBack;
        //}
        #endregion
    }
}