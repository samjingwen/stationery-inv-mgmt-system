using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProjectApi.Entities;

namespace Team7ADProjectApi.ViewModels
{
    public class AckListViewModel
    {
        public string DisbursedBy { get; set; }
        public List<AckDisbViewModel> AckList { get; set; }
    }

    public class AckDisbViewModel
    {
        public string DisbursementNo { get; set; }
        public string DepartmentId { get; set; }
        public string AcknowledgedBy
            {
                get
                {
                    if (DepartmentId != null)
                    {
                        LogicDB context = new LogicDB();
                        return context.Department.FirstOrDefault(x => x.DepartmentId == DepartmentId).DepartmentRepId;
                    }
                    else
                        return null;
                }
            }

        public string OTP { get; set; }
        public string ItemId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int ReceivedQty { get; set; }
        public string status { get; set; }
    }
}