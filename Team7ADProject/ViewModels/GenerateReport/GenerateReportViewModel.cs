using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels.GenerateReport
{
    //Author: Elaine Chan

    public class GenerateReportViewModel
    {
        [Required]
        public DateTime fDate { get; set; }
        [Required]
        public DateTime tDate { get; set; }

        public string module { get; set; }
        public string deptID { get; set; }

        public List<string> statcategory { get; set; }
        public List<string> entcategory { get; set; }
        public List<string> selectstatcategory { get; set; }
        public List<string> selectentcategory { get; set; }
        public List<string> supplier { get; set; }
        public List<string> employee { get; set; }

        public ChartViewModel stattimeDP { get; set; }
        public ChartViewModel enttimeDP { get; set; }
        public ChartViewModel statDP { get; set; }
        public ChartViewModel deptDP { get; set; }
        public List<ChartViewModel> data { get; set; }
        public List<ChartViewModel> entdata { get; set; }

        public static GenerateReportViewModel InitGRVM(string DID)
        {
            var grvm = new GenerateReportViewModel
            {
                fDate = new DateTime(2017, 1, 1),
                tDate = DateTime.Today,
                module = "Disbursements",
                deptID = DID,
                statcategory = new List<string>(),
                entcategory = new List<string>(),
                employee = new List<string>(),
                supplier = new List<string>(),
                selectentcategory = new List<string>(),
                selectstatcategory = new List<string>(),
                data = new List<ChartViewModel>(),
                entdata = new List<ChartViewModel>(),
                stattimeDP = new ChartViewModel("Breakdown by Stationery over Time", "", new List<StringDoubleDPViewModel>()),
                enttimeDP = new ChartViewModel("Breakdown by Entity over Time","", new List<StringDoubleDPViewModel>()),
                statDP = new ChartViewModel("Breakdown by Stationery Category", "", new List<StringDoubleDPViewModel>()),
                deptDP = new ChartViewModel("Breakdown by Entity", "", new List<StringDoubleDPViewModel>())

            };
            LogicDB context = new LogicDB();

            var slist = context.Stationery.GroupBy(x => x.Category).Select(y => y.Key);
            foreach (var l in slist)
            {
                grvm.statcategory.Add(l);
            }
            grvm.selectstatcategory = grvm.statcategory;

            if (DID == "STAT")
            { 
            var sslist = context.PurchaseOrder.GroupBy(x => x.SupplierId).Select(y => y.Key);
            foreach (var l in sslist) { grvm.supplier.Add(l); }

            var eelist = context.StationeryRetrieval.GroupBy(x => x.AspNetUsers.EmployeeName).Select(y => y.Key);
            foreach (var l in eelist) { grvm.employee.Add(l); }
                        
            var elist = context.Department.GroupBy(x => x.DepartmentId).Select(y => y.Key);
            foreach (var l in elist)
            {
                grvm.entcategory.Add(l);
            }
            grvm.selectentcategory = grvm.entcategory;
            }
            else
            {
                grvm.entcategory.Add(DID);
                grvm.selectentcategory.Add(DID);
                
            }

            return grvm;
        }

        public static GenerateReportViewModel InitGRVM(string DID, DateTime? fromDateTP, DateTime? toDateTP, 
            string module, List<string> selstatcat, List<string> seldeptcat, List<string> seleecat, List<string> selsscat)
        {
            LogicDB context = new LogicDB();
            var grvm = new GenerateReportViewModel
            {
                fDate = (DateTime)fromDateTP,
                tDate = (DateTime)toDateTP,
                module = module,
                deptID = DID,
                statcategory = new List<string>(),
                entcategory = new List<string>(),
                employee = new List<string>(),
                supplier = new List<string>(),
                selectentcategory = new List<string>(),
                selectstatcategory = new List<string>(),
                data = new List<ChartViewModel>(),
                entdata = new List<ChartViewModel>(),
                stattimeDP = new ChartViewModel("Breakdown by Stationery over Time", "", new List<StringDoubleDPViewModel>()),
                enttimeDP = new ChartViewModel("Breakdown by Entity over Time", "", new List<StringDoubleDPViewModel>()),
                statDP = new ChartViewModel("Breakdown by Stationery Category", "", new List<StringDoubleDPViewModel>()),
                deptDP = new ChartViewModel("Breakdown by Entity", "", new List<StringDoubleDPViewModel>())

            };

            var slist = context.Stationery.GroupBy(x => x.Category).Select(y => y.Key);
            foreach (var l in slist)
            {
                grvm.statcategory.Add(l);
            }

            if (selstatcat == null)
            {
                foreach (var l in grvm.statcategory)
                {
                    grvm.selectstatcategory.Add(l);
                }
            }
            else
            {
                foreach (var l in selstatcat)
                {
                    grvm.selectstatcategory.Add(l);
                }
            }

            if (DID == "STAT")
            {
                var sslist = context.PurchaseOrder.GroupBy(x => x.SupplierId).Select(y => y.Key);

                foreach (var l in sslist) { grvm.supplier.Add(l); }

                var eelist = context.StationeryRetrieval.GroupBy(x => x.AspNetUsers.EmployeeName).Select(y => y.Key);
                foreach (var l in eelist) { grvm.employee.Add(l); }

                var entlist = context.Department.GroupBy(x => x.DepartmentId).Select(y => y.Key);
                foreach (var l in entlist)
                {
                    grvm.entcategory.Add(l);
                }
                if (module == "Purchases")
                {
                    if (selsscat == null)
                        foreach (var l in sslist)
                        { grvm.selectentcategory.Add(l); }
                    else { foreach (var l in selsscat) { grvm.selectentcategory.Add(l); } }
                }

                if (module == "Retrieval")
                {
                    if (seleecat == null)
                        foreach (var l in eelist)
                        { grvm.selectentcategory.Add(l); }
                    else { foreach (var l in seleecat) { grvm.selectentcategory.Add(l); } }
                }
                else if(module == "Disbursements" || module == "ChargeBack" || module == "Requests")
                {
                    if (seldeptcat == null)
                    {
                        foreach (var l in grvm.entcategory) { grvm.selectentcategory.Add(l); }
                    }
                    else
                    {
                        foreach (var l in seldeptcat)
                        {
                            grvm.selectentcategory.Add(l);
                        }
                    }
                }
            }

            else
            {
                grvm.entcategory.Add(DID);
                grvm.selectentcategory.Add(DID);
            }

            

            return grvm;
        }


    }

   

}