using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;

namespace Team7ADProject.Controllers
{
    //Author: Elaine Chan
    //For generating charge back and viewing charge back
    public class ChargeBackController : Controller
    {
        #region Author:Elaine
        LogicDB context = new LogicDB();

        // GET: ChargeBack
        public ActionResult ChargeBack()
        {
            
            var chargeback = new List<Disbursement>();
            chargeback = context.Disbursement.ToList();
            return View(chargeback);
        }
        #endregion

    }
}