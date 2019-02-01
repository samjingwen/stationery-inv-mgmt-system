using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    public class DelegateHeadViewModel
    {
        LogicDB context = new LogicDB();

        public string DeptHeadId { get; set; }

        public DelegateHeadViewModel(string userId)
        {
            DeptHeadId = userId;
        }

        public DelegateHeadViewModel() { }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string DepartmentName
        {
            get
            {
                if (DeptHeadId != null)
                    return context.Department.Where(x => x.DepartmentHeadId == DeptHeadId).FirstOrDefault().DepartmentName;
                else
                    return null;
            }
        }

        public string DepartmentId
        {
            get
            {
                if (DeptHeadId != null)
                    return context.Department.FirstOrDefault(x => x.DepartmentHeadId == DeptHeadId).DepartmentId;
                else
                    return null;
            }
        }

        public string DepartmentRepId
        {
            get
            {
                return context.Department.FirstOrDefault(x => x.DepartmentHeadId == DeptHeadId).DepartmentRepId;
            }
        }

        [Required]
        public string SelectedUser { get; set; }

        public List<AspNetUsers> DeptEmployees
        {
            get
            {
                if (DeptHeadId != null)
                {
                    return context.AspNetUsers.Where(x => x.DepartmentId == DepartmentId && x.Id != DeptHeadId && x.Id != DepartmentRepId).ToList();
                }
                else
                    return null;
            }
        }
    }
}