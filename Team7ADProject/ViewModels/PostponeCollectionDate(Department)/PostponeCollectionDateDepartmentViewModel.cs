using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.ViewModels
{
    public class PostponeCollectionDateDepartmentViewModel
    {
        #region Author:Lynn Lynn Oo
        public string DepartmentID { get; set; }
        public string RequestBy { get; set; }
        public string RequestID { get; set; }
        public string Status { get; set; }
        public DateTime CollectionDate { get; set; }

        #endregion
    }
}