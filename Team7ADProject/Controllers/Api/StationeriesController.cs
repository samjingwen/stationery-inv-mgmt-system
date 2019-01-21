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


        // PUT: api/Stationeries/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStationery(string id)
        {
            Stationery stationery = _context.Stationery.Find(id);
            if (stationery == null)
            {
                return NotFound();
            }

            stationery.ActiveState = false;
            _context.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
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

            //List<String> collections = new List<String>();
            //foreach (string current in categories)
            //{
            //    collections.Add(current);
            //}

            return Ok(categories);
            //List<String> collections = new List<String>();
            //foreach (string current in categories)
            //{
            //    collections.Add(current);
            //}
            //return Ok(categories);
        }

        [Route("~/api/stationeries/categories/{category}")]
        public IHttpActionResult GetItemsFromCategory(string category)
        {
            List<Stationery> items = _context.Stationery.Where(m => m.Category == category && m.ActiveState==true).ToList();

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
        [Route("~/api/stationeries/categories/items/{itemId}")]
        public IHttpActionResult GetSupplierAndPriceFromItem(string itemId)
        {

            List<TopSupplierAndPriceDTO> spObj = new List<TopSupplierAndPriceDTO>();

            Stationery stationery = _context.Stationery.FirstOrDefault(x => x.ItemId == itemId);

            TopSupplierAndPriceDTO firstSup = new TopSupplierAndPriceDTO();
            firstSup.Id = stationery.FirstSupplierId;
            firstSup.Price = stationery.FirstSuppPrice;
            firstSup.Supplier = stationery.Supplier.SupplierName;
            spObj.Add(firstSup);

            TopSupplierAndPriceDTO secondSup = new TopSupplierAndPriceDTO();
            secondSup.Id = stationery.SecondSupplierId;
            secondSup.Price = stationery.SecondSuppPrice;
            secondSup.Supplier = stationery.Supplier1.SupplierName;
            spObj.Add(secondSup);

            TopSupplierAndPriceDTO thirdSup = new TopSupplierAndPriceDTO();
            thirdSup.Id = stationery.ThirdSupplierId;
            thirdSup.Price = stationery.ThirdSuppPrice;
            thirdSup.Supplier = stationery.Supplier2.SupplierName;
            spObj.Add(thirdSup);


            return Ok(spObj);
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

        #region Gao Jiaxue
        //For loading suppliers
        [Route("~/api/stationeries/suppliers")]
        public IHttpActionResult GetSuppliers()
        {
            IEnumerable<String> suppliers = _context.Supplier.Select(m => m.SupplierId).Distinct().ToList();

            if (suppliers == null)
            {
                return NotFound();
            }


            return Ok(suppliers);
        }
        #endregion

        #region Cheng Zong Pei
        [Route("~/api/stationeries/adjustment/{itemId}")]
        public IHttpActionResult GetAdjustmentInfo(string itemId)
        {
            Stationery stationery = _context.Stationery.Single(x => x.ItemId == itemId);
            AdjustmentApiViewModel adjustment = new AdjustmentApiViewModel();
            adjustment.UnitOfMeasure = stationery.UnitOfMeasure;
            adjustment.QuantityWareHouse = stationery.QuantityWarehouse;
            adjustment.Price = stationery.FirstSuppPrice;
            return Ok(adjustment);
        }
        #endregion

    }
}