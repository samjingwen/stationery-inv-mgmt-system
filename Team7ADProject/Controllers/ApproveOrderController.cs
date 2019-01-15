using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
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
        public ActionResult Index()
        {
            return View();
        }
        #region Gao Jiaxue
        //Get data from DB
   
        //Retrieve All  PO
        public ActionResult ViewPORecord()
        {
           List<PurchaseOrder> poList = _context.PurchaseOrder.ToList();
            return View(poList);
        }
        //Approve or Reject PO
        [HttpPost]
        public ActionResult PODetails(string  poNo)
        {
           PurchaseOrder purchaseOrder = _context.PurchaseOrder.SingleOrDefault(c => c.PONo == poNo);
            List<TransactionDetail> transactionDetail = _context.TransactionDetail.Where(c => c.TransactionRef == poNo).ToList();
                if (poNo == null)
            { return HttpNotFound(); }
            var poDetailsViewModel = new PoDetailsViewModel
            { PurchaseOrder = purchaseOrder,PODetails=transactionDetail};
            return View(poDetailsViewModel);
        }
        #endregion
    }
}