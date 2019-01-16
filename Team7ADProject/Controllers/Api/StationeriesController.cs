using System;
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

namespace Team7ADProject.Controllers.Api
{
    public class StationeriesController : ApiController
    {
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}