using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels.GenerateReport;
using Newtonsoft.Json;

namespace Team7ADProject.Controllers
{
    //For SS to generate reports
    //Author: Elaine Chan
    public class GenerateReportController : Controller
    {
        #region Author: Elaine Chan
        // GET: GenerateReport
        [Authorize(Roles = "Store Manager, Store Supervisor")]
        public ActionResult GenerateDashboard()
        {
            LogicDB context = new LogicDB();

            var grvm = new GenerateReportViewModel
            {
                fDate = new DateTime(2017, 1, 1),
                tDate = DateTime.Today,
                module = "Disbursements",
                statcategory = new List<string>(),
                entcategory = new List<string>(),
                employee = new List<string>(),
                supplier = new List<string>(),
                selectentcategory = new List<string>(),
                selectstatcategory = new List<string>(),
                stattimeDP = new ChartViewModel("Breakdown by Stationery over Time","", new List<StringDoubleDPViewModel>()),
                statDP = new ChartViewModel("Breakdown by Stationery Category","", new List<StringDoubleDPViewModel>()),
                deptDP = new ChartViewModel("Breakdown by Entity","", new List<StringDoubleDPViewModel>())

            };
            var slist = context.Stationery.GroupBy(x => x.Category).Select(y => y.Key);
            foreach (var l in slist)
            {
                grvm.statcategory.Add(l);
            }
            var sslist = context.PurchaseOrder.GroupBy(x => x.SupplierId).Select(y => y.Key);
            foreach (var l in sslist) { grvm.supplier.Add(l); }

            var eelist = context.StationeryRetrieval.GroupBy(x => x.AspNetUsers.EmployeeName).Select(y => y.Key);
            foreach (var l in eelist) { grvm.employee.Add(l); } 

            grvm.selectstatcategory = grvm.statcategory;
            var elist = context.Department.GroupBy(x => x.DepartmentId).Select(y => y.Key);
            foreach (var l in elist)
            {
                grvm.entcategory.Add(l);
            }
            grvm.selectentcategory = grvm.entcategory;

            #region Disbursement by DeptID

            var gendeptRpt = context.TransactionDetail.Where(x=> x.Disbursement.AcknowledgedBy != null).
                GroupBy(x => new { x.Disbursement.DepartmentId }).
                Select(y => new { DeptID = y.Key.DepartmentId, TotalAmt = y.Sum(z => (z.Quantity * z.UnitPrice)) });

            foreach (var i in gendeptRpt)
            {
                grvm.deptDP.datapoint.Add(new StringDoubleDPViewModel(i.DeptID, (double)i.TotalAmt));
            }
            
            #endregion

            #region Disbursement by Stationery Category
            
            var genstatRpt = context.TransactionDetail.Where(x => x.Disbursement.AcknowledgedBy != null).
                    GroupBy(y => new { y.Stationery.Category }).
                    Select(z => new { itemCat = z.Key.Category, totalAmt = z.Sum(a => (a.Quantity * a.UnitPrice)) });

            foreach (var i in genstatRpt)
            {
                grvm.statDP.datapoint.Add(new StringDoubleDPViewModel(i.itemCat, (double)i.totalAmt));
            }
            
            #endregion

            #region Disbursements over time

            var timeRpt = context.TransactionDetail.Where(x => x.Disbursement.AcknowledgedBy != null).
                OrderBy(x => x.TransactionDate).
                GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).ToArray().
                Select(y => new { dateval = string.Format("{0} {1}", Enum.Parse(typeof(EnumMonth), y.Key.Month.ToString()), y.Key.Year),
                    totalAmt = y.Sum(z => (z.Quantity * z.UnitPrice))});

            foreach (var i in timeRpt)
            {
                grvm.stattimeDP.datapoint.Add(new StringDoubleDPViewModel(i.dateval, (double)i.totalAmt));
            }
            
            #endregion

            return View(grvm);
        }

