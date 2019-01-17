using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
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
            RequestByItemViewModel model = new RequestByItemViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult GenerateDisbursement(RetrievedListViewModel model)
        {
            //Create new Retrieval
            string rid;
            LogicDB context = new LogicDB();
            var ret = context.StationeryRetrieval.OrderByDescending(x => x.Date).FirstOrDefault();
            if (ret.Date.Year == DateTime.Now.Year)
            {
                rid = "R" + DateTime.Now.Year.ToString() + "-" + (Convert.ToInt32(ret.RetrievalId.Substring(6, 4)) + 1).ToString("0000");
            }
            else
            {
                rid = "R" + DateTime.Now.Year.ToString() + "-" + "0001";
            }

            List<StationeryRetrievalViewModel> srList = model.RetrievalList;
            StationeryRetrieval retrieval = new StationeryRetrieval();
            retrieval.RetrievalId = rid;
            retrieval.RetrievedBy = User.Identity.GetUserId();
            retrieval.Date = DateTime.Now;
            foreach (var sr in srList)
            {
                if (sr.RetrievedQty > 0)
                {
                    TransactionDetail detail = new TransactionDetail();
                    detail.ItemId = sr.ItemId;
                    detail.Quantity = sr.RetrievedQty;
                    detail.TransactionDate = DateTime.Now;
                    detail.Remarks = "Retrieved";
                    detail.TransactionRef = rid;
                    retrieval.TransactionDetail.Add(detail);
                }
            }
            context.StationeryRetrieval.Add(retrieval);
            context.SaveChanges();

            //Generate Disbursement
            StationeryDisbursementViewModel sdViewModel = new StationeryDisbursementViewModel(retrieval);
























            return View();
        }

        


    }
    #endregion

}