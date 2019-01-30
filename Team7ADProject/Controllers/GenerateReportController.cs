using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels.GenerateReport;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;

namespace Team7ADProject.Controllers
{
    //For SS to generate reports
    //Author: Elaine Chan
    [RoleAuthorize(Roles = "Store Supervisor,Department Head")]
    public class GenerateReportController : Controller
    {
        #region Author: Elaine Chan
        // GET: GenerateReport
        public ActionResult GenerateDashboard()
        {
            LogicDB context = new LogicDB();
            String userId = User.Identity.GetUserId();

            String DID = context.AspNetUsers.Where(x => x.Id == userId).First().DepartmentId;

            #region Build VM
            var grvm = GenerateReportViewModel.InitGRVM(DID);

            #endregion

            #region Disbursement by DeptID
            
            if(DID == "STAT") { 
            var gendeptRpt = context.TransactionDetail.Where(x=> x.Disbursement.AcknowledgedBy != null).
                GroupBy(x => new { x.Disbursement.DepartmentId }).
                Select(y => new { DeptID = y.Key.DepartmentId, TotalAmt = y.Sum(z => (z.Quantity * z.UnitPrice)) });

                foreach (var i in gendeptRpt)
                {
                    grvm.deptDP.datapoint.Add(new StringDoubleDPViewModel(i.DeptID, (double)i.TotalAmt));
                }
            }
            else
            {
                var gendeptRpt = context.TransactionDetail.Where(x => x.Disbursement.AcknowledgedBy != null 
                && x.Disbursement.DepartmentId == DID).
                GroupBy(x => new { x.Disbursement.DepartmentId }).
                Select(y => new { DeptID = y.Key.DepartmentId, TotalAmt = y.Sum(z => (z.Quantity * z.UnitPrice)) });

                foreach (var i in gendeptRpt)
                {
                    grvm.deptDP.datapoint.Add(new StringDoubleDPViewModel(i.DeptID, (double)i.TotalAmt));
                }
            }

            
            #endregion

            #region Disbursement by Stationery Category
            
            if (DID == "STAT") { 
            var genstatRpt = context.TransactionDetail.Where(x => x.Disbursement.AcknowledgedBy != null).
                    GroupBy(y => new { y.Stationery.Category }).
                    Select(z => new { itemCat = z.Key.Category, totalAmt = z.Sum(a => (a.Quantity * a.UnitPrice)) });

            foreach (var i in genstatRpt)
            {
                grvm.statDP.datapoint.Add(new StringDoubleDPViewModel(i.itemCat, (double)i.totalAmt));
            }
            }

            else
            {
                var genstatRpt = context.TransactionDetail.Where(x => x.Disbursement.AcknowledgedBy != null 
                && x.Disbursement.DepartmentId== DID).
        GroupBy(y => new { y.Stationery.Category }).
        Select(z => new { itemCat = z.Key.Category, totalAmt = z.Sum(a => (a.Quantity * a.UnitPrice)) });

                foreach (var i in genstatRpt)
                {
                    grvm.statDP.datapoint.Add(new StringDoubleDPViewModel(i.itemCat, (double)i.totalAmt));
                }
            }
            #endregion

            #region Disbursements over time
            int r = 0;
            foreach (var i in grvm.selectstatcategory)
            {
                if (DID == "STAT")
                {
                    var timeRpt = context.TransactionDetail.Where(x => x.Disbursement.AcknowledgedBy != null && x.Stationery.Category == i).
                        OrderBy(x => x.TransactionDate).
                        GroupBy(x => new { x.Disbursement.DepartmentId, x.TransactionDate.Year, x.TransactionDate.Month }).ToArray().
                        Select(y => new
                        {
                            deptID = y.Key.DepartmentId,
                            dateval = string.Format("{0} {1}", Enum.Parse(typeof(EnumMonth), y.Key.Month.ToString()), y.Key.Year),
                            totalAmt = y.Sum(z => (z.Quantity * z.UnitPrice))
                        });
                    grvm.data.Add(new ChartViewModel(i, i, new List<StringDoubleDPViewModel>()));

                    foreach (var q in timeRpt)
                    {
                        grvm.data[r].datapoint.Add(new StringDoubleDPViewModel(q.dateval, (double)q.totalAmt));

                    }
                    r++;
                }

                else
                {
                    var timeRpt = context.TransactionDetail.Where(x => x.Disbursement.AcknowledgedBy != null && x.Stationery.Category == i
                    && x.Disbursement.DepartmentId == DID).
                        OrderBy(x => x.TransactionDate).
                        GroupBy(x => new { x.Disbursement.DepartmentId, x.TransactionDate.Year, x.TransactionDate.Month }).ToArray().
                        Select(y => new
                        {
                            deptID = y.Key.DepartmentId,
                            dateval = string.Format("{0} {1}", Enum.Parse(typeof(EnumMonth), y.Key.Month.ToString()), y.Key.Year),
                            totalAmt = y.Sum(z => (z.Quantity * z.UnitPrice))
                        });
                    grvm.data.Add(new ChartViewModel(i, i, new List<StringDoubleDPViewModel>()));

                    foreach (var q in timeRpt)
                    {
                        grvm.data[r].datapoint.Add(new StringDoubleDPViewModel(q.dateval, (double)q.totalAmt));

                    }
                    r++;
                }
            }
            
            #endregion

            return View(grvm);
        }

