using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProjectApi.Entities;
using Team7ADProjectApi.ViewModels;

namespace Team7ADProjectApi
{
    public class GlobalClass
    {
        LogicDB context = new LogicDB();

        #region Author: Sam Jing Wen

        public List<BriefDepartment> ListDepartment(string id)
        {
            var query = from x in context.Department
                        join y in context.AspNetUsers
                        on x.DepartmentRepId equals y.Id
                        where x.DepartmentId == id
                        select new BriefDepartment
                        {
                            DepartmentId = x.DepartmentId,
                            DepartmentName = x.DepartmentName,
                            DepartmentRepName = y.EmployeeName,
                            DepartmentRepId = x.DepartmentRepId

                        };
            return query.ToList();
        }

        public List<RequestItems> ListRequestByItem()
        {
            var query = (from x in context.RequestByItemView select new RequestItems
                            {
                               ItemId = x.ItemId,
                               Description = x.Description
                            }).Distinct().ToList();
            return query;
        }

        #endregion




        #region Author : Kay Thi Swe Tun
        //public List<DepEmp> ListEmp(string id)
        //{
        //    var query = from y in context.AspNetUsers
        //                where y.DepartmentId == id
        //                select new DepEmp
        //                {
        //                     String name;
        //    String empid;
        //    String email;
        //    String phone;

        //    DepartmentId = x.DepartmentId,
        //                    DepartmentName = x.DepartmentName,
        //                    DepartmentRepName = y.EmployeeName,
        //                    DepartmentRepId = x.DepartmentRepId

        //                };
        //    return query.ToList();
        //}

        #endregion

    }
}