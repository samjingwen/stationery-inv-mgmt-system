using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

//Authors: Cheng Zongpei
namespace Team7ADProject.Controllers
{
    //For SC to raise adjustment
    public class RaiseAdjustmentController : Controller
    {
        LogicDB context = new LogicDB();
        // GET: RaiseAdjustment
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(AdjustmentViewModel[] requests)
        {

            string result = "Error! Request is incomplete!";


            //string currentUserId = User.Identity.GetUserId();
            //AspNetUsers currentUser = _context.AspNetUsers.First(m => m.Id == currentUserId);


            //string newStationeryRequestId = GenerateRequestId();
            if (requests != null)
            {
                StockAdjustment adjustment = new StockAdjustment
                {
                    StockAdjId = newAdjustId(),
                    PreparedBy = null,
                    ApprovedBy = null,
                    Remarks = null,
                    Date = DateTime.Today
                };

                foreach (var item in requests)
                {
                    TransactionDetail transactionDetail = new TransactionDetail
                    {
                        TransactionId = GenerateTransactionDetailId(),
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        Remarks = "Pending Approval",
                        TransactionRef = adjustment.StockAdjId,
                        TransactionDate = DateTime.Today,
                        UnitPrice = context.Stationery.Single(x => x.ItemId == item.ItemId).FirstSuppPrice
                    };
                    adjustment.TransactionDetail.Add(transactionDetail);
                    context.TransactionDetail.Add(transactionDetail);
                }

                context.StockAdjustment.Add(adjustment);
                context.SaveChanges();
                result = "Success! Request is complete!";

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string newAdjustId()
        {
            string lastId = context.StockAdjustment.LastOrDefault().StockAdjId;
            if (lastId == null)
            {
                lastId = "SAD-000000";
            }
            int id = int.Parse(lastId.Substring(4));
            return "SAD-" + (id + 1).ToString().PadLeft(6, '0');
        }

        public int GenerateTransactionDetailId()
        {
            TransactionDetail lastItem = context.TransactionDetail.LastOrDefault();
            int lastRequestId;
            if (lastItem == null)
            {
                lastRequestId = 1000;
            }
            else
            {
                lastRequestId = lastItem.TransactionId;
            }
            int newRequestId = lastRequestId + 1;
            return newRequestId;
        }
    }
}