        [Authorize(Roles = "Store Manager, Store Supervisor")]
        [HttpPost]
        public ActionResult GenerateDashboard(DateTime? fromDateTP, DateTime? toDateTP, string module, List<string> selstatcat, List<string> seldeptcat, List<string> seleecat, List<string> selsscat)
        {
          
                LogicDB context = new LogicDB();

            #region Build VM
            var grvm = new GenerateReportViewModel
            {
                fDate = (DateTime)fromDateTP,
                tDate = (DateTime)toDateTP,
                module = module,
                statcategory = new List<string>(),
                entcategory = new List<string>(),
                employee = new List<string>(),
                supplier = new List<string>(),
                selectentcategory = new List<string>(),
                selectstatcategory = new List<string>(),
                stattimeDP = new ChartViewModel("Breakdown by Stationery over Time","", new List<StringDoubleDPViewModel>()),
                statDP = new ChartViewModel("Breakdown by Stationery Category","", new List<StringDoubleDPViewModel>()),
                deptDP = new ChartViewModel("Breakdown by Entity","", new List<StringDoubleDPViewModel>())

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
                    foreach(var l in sslist)
                { grvm.selectentcategory.Add(l); }
                else { foreach(var l in selsscat) { grvm.selectentcategory.Add(l); } }
            }
            
            if(module == "Retrieval")
            {
                if (seleecat == null)
                    foreach (var l in eelist)
                    { grvm.selectentcategory.Add(l); }
                else { foreach (var l in seleecat) { grvm.selectentcategory.Add(l); } }
            }
            else
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

            #endregion

            if (module =="Disbursements")
            { 

                #region Disbursement by DeptId
                
                        var gendeptRpt = context.TransactionDetail.
                            Where(x => x.Disbursement.AcknowledgedBy != null && 
                            grvm.selectstatcategory.Any(id=>id==x.Stationery.Category) && 
                            grvm.selectentcategory.Any(id=>id==x.Disbursement.DepartmentId) && 
                            x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP).
                            GroupBy(y => new { y.Disbursement.DepartmentId }).
                            Select(z => new { DeptID = z.Key.DepartmentId, TotalAmt = z.Sum(a => (a.Quantity * a.UnitPrice)) });

                        foreach (var i in gendeptRpt)
                        {
                            grvm.deptDP.datapoint.Add(new StringDoubleDPViewModel(i.DeptID, (double)i.TotalAmt));
                        }

                #endregion

                #region Disbursement by Stationery Category
                        
                        var genstatRpt = context.TransactionDetail.
                            Where(x => x.Disbursement.AcknowledgedBy != null &&
                            grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                            grvm.selectentcategory.Any(id => id == x.Disbursement.DepartmentId) && 
                            x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP).
                                GroupBy(y => new { y.Stationery.Category }).
                                Select(z => new { itemCat = z.Key.Category, totalAmt = z.Sum(a => (a.Quantity * a.UnitPrice)) });

                        foreach (var i in genstatRpt)
                        {
                            grvm.statDP.datapoint.Add(new StringDoubleDPViewModel(i.itemCat, (double)i.totalAmt));
                        }
                        
                #endregion

                #region Disbursements over time

                        var timeRpt = context.TransactionDetail.Where(x => x.Disbursement.AcknowledgedBy != null &&
                            grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                            grvm.selectentcategory.Any(id => id == x.Disbursement.DepartmentId) &&
                            x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP).
                                            OrderBy(x => x.TransactionDate).
                                            GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).ToArray().
                                            Select(y => new { dateval = string.Format("{0} {1}", Enum.Parse(typeof(EnumMonth), y.Key.Month.ToString()), y.Key.Year), totalAmt = y.Sum(z => (z.Quantity * z.UnitPrice)) });

                        foreach (var i in timeRpt)
                        {
                            grvm.stattimeDP.datapoint.Add(new StringDoubleDPViewModel(i.dateval, (double)i.totalAmt));
                        }
                        
                #endregion
            }

