using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;
using Team7ADProject.Models;

//Author: Sam Jing Wen
namespace Team7ADProject.ViewModels
{
    public class RequestByDeptViewModel
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public List<BreakdownByItemViewModel> requestList { get; set; }
    }

    public class BreakdownByItemViewModel
    {
        public string ItemId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int RetrievedQty { get; set; }
    }

    public class RequestByItemViewModel
    {
        public string ItemId { get; set; }
        public string Description { get; set; }
        public int TotalQty
        {
            get
            {
                if (ItemId != null)
                    return requestList.Sum(x => x.Quantity);
                else
                    return 0;
            }
        }
        public List<BreakdownByDeptViewModel> requestList { get; set; }
    }

    public class BreakdownByDeptViewModel
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Quantity { get; set; }
        public int RetrievedQty { get; set; }
    }

    public class DisbursementByDeptViewModel
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int CollectionPointId
        {
            get
            {
                if(DepartmentId != null)
                {
                    LogicDB context = new LogicDB();
                    return context.Department.Where(x => x.DepartmentId == DepartmentId).FirstOrDefault().CollectionPointId;
                }
                else
                {
                    return 0;
                }
            }
        }
        public string CollectionDescription
        {
            get
            {
                if (DepartmentId != null)
                {
                    LogicDB context = new LogicDB();
                    return context.Department.Where(x => x.DepartmentId == DepartmentId).FirstOrDefault().CollectionPoint.CollectionDescription;
                }
                else
                {
                    return null;
                }
            }
        }
        public List<BreakdownByItemViewModel> requestList { get; set; }

    }

}