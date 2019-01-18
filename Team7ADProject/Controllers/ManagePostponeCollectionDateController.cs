using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;

namespace Team7ADProject.Controllers
{
    public class ManagePostponeCollectionDateController : Controller
    {
        #region Author:Lynn Lynn Oo

        LogicDB _context;
        public ManagePostponeCollectionDateController()
        {
            _context = new LogicDB();
        }
        // GET: ManagePostponeCollectionDate
        public ActionResult Index()
        {
            List<StationeryRequest> id = _context.StationeryRequest.ToList();
            return View(id);
        }
    }
    #endregion
}