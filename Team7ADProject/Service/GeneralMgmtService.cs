using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;
using Team7ADProject.Models;

namespace Team7ADProject.Service
{
    public sealed class GeneralMgmtService
    {
        #region Singleton Design Pattern

        private static readonly GeneralMgmtService instance = new GeneralMgmtService();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static GeneralMgmtService()
        {
        }

        private GeneralMgmtService()
        {
        }

        public static GeneralMgmtService Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        LogicDB context = new LogicDB();

        public void AssignDelegateHead(string userId, string selectedUser, string deptId, DateTime startDate, DateTime endDate)
        {
            DelegationOfAuthority doaInDb = new DelegationOfAuthority
            {
                DelegatedBy = userId,//"b36a58f3-51f9-47eb-8601-bcc757a8cadb";//selected Employee ID;
                DelegatedTo = selectedUser,
                StartDate = startDate,//new DateTime(2017,3,5);
                EndDate = endDate,//new DateTime(2017, 5, 5);
                DepartmentId = deptId
            };

            context.DelegationOfAuthority.Add(doaInDb);
            context.SaveChanges();
        }

        public string[] GetDelegatedHead(string userId)
        {
            DateTime todayDate = DateTime.Now.Date;
            LogicDB context = new LogicDB();

            var query = context.DelegationOfAuthority.Where(x => x.EndDate >= todayDate && x.DelegatedBy == userId).FirstOrDefault();

            if (query == null)
            {
                return null;
            }
            else
            {
                return new string[] { query.AspNetUsers1.Id, query.AspNetUsers1.EmployeeName };
            }
        }


    }
}