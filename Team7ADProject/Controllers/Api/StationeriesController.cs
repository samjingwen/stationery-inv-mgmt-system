using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels.Api;

namespace Team7ADProject.Controllers.Api
{
    public class StationeriesController : ApiController
    {
        //Author1: Teh Li Heng 16/1/2019  
        //Delete operation for stationeries, and loading of stationeries based on categories

        //Author 2: Zan Tun Khine 17/1/2019
        //Load top 3 suppliers and their respective prices based on the selected item
        #region Teh Li Heng
        private LogicDB _context;

        public StationeriesController()
        {
            _context = new LogicDB();
        }


        // DELETE: api/Stationeries/5
        [ResponseType(typeof(Stationery))]
        public IHttpActionResult DeleteStationery(string id)
        {
            Stationery stationery = _context.Stationery.Find(id);
            if (stationery == null)
            {
                return NotFound();
            }

            _context.Stationery.Remove(stationery);
            _context.SaveChanges();

            return Ok(stationery);
        }

        //For loading DropDownList
        [Route("~/api/stationeries/categories")]
        public IHttpActionResult GetCategory()
        {
            IEnumerable<String> categories = _context.Stationery.Select(m => m.Category).Distinct().ToList();

            if (categories == null)
            {
                return NotFound();
            }

            List<String> collections = new List<String>();
            foreach (string current in categories)
            {
                collections.Add(current);
            }

            return Ok(collections);
        }

        [Route("~/api/stationeries/categories/{category}")]
        public IHttpActionResult GetItemsFromCategory(string category)
        {
            List<Stationery> items = _context.Stationery.Where(m => m.Category == category).ToList();

            List<RaiseRequestDTO> viewModels = new List<RaiseRequestDTO>();
            for (int i = 0; i < items.Count; i++)
            {
                RaiseRequestDTO viewModel = new RaiseRequestDTO();
                viewModel.Id = items[i].ItemId;
                viewModel.ItemDescription = items[i].Description;
                viewModels.Add(viewModel);
            }
            return Ok(viewModels);
        }

        [Route("~/api/stationeries/item/{itemId}")]
        public IHttpActionResult GetUnitFromItem(string itemId)
        {
            Stationery stationery = _context.Stationery.Single(m => m.ItemId == itemId);
            string unit = stationery.UnitOfMeasure;

            return Ok(unit);
        }
        #endregion

        #region Zan Tun Khine

        // To get the top 3 suppliers based on selected item
        [Route("~/api/stationeries/supplier/{itemId}")]
        public IHttpActionResult GetSupplierFromItem(string itemId)
        {

            List<String> supplierlist = new List<String>();

            Stationery stationery = _context.Stationery.SingleOrDefault(x => x.ItemId == itemId);
            supplierlist.Add(stationery.Supplier.SupplierName);
            supplierlist.Add(stationery.Supplier1.SupplierName);
            supplierlist.Add(stationery.Supplier2.SupplierName);
            return Ok(supplierlist);
        }

        // To get the top 3 prices based on selected item
        [Route("~/api/stationeries/price/{itemId}")]
        public IHttpActionResult GetPriceFromItem(string itemId)
        {

            List<decimal> pricelist = new List<decimal>();

            Stationery stationery = _context.Stationery.SingleOrDefault(x => x.ItemId == itemId);
            pricelist.Add(stationery.FirstSuppPrice);
            pricelist.Add(stationery.SecondSuppPrice);
            pricelist.Add(stationery.ThirdSuppPrice);

            return Ok(pricelist);
        }

        // To get the top 3 suppliers and their respective prices based on selected item
        [Route("~/api/stationeries/supplierandprice/{itemId}")]
        public IHttpActionResult GetSupplierAndPriceFromItem(string itemId)
        {

            List<String> supplierlist = new List<String>();

            Stationery stationery = _context.Stationery.SingleOrDefault(x => x.ItemId == itemId);
            supplierlist.Add(stationery.Supplier.SupplierName + stationery.FirstSuppPrice);
            supplierlist.Add(stationery.Supplier1.SupplierName + stationery.SecondSuppPrice);
            supplierlist.Add(stationery.Supplier2.SupplierName + stationery.ThirdSuppPrice);

            return Ok(supplierlist);
        }
        #endregion

        #region Teh Li Heng

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }


        #endregion




    }
}