using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    //Author:Lynn Lynn Oo
    public class PostponeCollDateViewModel
    {
        #region Author:Lynn Lynn Oo
        public string RequestBy { get; set; }
        public string DepartmentID { get; set; }
        public string RequestID { get; set; }
        public DateTime CollectionDate { get; set; }

        #endregion
    }

    public class BriefDept
    {
        public string DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/M/dd}")]
        public DateTime CollectionDate
        {
            get
            {
                LogicDB context = new LogicDB();
                var query = context.Department.FirstOrDefault(x => x.DepartmentId == DepartmentId);
                DateTime nextMonday = GlobalClass.GetNextWeekDay((DateTime) query.NextAvailableDate, DayOfWeek.Monday);
                DateTime comingMonday = GlobalClass.GetNextWeekDay(DateTime.Now, DayOfWeek.Monday);
                if (nextMonday > comingMonday)
                {
                    return nextMonday;
                }
                else
                {
                    return comingMonday;
                }
            }
        }
    }
}