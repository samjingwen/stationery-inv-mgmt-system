using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;
using Team7ADProject.ViewModels.Api;

//Authors: Lynn Lynn Oo
namespace Team7ADProject.Controllers
{
    public class RaiseRequestController : Controller
    {
        #region Teh Li Heng
        private LogicDB _context;

        public RaiseRequestController()
        {
            _context=new LogicDB();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            List<RaiseRequestViewModel> viewModels= new List<RaiseRequestViewModel>();
            return View(viewModels);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Save(RaiseRequestViewModel viewModel)
        //{
        //    //Check if there's any existing stationery request

        //    //Adding transaction detail for each item
        //    int transactionId = _context.TransactionDetail.Count();
        //    string itemId = _context.Stationery.Single(m => m.Description == viewModel.Description).ItemId;
        //    TransactionDetail transactionDetailInDb = new TransactionDetail
        //    {
        //        TransactionId = transactionId+1,
        //        ItemId = itemId,
        //        Quantity = viewModel.Quantity,
        //        Remarks = string.Empty,
        //        TransactionRef = "",
        //        TransactionDate = DateTime.Now,
        //    };
            
            
        //    //viewModel.
        //    //throw new NotImplementedException();
        //}


        #endregion

        public ActionResult Add()
        {

            throw new NotImplementedException();
        }
    }
}