            if (module == "Requests")
            {
                #region Requests by Dept

                var gendeptRpt = context.TransactionDetail.
                    Where(x => x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP &&
                            grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                            grvm.selectentcategory.Any(id => id == x.StationeryRequest.DepartmentId)).
                    GroupBy(y=> new { y.StationeryRequest.DepartmentId}).
                    Select(z => new { DeptID = z.Key.DepartmentId, TotalAmt = z.Sum(a => (a.Quantity * a.UnitPrice)) });
                foreach (var i in gendeptRpt)
                {
                    grvm.deptDP.datapoint.Add(new StringDoubleDPViewModel(i.DeptID, (double)i.TotalAmt));
                }

                #endregion

                #region Requests by Stationery Category

                var genstatRpt = context.TransactionDetail.
                    Where(x => x.StationeryRequest.Status == "Completed" && 
                    x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP &&
                            grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                            grvm.selectentcategory.Any(id => id == x.StationeryRequest.DepartmentId)).
                        GroupBy(y => new { y.Stationery.Category }).
                        Select(z => new { itemCat = z.Key.Category, totalAmt = z.Sum(a => (a.Quantity * a.UnitPrice)) });

                foreach (var i in genstatRpt)
                {
                    grvm.statDP.datapoint.Add(new StringDoubleDPViewModel(i.itemCat, (double)i.totalAmt));
                }
                
                #endregion

                #region Requests over time
                
                var timeRpt = context.TransactionDetail.Where(x => x.StationeryRequest.Status == "Completed" 
                && x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP &&
                            grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                            grvm.selectentcategory.Any(id => id == x.StationeryRequest.DepartmentId)).
                    OrderBy(x => x.TransactionDate).
                    GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).ToArray().
                    Select(y => new { dateval = string.Format("{0} {1}", Enum.Parse(typeof(EnumMonth), y.Key.Month.ToString()), y.Key.Year), totalAmt = y.Sum(z => (z.Quantity * z.UnitPrice)) });

                foreach (var i in timeRpt)
                {
                    grvm.stattimeDP.datapoint.Add(new StringDoubleDPViewModel(i.dateval, (double)i.totalAmt));
                }
                
