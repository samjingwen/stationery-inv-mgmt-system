using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    public class DelegateHeadViewModel
    {
        public string CurrentUser { get; set; }
        public DelegateHeadViewModel(String usrId)
        {
            CurrentUser = usrId;
        }
        public string DepartmentName
        {
            get
            {
                LogicDB context = new LogicDB();
                String depId = context.AspNetUsers.Where(x => x.Id == CurrentUser).Select(x => x.DepartmentId).First();

                string s = context.Department.Where(x=>x.DepartmentId== depId).Select(x => x.DepartmentName).First();
                return s;

            }
        }
        public DelegateHeadViewModel()
        {
           
        }
        public string SelectedUser { get; set; }
        public List<AspNetUsers> DelegateHead
        {
            get
            {
                LogicDB context = new LogicDB();
                String depId = context.AspNetUsers.Where(x => x.Id == CurrentUser).Select(x => x.DepartmentId).First();
                Console.WriteLine("dep Id --------------------------"+depId);
                return context.AspNetUsers.Where(x=> x.DepartmentId==depId).ToList(); 
            }
        }
        public List<String> EName
        {
            get
            {
                LogicDB context = new LogicDB();

                return context.AspNetUsers.Select(x => x.Id).ToList();
            }
        }
    }
}