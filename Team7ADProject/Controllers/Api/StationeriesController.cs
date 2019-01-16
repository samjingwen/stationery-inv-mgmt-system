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
        //Author: Teh Li Heng 16/1/2019
        //Delete operation for stationeries, and loading of stationeries based on categories
        #region Teh Li Heng
        private LogicDB _context;

        public StationeriesController()
        {
            _context=new LogicDB();
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

            return Ok(categories);
        }

        [Route("~/api/stationeries/categories/{category}")]
        public IHttpActionResult GetItemsFromCategory(string category)
        {
            List<Stationery> items = _context.Stationery.Where(m => m.Category == category).ToList();

            RaiseRequestDTO viewModelApi = new RaiseRequestDTO();
            for (int i = 0; i < items.Count; i++)
            {
                viewModelApi.UnitOfMeasure.Add(items[i].UnitOfMeasure);
                viewModelApi.ItemDescription.Add(items[i].Description);
            }
            return Ok(viewModelApi);
        }

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