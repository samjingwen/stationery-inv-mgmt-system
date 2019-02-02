using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.Models;
using Team7ADProject.ViewModels;

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

        public void RevokeDelegateHead(string userId)
        {
            DateTime todayDate = DateTime.Now.Date;
            var query = context.DelegationOfAuthority.Where(x => x.EndDate >= todayDate && x.DelegatedBy == userId).Where(x => x.Status != "VOID").FirstOrDefault();
            query.Status = "VOID";
            context.SaveChanges();
        }

        public string[] GetDelegatedHead(string userId)
        {
            DateTime todayDate = DateTime.Now.Date;
            LogicDB context = new LogicDB();

            var query = context.DelegationOfAuthority.Where(x => x.EndDate >= todayDate && x.DelegatedBy == userId).Where(x => x.Status != "VOID").FirstOrDefault();

            if (query == null)
            {
                return null;
            }
            else
            {
                return new string[] { query.AspNetUsers1.Id, query.AspNetUsers1.EmployeeName, query.StartDate.ToShortDateString(), query.EndDate.ToShortDateString() };
            }
        }

        public string GetDeptIdOfUser(string userId)
        {
            return context.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault().DepartmentId;
        }

        public string GetCurrentRepName(string deptId)
        {
            var query = (from x in context.Department
                        join y in context.AspNetUsers
                        on x.DepartmentRepId equals y.Id
                        where x.DepartmentId == deptId
                        select y.EmployeeName).FirstOrDefault();
            return query;
        }

        public string GetCurrentRepId(string deptId)
        {
            var query = (from x in context.Department
                         join y in context.AspNetUsers
                         on x.DepartmentRepId equals y.Id
                         where x.DepartmentId == deptId
                         select y.Id).FirstOrDefault();
            return query;

        }


        public ManageRepViewModel GetManageRepViewModel(string deptId)
        {
            var query = (from x in context.AspNetUsers
                         join y in context.Department
                         on x.DepartmentId equals y.DepartmentId
                         where x.DepartmentId == deptId && x.Id != y.DepartmentHeadId && x.Id != y.DepartmentRepId
                         select x).ToList();

            //Check if any employee is already an acting department head, remove from list if so
            var modQuery = new List<AspNetUsers>(query);
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            foreach (var i in modQuery)
            {
                if (manager.IsInRole(i.Id, "Acting Department Head"))
                {
                    query.Remove(i);
                }
            }

            var users = query.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.EmployeeName
            });

            ManageRepViewModel model = new ManageRepViewModel() { UserList = users };

            return model;
        }

        public void UpdateDepartmentRep(string userId, string deptId)
        {
            var query = context.Department.FirstOrDefault(x => x.DepartmentId == deptId);
            query.DepartmentRepId = userId;
            context.SaveChanges();
        }

        public string GetUserEmail(string id)
        {
            LogicDB context = new LogicDB();
            return context.AspNetUsers.FirstOrDefault(x => x.Id == id).Email;
        }
        public bool IsAllRequestsApproved(string deptId)
        {
            var query = context.StationeryRequest.Where(x => x.DepartmentId == deptId && x.ApprovedBy == null).Count();
            if (query > 0)
                return false;
            else
                return true;
        }
    }
}