                #endregion

            }

            if(module == "ChargeBack")
            {
                #region Charge back by DeptId

                var gendeptRpt = context.TransactionDetail.
                    Where(x => x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP &&
                            grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                            grvm.selectentcategory.Any(id => id == x.Disbursement.DepartmentId)).
                    GroupBy(y => new { y.Disbursement.DepartmentId }).
                    Select(z => new { DeptID = z.Key.DepartmentId, TotalAmt = z.Sum(a => (a.Quantity * a.UnitPrice)) });

                foreach (var i in gendeptRpt)
                {
                    grvm.deptDP.datapoint.Add(new StringDoubleDPViewModel(i.DeptID, (double)i.TotalAmt));
                }

                #endregion

                #region Charge back by Stationery Category

                var genstatRpt = context.TransactionDetail.
                    Where(x => x.Disbursement.DepartmentId != null && x.TransactionDate >= fromDateTP && 
                    x.TransactionDate <= toDateTP &&
                            grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                            grvm.selectentcategory.Any(id => id == x.Disbursement.DepartmentId)).
                        GroupBy(y => new { y.Stationery.Category }).
                        Select(z => new { itemCat = z.Key.Category, totalAmt = z.Sum(a => (a.Quantity * a.UnitPrice)) });

                foreach (var i in genstatRpt)
                {
                    grvm.statDP.datapoint.Add(new StringDoubleDPViewModel(i.itemCat, (double)i.totalAmt));
                }
                
                #endregion

                #region Charge back over time
                
                var timeRpt = context.TransactionDetail.Where(x => x.Disbursement.DepartmentId != null && 
                x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP &&
                            grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                            grvm.selectentcategory.Any(id => id == x.Disbursement.DepartmentId)).
                    OrderBy(x => x.TransactionDate).
                    GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).ToArray().
                    Select(y => new { dateval = string.Format("{0} {1}", Enum.Parse(typeof(EnumMonth), y.Key.Month.ToString()), y.Key.Year), totalAmt = y.Sum(z => (z.Quantity * z.UnitPrice)) });

                foreach (var i in timeRpt)
                {
                    grvm.stattimeDP.datapoint.Add(new StringDoubleDPViewModel(i.dateval, (double)i.totalAmt));
                }
                
                #endregion
            }

            if (module == "Purchases")
            {
                #region Purchases by SupplierID

                var gendeptRpt = context.TransactionDetail.
                    Where(x => x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP &&
                    grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                            grvm.selectentcategory.Any(id => id == x.PurchaseOrder.SupplierId)).
                    GroupBy(y => new { y.PurchaseOrder.SupplierId }).
                    Select(z => new { DeptID = z.Key.SupplierId, TotalAmt = z.Sum(a => (a.Quantity * a.UnitPrice)) });

                foreach (var i in gendeptRpt)
                {
                    grvm.deptDP.datapoint.Add(new StringDoubleDPViewModel(i.DeptID, (double)i.TotalAmt));
                }

                #endregion

                #region Purchases by Stationery Category

                var genstatRpt = context.TransactionDetail.
                    Where(x => x.PurchaseOrder.Status =="Completed" && x.TransactionDate >= fromDateTP && 
                    x.TransactionDate <= toDateTP &&
                    grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                            grvm.selectentcategory.Any(id => id == x.PurchaseOrder.SupplierId)).
                        GroupBy(y => new { y.Stationery.Category }).
                        Select(z => new { itemCat = z.Key.Category, totalAmt = z.Sum(a => (a.Quantity * a.UnitPrice)) });

                foreach (var i in genstatRpt)
                {
                    grvm.statDP.datapoint.Add(new StringDoubleDPViewModel(i.itemCat, (double)i.totalAmt));
                }
                
                #endregion

                #region Purchases over time

                var timeRpt = context.TransactionDetail.Where(x => x.PurchaseOrder.Status == "Completed" && 
                x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP &&
                    grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                            grvm.selectentcategory.Any(id => id == x.PurchaseOrder.SupplierId)).
                    OrderBy(x => x.TransactionDate).
                    GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).ToArray().
                    Select(y => new { dateval = string.Format("{0} {1}", Enum.Parse(typeof(EnumMonth), y.Key.Month.ToString()), y.Key.Year), totalAmt = y.Sum(z => (z.Quantity * z.UnitPrice)) });

                foreach (var i in timeRpt)
                {
                    grvm.stattimeDP.datapoint.Add(new StringDoubleDPViewModel(i.dateval, (double)i.totalAmt));
                }
                
                #endregion
            }

            if (module == "Retrieval")
            {
                #region Retrieval by Employee

                var gendeptRpt = context.TransactionDetail.
                    Where(x => x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP &&
                    grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                            grvm.selectentcategory.Any(id => id == x.StationeryRetrieval.AspNetUsers.EmployeeName)).
                    GroupBy(y => new { y.StationeryRetrieval.AspNetUsers.EmployeeName }).
                    Select(z => new { DeptID = z.Key.EmployeeName, TotalAmt = z.Sum(a => (a.Quantity * a.UnitPrice)) });

                foreach (var i in gendeptRpt)
                {
                    grvm.deptDP.datapoint.Add(new StringDoubleDPViewModel(i.DeptID, (double)i.TotalAmt));
                }

                #endregion

                #region Retrieval by Stationery Category

                var genstatRpt = context.TransactionDetail.
                    Where(x => x.StationeryRetrieval.RetrievalId != null && x.TransactionDate >= fromDateTP && 
                    x.TransactionDate <= toDateTP &&
                    grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                            grvm.selectentcategory.Any(id => id == x.StationeryRetrieval.AspNetUsers.EmployeeName)).
                        GroupBy(y => new { y.Stationery.Category }).
                        Select(z => new { itemCat = z.Key.Category, totalAmt = z.Sum(a => (a.Quantity * a.UnitPrice)) });

                foreach (var i in genstatRpt)
                {
                    grvm.statDP.datapoint.Add(new StringDoubleDPViewModel(i.itemCat, (double)i.totalAmt));
                }
                
                #endregion

                #region Retrieval over time

                var timeRpt = context.TransactionDetail.Where(x => x.StationeryRetrieval.RetrievalId != null && 
                x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP &&
                    grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                            grvm.selectentcategory.Any(id => id == x.StationeryRetrieval.AspNetUsers.EmployeeName)).
                    OrderBy(x => x.TransactionDate).
                    GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).ToArray().
                    Select(y => new { dateval = string.Format("{0} {1}", Enum.Parse(typeof(EnumMonth), y.Key.Month.ToString()), y.Key.Year), totalAmt = y.Sum(z => (z.Quantity * z.UnitPrice)) });

                foreach (var i in timeRpt)
                {
                    grvm.stattimeDP.datapoint.Add(new StringDoubleDPViewModel(i.dateval, (double)i.totalAmt));
                }

            }
                #endregion
                return View(grvm);
            
        }
    }

     

        #endregion
    }
