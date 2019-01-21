using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;
using Microsoft.AspNet.Identity;


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
        public ActionResult Save(AdjustmentViewModel[] requests,string remark)
        {

            string result = "You Haven't Add Any Adjustment Item!";


            string currentUserId = User.Identity.GetUserId();
            //AspNetUsers currentUser = context.AspNetUsers.First(m => m.Id == currentUserId);

            decimal? amount = 0;
            if (requests != null)
            {
                StockAdjustment adjustment = new StockAdjustment
                {
                    StockAdjId = newAdjustId(),
                    PreparedBy = currentUserId,
                    ApprovedBy = null,
                    Remarks = remark,
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
                    amount += item.Quantity * transactionDetail.UnitPrice;
                    adjustment.TransactionDetail.Add(transactionDetail);
                    context.TransactionDetail.Add(transactionDetail);
                    context.SaveChanges();
                }

                context.StockAdjustment.Add(adjustment);
                context.SaveChanges();
                result = "Success! Request is complete!";

                SendEmail(currentUserId, amount);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string newAdjustId()
        {
            string lastId = context.StockAdjustment.OrderByDescending(x=>x.StockAdjId).FirstOrDefault().StockAdjId;
            if (lastId == null)
            {
                lastId = "SAD-000000";
            }
            int id = int.Parse(lastId.Substring(4));
            return "SAD-" + (id + 1).ToString().PadLeft(6, '0');
        }

        public int GenerateTransactionDetailId()
        {
            TransactionDetail lastItem = context.TransactionDetail.OrderByDescending(x => x.TransactionId).FirstOrDefault();
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

        public void SendEmail(string userId, decimal? amount)
        {
            if(userId == null)
            {
                return;
            }
            string emailAddress;
            AspNetUsers user;
            if (amount <= -250)
            {
                emailAddress = context.AspNetUsers.Single(x => x.Id == userId).Department.AspNetUsers2.Email;
                user = context.AspNetUsers.Single(x => x.Id == userId).Department.AspNetUsers2;
            }
            else
            {
                emailAddress = context.AspNetUsers.Single(x => x.Id == userId).Department.AspNetUsers1.Email;
                user = context.AspNetUsers.Single(x => x.Id == userId).Department.AspNetUsers1;
            }
            string subject = "New Adjustment Raised";
            string content = " Dear " + user.EmployeeName + ": \n You got a new adjustment raised.\n Raised by " + context.AspNetUsers.Single(x => x.Id == userId).EmployeeName + ".";
            Email.Send("1015440098@qq.com", subject, content);
        }
    }
}