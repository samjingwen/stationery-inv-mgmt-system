using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    #region Sam Jing Wen

    //For SC to generate retrieval and make amendments
    public class ManageRetrievalController : Controller
    {
        // GET: ManageRetrieval
        public ActionResult Index()
        {
            CompiledRequestViewModel model = new CompiledRequestViewModel();
            return View(model.RetrievalList);
        }

        [HttpPost]
        public ActionResult SaveRetrieval(CompiledRequestViewModel model)
        {
            ViewBag.testing = model.RetrievalList[0].ItemId;
            ViewBag.testing2 = model.RetrievalList[0].RetrievedQty;
            ViewBag.testing3 = "adasda";
            return View();
        }

        


    }
    #endregion

}