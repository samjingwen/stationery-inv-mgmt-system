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
            return View();
        }
    }
    #endregion

}