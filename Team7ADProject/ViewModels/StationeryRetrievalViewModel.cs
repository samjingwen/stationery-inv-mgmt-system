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
    public class DeptRequestViewModel
    {
        public string ItemId { get; set; }
        public List<DeptItemDetailViewModel> DeptList { get; set; }

        public DeptRequestViewModel(string ItemId)
        {
            this.ItemId = ItemId;
            LogicDB context = new LogicDB();
            var queryPendDisb = context.StationeryRequest.Where(x => x.Status == "Pending Disbursement" || x.Status == "Partially Fulfilled");
            var query = from x in queryPendDisb
                        join y in context.TransactionDetail
                        on x.RequestId equals y.TransactionRef
                        group y by x.DepartmentId into g
                        select new
                        {
                            DepartmentId = g.Key,
                            Quantity = g.Where(x => x.ItemId == ItemId).Sum(x => x.Quantity)
                        };
        }
    }

    public class DeptItemDetailViewModel
    {
        public string DepartmentId { get; set; }
        public string Quantity { get; set; }
    }

    public class StationeryRetrievalViewModel
    {
        [StringLength(4)]
        public string ItemId { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [StringLength(25)]
        public string UnitOfMeasure { get; set; }

        public int Quantity { get; set; }

        public int RetrievedQty { get; set; }

        [Required]
        [StringLength(50)]
        public string Location { get; set; }

        public void Refresh()
        {
            LogicDB context = new LogicDB();
            Stationery item = context.Stationery.Find(ItemId);
            if (item != null)
            {
                Category = item.Category;
                Description = item.Description;
                UnitOfMeasure = item.UnitOfMeasure;
                Location = item.Location;
                if (item.QuantityWarehouse < Quantity)
                {
                    Quantity = item.QuantityWarehouse;
                }
            }
        }

    }

    public class CompiledRequestViewModel
    {
        public List<StationeryRetrievalViewModel> RetrievalList
        {
            get
            {
                //Get Pending disbursement and Partially fufilled 
                LogicDB context = new LogicDB();
                List<StationeryRetrievalViewModel> newList = new List<StationeryRetrievalViewModel>();
                var queryPendDisb = context.StationeryRequest.Where(x => x.Status == "Pending Disbursement" || x.Status == "Partially Fulfilled");
                var sumItemDisbursement = (from x in queryPendDisb
                                          join y in context.TransactionDetail
                                          on x.RequestId equals y.TransactionRef
                                          group y by y.ItemId into g
                                          select new StationeryRetrievalViewModel
                                          {
                                              ItemId = g.Key,
                                              Quantity = g.Sum(y => y.Quantity)
                                          }).ToList();

                //Get partially fufilled disbursed quantity
                var queryPartialDisb = from x in context.StationeryRequest
                                       where x.Status.Equals("Partially Fulfilled")
                                       select new { x.RequestId };

                var queryDisb = from x in queryPartialDisb
                                join y in context.Disbursement
                                on x.RequestId equals y.RequestId
                                join z in context.TransactionDetail
                                on y.DisbursementId equals z.TransactionRef
                                group z by z.ItemId into g
                                select new
                                {
                                    ItemId = g.Key,
                                    Quantity = g.Sum(m => m.Quantity)
                                };

                //Less off from partially fufilled and pending disbursement
                foreach (var item in sumItemDisbursement)
                {
                    var itemDisb = queryDisb.FirstOrDefault(x => x.ItemId == item.ItemId);
                    if (itemDisb != null)
                    {
                        item.Quantity -= itemDisb.Quantity;
                    }
                    item.Refresh();
                }

                return sumItemDisbursement.ToList();
            }

        }

    }

    public class RetrievedListViewModel
    {
        public List<StationeryRetrievalViewModel> RetrievalList { get; set; }
    }



}