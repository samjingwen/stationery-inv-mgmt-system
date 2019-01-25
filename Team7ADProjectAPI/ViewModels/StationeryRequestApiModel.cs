using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProjectApi.Entities;

namespace Team7ADProjectApi.ViewModels
{
    public class StationeryRequestApiModel
    {
       public String RequestId { get; set; }
       public String RequestedBy { get; set; }
       public String ApprovedBy { get; set; }
       public String DepartmentId { get; set; }
        public String Status { get; set; }
        public DateTime RequestDate { get; set; }
        public List<TransactionDetail> transactionDetailsList { get; set; }
    }
}