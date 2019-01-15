using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    public class DelegateHeadViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string SelectedUser { get; set; }
        public List<AspNetUsers> DelegateHead
        {
            get
            {
                LogicDB context = new LogicDB();

                return context.AspNetUsers.ToList(); 
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