        [HttpPost]
        public ActionResult GenerateDashboard(DateTime? fromDateTP, DateTime? toDateTP, string module, List<string> selstatcat, List<string> seldeptcat, List<string> seleecat, List<string> selsscat)
        {
          
            LogicDB context = new LogicDB();
            String userId = User.Identity.GetUserId();

            String DID = context.AspNetUsers.Where(x => x.Id == userId).First().DepartmentId;
            int r = 0;

            #region Build VM

            var grvm = GenerateReportViewModel.InitGRVM(DID, fromDateTP,toDateTP, module, selstatcat, seldeptcat, seleecat, selsscat);
            
            
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
                foreach (var i in grvm.selectstatcategory)
                {
                    var timeRpt = context.TransactionDetail.Where(x => x.Disbursement.AcknowledgedBy != null && x.Stationery.Category == i &&
                            grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                            grvm.selectentcategory.Any(id => id == x.Disbursement.DepartmentId) &&
                            x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP).
                                            OrderBy(x => x.TransactionDate).
                                            GroupBy(x => new { x.Disbursement.DepartmentId, x.TransactionDate.Year, x.TransactionDate.Month }).ToArray().
                                            Select(y => new { deptID = y.Key.DepartmentId, dateval = string.Format("{0} {1}", Enum.Parse(typeof(EnumMonth), y.Key.Month.ToString()), y.Key.Year), totalAmt = y.Sum(z => (z.Quantity * z.UnitPrice)) });

                    grvm.data.Add(new ChartViewModel(i, i, new List<StringDoubleDPViewModel>()));
                    
                        foreach (var j in timeRpt)
                    {
                        grvm.data[r].datapoint.Add(new StringDoubleDPViewModel(j.dateval, (double)j.totalAmt));
                    }
                    r++;
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
                
                foreach(var j in grvm.selectstatcategory) { 

                var timeRpt = context.TransactionDetail.Where(x => x.StationeryRequest.Status == "Completed" && x.Stationery.Category == j
                && x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP && 
                            grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                            grvm.selectentcategory.Any(id => id == x.StationeryRequest.DepartmentId)).
                    OrderBy(x => x.TransactionDate).
                    GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).ToArray().
                    Select(y => new { dateval = string.Format("{0} {1}", Enum.Parse(typeof(EnumMonth), y.Key.Month.ToString()), y.Key.Year), totalAmt = y.Sum(z => (z.Quantity * z.UnitPrice)) });

                    grvm.data.Add(new ChartViewModel(j, j, new List<StringDoubleDPViewModel>()));

                    foreach (var i in timeRpt)
                {
                    grvm.data[r].datapoint.Add(new StringDoubleDPViewModel(i.dateval, (double)i.totalAmt));
                }
                    r++;
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
                foreach (var j in grvm.selectstatcategory)
                {
                    var timeRpt = context.TransactionDetail.Where(x => x.Disbursement.DepartmentId != null && x.Stationery.Category == j && 
                x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP &&
                            grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                            grvm.selectentcategory.Any(id => id == x.Disbursement.DepartmentId)).
                    OrderBy(x => x.TransactionDate).
                    GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).ToArray().
                    Select(y => new { dateval = string.Format("{0} {1}", Enum.Parse(typeof(EnumMonth), y.Key.Month.ToString()), y.Key.Year), totalAmt = y.Sum(z => (z.Quantity * z.UnitPrice)) });

                    grvm.data.Add(new ChartViewModel(j, j, new List<StringDoubleDPViewModel>()));

                    foreach (var i in timeRpt)
                    {
                        grvm.data[r].datapoint.Add(new StringDoubleDPViewModel(i.dateval, (double)i.totalAmt));
                    }
                    r++;
                }
                #endregion
            }

            if (DID == "STAT")
            {
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
                        Where(x => x.PurchaseOrder.Status == "Completed" && x.TransactionDate >= fromDateTP &&
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
                    foreach (var j in grvm.selectstatcategory)
                    {
                        var timeRpt = context.TransactionDetail.Where(x => x.PurchaseOrder.Status == "Completed" && x.Stationery.Category == j &&
                    x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP &&
                        grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                                grvm.selectentcategory.Any(id => id == x.PurchaseOrder.SupplierId)).
                        OrderBy(x => x.TransactionDate).
                        GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).ToArray().
                        Select(y => new { dateval = string.Format("{0} {1}", Enum.Parse(typeof(EnumMonth), y.Key.Month.ToString()), y.Key.Year), totalAmt = y.Sum(z => (z.Quantity * z.UnitPrice)) });

                        grvm.data.Add(new ChartViewModel(j, j, new List<StringDoubleDPViewModel>()));

                        foreach (var i in timeRpt)
                        {
                            grvm.data[r].datapoint.Add(new StringDoubleDPViewModel(i.dateval, (double)i.totalAmt));
                        }
                        r++;
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
                    foreach (var j in grvm.selectstatcategory)
                    {
                        var timeRpt = context.TransactionDetail.Where(x => x.StationeryRetrieval.RetrievalId != null && x.Stationery.Category == j &&
                    x.TransactionDate >= fromDateTP && x.TransactionDate <= toDateTP &&
                        grvm.selectstatcategory.Any(id => id == x.Stationery.Category) &&
                                grvm.selectentcategory.Any(id => id == x.StationeryRetrieval.AspNetUsers.EmployeeName)).
                        OrderBy(x => x.TransactionDate).
                        GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).ToArray().
                        Select(y => new { dateval = string.Format("{0} {1}", Enum.Parse(typeof(EnumMonth), y.Key.Month.ToString()), y.Key.Year), totalAmt = y.Sum(z => (z.Quantity * z.UnitPrice)) });

                        grvm.data.Add(new ChartViewModel(j, j, new List<StringDoubleDPViewModel>()));

                        foreach (var i in timeRpt)
                        {
                            grvm.data[r].datapoint.Add(new StringDoubleDPViewModel(i.dateval, (double)i.totalAmt));
                        }
                        r++;
                    }
                }
                #endregion

            }
                return View(grvm);
            
        }
    }

     

        #endregion
    }
