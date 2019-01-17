using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    
    public class RaiseRequestController : Controller
    {
        #region Teh Li Heng
        static private LogicDB _context;
        static List<RaiseRequestViewModel> test;

        public RaiseRequestController()
        {
            _context=new LogicDB();
            test=new List<RaiseRequestViewModel>();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(RaiseRequestViewModel viewModel)
        {
            //Check if there's any existing stationery request

            //Adding transaction detail for each item
            int transactionId = _context.TransactionDetail.Count();
            string itemId = _context.Stationery.Single(m => m.Description == viewModel.Description).ItemId;
            TransactionDetail transactionDetailInDb = new TransactionDetail
            {
                TransactionId = transactionId+1,
                ItemId = itemId,
                Quantity = viewModel.Quantity,
                Remarks = string.Empty,
                TransactionRef = "",
                TransactionDate = DateTime.Now,
            };
            throw new NotImplementedException();
        }

        public ActionResult Add(RaiseRequestViewModel viewModel)
        {
            string userid = User.Identity.GetUserId();
            TransactionDetail newTransactionDetail = new TransactionDetail
            {

            };
            //test.Add(selection);
            viewModel.Models = test;
            return View("Index",viewModel);
        }
        #endregion
    }
}