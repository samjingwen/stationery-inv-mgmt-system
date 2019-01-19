using System;
using System.Collections;
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
        public decimal UnitPrice
        {
            get
            {
                if (ItemId != null)
                {
                    LogicDB context = new LogicDB();
                    return context.Stationery.Where(x => x.ItemId == ItemId).FirstOrDefault().FirstSuppPrice;
                }
                else
                    return 0;
            }
        }
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

    public class RequestByIdViewModel
    {
        public string RequestId { get; set; }
        public List<BreakdownByItemViewModel> ItemList { get; set; }
    }

    public static class CreateDisbHelpers
    {
        public static String GetNewDisbId()
        {
            LogicDB context = new LogicDB();
            var disbursement = context.Disbursement.OrderByDescending(x => x.DisbursementId).First();
            string did = "DISB" + (Convert.ToInt32(disbursement.DisbursementId.Substring(4, 6)) + 1).ToString("000000");
            return did;
        }

        public static string GetNewDisbNo(string DepartmentId)
        {
            LogicDB context = new LogicDB();
            var disbursement = context.Disbursement.OrderByDescending(x => x.DisbursementId).First();
            string dno = "D" + DepartmentId + (Convert.ToInt32(disbursement.DisbursementNo.Substring(5, 5)) + 1).ToString("00000");
            return dno;
        }

        public static List<RequestByReqIdView> GetRequestQuery()
        {
            LogicDB context = new LogicDB();
            List<RequestByReqIdView> model = context.RequestByReqIdView.ToList();
            var query = context.DisbByDept.ToList();
            foreach(var i in query)
            {
                var reqt = model.Where(x => x.DepartmentId == i.DepartmentId && x.ItemId == i.ItemId).FirstOrDefault();
                if (reqt != null)
                {
                    if (reqt.Quantity > i.Quantity)
                    {
                        reqt.Quantity = reqt.Quantity - (int)i.Quantity;
                    }
                    else if (reqt.Quantity == i.Quantity)
                    {
                        model.Remove(reqt);
                    }
                    else
                    {
                        i.Quantity -= reqt.Quantity;
                        while (i.Quantity > 0)
                        {
                            model.Remove(reqt);
                            reqt = model.Where(x => x.DepartmentId == i.DepartmentId && x.ItemId == i.ItemId).FirstOrDefault();
                            if (reqt != null)
                            {
                                if (reqt.Quantity > i.Quantity)
                                {
                                    reqt.Quantity = reqt.Quantity - (int)i.Quantity;
                                }
                                else if (reqt.Quantity == i.Quantity)
                                {
                                    model.Remove(reqt);
                                }
                                else
                                {
                                    i.Quantity -= reqt.Quantity;
                                }
                            }
                        }
                    }
                }
            }
            return model;
        }

        //public static List<RequestByIdViewModel> GetRequestByIdQuery()
        //{
        //    var model = GetRequestQuery();
        //    var newList = new List<RequestByIdViewModel>();
        //    foreach (var i in model)
        //    {
        //        var ele = newList.Where(x => x.RequestId == i.RequestId).FirstOrDefault();
        //        if (ele != null)
        //        {
        //            var item = ele.ItemList.Where(x => x.ItemId == i.ItemId);

        //        }
        //    }



        //}

    }

    

}