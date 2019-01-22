using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    #region Author:Gao Jiaxue

    public class ApproveOrderController : Controller
    {   //get Data
        private LogicDB _context;

        public ApproveOrderController()
        {
            _context = new LogicDB();
        }
        //Clean garbage
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: ApproveOrder
        [Authorize(Roles = "Store Manager")]
        [Authorize(Roles = "Acting Department Head")]
        public ActionResult Index()
        {
            return View();
        }

        //Get data from DB

        //Retrieve All  PO
        [Authorize(Roles = "Store Manager")]
        [Authorize(Roles = "Acting Department Head")]
        public ActionResult ViewPORecord()
        {
           List<PurchaseOrder> poList = _context.PurchaseOrder.ToList();
            return View(poList);
        }
        //Get PoDetails
        [HttpGet]
        [Authorize(Roles = "Store Manager")]
        [Authorize(Roles = "Acting Department Head")]
        public ActionResult PODetails(string  poNo)
        {
           PurchaseOrder purchaseOrder = _context.PurchaseOrder.SingleOrDefault(c => c.PONo == poNo);
            List<TransactionDetail> transactionDetail = _context.TransactionDetail.Where(c => c.TransactionRef == poNo).ToList();
                if (purchaseOrder == null)
            { return HttpNotFound(); }
            var poDetailsViewModel = new PoDetailsViewModel
            { PurchaseOrder = purchaseOrder,PODetails=transactionDetail};
            return View(poDetailsViewModel);
        }

        //Approve Po
        [Authorize(Roles = "Store Manager")]
        [Authorize(Roles = "Acting Department Head")]
        public ActionResult Approve(string poNo)
        {
            string userId = User.Identity.GetUserId();
            var query = _context.AspNetUsers.FirstOrDefault(x => x.Id == userId);
            var thisPo = _context.PurchaseOrder.SingleOrDefault(c => c.PONo == poNo);
            if (query == null || thisPo == null)
            {
                HttpNotFound();
            }
            thisPo.Status = "Pending Delivery";
            thisPo.ApprovedBy = query.UserName;

            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }
            #region Send Email
            string recipientEmail, subject, content;
            recipientEmail = thisPo.AspNetUsers1.Email;
            subject = " PO approved!";
            content = "I am very happy to inform you, your PO has been approved ";
            Email.Send( recipientEmail,  subject,  content);
            return RedirectToAction("ViewPORecord", "ApproveOrder");
            #endregion
        }

        //Reject PO
        [Authorize(Roles = "Store Manager")]
        [Authorize(Roles = "Acting Department Head")]
        public ActionResult Reject(string poNo)
        {
            string userId = User.Identity.GetUserId();
            var query = _context.AspNetUsers.FirstOrDefault(x => x.Id == userId);
            var thisPo = _context.PurchaseOrder.SingleOrDefault(c => c.PONo == poNo);
            if (query == null || thisPo== null)
            {
                HttpNotFound();
            }
            thisPo.Status = "Rejected";
            thisPo.ApprovedBy = query.UserName;
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }
            #region SendEmail
            string recipientEmail, subject, content;
            //recipientEmail = thisPo.AspNetUsers1.Email;
            recipientEmail = "gaojiaxue@outlook.com";
            subject = " PO rejected!";
            content = "Unfortunately, your PO was rejected";
            Email.Send(recipientEmail, subject, content);
            return RedirectToAction("ViewPORecord", "ApproveOrder");
            #endregion
        }
        #endregion
    }
}