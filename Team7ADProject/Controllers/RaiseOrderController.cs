using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Text;

namespace Team7ADProject.Controllers
{
    //For SC to raise PO
    public class RaiseOrderController : Controller
    {

        private LogicDB _context = new LogicDB();


        [Authorize(Roles = "Store Clerk")]
        #region // For the Store Staff To View His/Her Own POs
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var query = _context.AspNetUsers.FirstOrDefault(x => x.Id == userId);
            ViewBag.CurrentUser = query.EmployeeName;
            List<PurchaseOrder> purchaseOrder = _context.PurchaseOrder.Where(x => x.OrderedBy == userId).ToList();
            if (purchaseOrder.Count() == 0)
            {
                ViewBag.error = "No Records Found!";

            }
            return View(purchaseOrder);

        }
        #endregion

        #region For the Store Staff To Raise PO
        public ActionResult RaisePo()
        {

            //load supplier
            var suppliers = _context.Supplier.ToList();
            var stationeries = _context.Stationery.ToList();
            RaisePOViewModel viewModel = new RaisePOViewModel();
            viewModel.Suppliers = suppliers;
            viewModel.Stationeries = stationeries;
            viewModel.PONo = "PO" + DateTime.Now.Date.ToString("yy") + "/" + DateTime.Now.Date.ToString("MM") + "/" + GetSerialNumber();
            ViewBag.PoNo = viewModel.PONo;
            viewModel.Categories = _context.Stationery.Select(m => m.Category).Distinct().ToList();
            return View(viewModel);
        }

        #region Save POs
        //add transaction details input by clerk

        [HttpPost]
        public ActionResult Save(RaisePOViewModel[] poModel)
        {
            string result = "Error! Request Failed!";

            // to get the user ID of the current user
            string userId = User.Identity.GetUserId();
            var query = _context.AspNetUsers.FirstOrDefault(x => x.Id == userId);

            string newPOnum = "PO" + DateTime.Now.Date.ToString("yy") + "/" + DateTime.Now.Date.ToString("MM") + "/" + GetSerialNumber();

            int count = poModel.Length;

            PurchaseOrder newPO = new PurchaseOrder()
            {
                PONo = newPOnum,
                SupplierId = "",
                OrderedBy = query.Id,
                ApprovedBy = null,
                Amount = 900,
                Date = DateTime.Today,
                Status = "Pending Approval"
            };

            _context.PurchaseOrder.Add(newPO);
            _context.SaveChanges();
            foreach (var item in poModel)

            {
                TransactionDetail newTD = new TransactionDetail
                {
                    ItemId = "C001",//item.ItemId,
                    Quantity = 12, //item.Quantity,
                    Remarks = "Pending Approval",
                    TransactionRef = "PO19/01/15",//newPOnum,
                    TransactionDate = DateTime.Today,
                    UnitPrice = 1//item.UnitPrice
                };
                _context.TransactionDetail.Add(newTD);

                //newPO.TransactionDetail.Add(newTD);
                //_context.PurchaseOrder.Add(newPO);
            }

            _context.SaveChanges();
            result = "success";
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        ////[HttpPost]
        ////public string Save(RaisePOCustomViewModel poModel)
        ////{
        ////    // to get the user ID of the current user
        ////    string userId = User.Identity.GetUserId();
        ////    var query = _context.AspNetUsers.FirstOrDefault(x => x.Id == userId);

        ////    poModel.RaisePOVM.OrderedBy = query.Id; //logic to get from login user
        ////    poModel.RaisePOVM.ApprovedBy = null;

        ////    //add transaction details input by clerk

        ////    PurchaseOrder newPO = new PurchaseOrder();
        ////    TransactionDetail newTD = new TransactionDetail();

        ////    newPO.PONo = "PO" + DateTime.Now.Date.ToString("yy") + "/" + DateTime.Now.Date.ToString("MM") + "/" + GetSerialNumber();
        ////    newPO.OrderedBy = poModel.RaisePOVM.OrderedBy;
        ////    newPO.ApprovedBy = poModel.RaisePOVM.ApprovedBy;
        ////    newPO.SupplierId = poModel.RaisePOVM.SupplierId;      // from view
        ////    newPO.Amount = (decimal)(poModel.RaisePOVM.Quantity * poModel.RaisePOVM.UnitPrice);
        ////    newPO.Date = DateTime.Now;
        ////    newPO.Status = "Pending Approval";

        ////    newTD.TransactionDate = newPO.Date;
        ////    newTD.Quantity = poModel.RaisePOVM.Quantity;          // from view
        ////    newTD.Remarks = poModel.RaisePOVM.Remarks;            // from view
        ////    newTD.TransactionRef = newPO.PONo;
        ////    newTD.TransactionDate = newPO.Date;
        ////    newTD.UnitPrice = poModel.RaisePOVM.UnitPrice;        // from view
        ////    newTD.ItemId = "C001";// poModel.ItemId;    // from view

        ////    newPO.TransactionDetail.Add(newTD);
        ////    _context.PurchaseOrder.Add(newPO);

        ////    _context.SaveChanges();
        ////    return "success";

        ////}
        #endregion
        #endregion


        #region Generate Running Number

        // Format : PO18/02/##
        private static string GetSerialNumber()
        {
            LogicDB context1 = new LogicDB();
            var lastPoNo = context1.PurchaseOrder.OrderByDescending(x => x.PONo).FirstOrDefault();
            string current = lastPoNo.PONo.Substring(8);
            int newSerial = Int32.Parse(current);
            return ((newSerial + 1).ToString("00"));
        }

        #endregion

        #region View Details of PO
        // GET: RaiseOrder/Details/id
        [HttpGet]
        public ActionResult Details(string poNo)
        {
            PurchaseOrder purchaseOrder = _context.PurchaseOrder.SingleOrDefault(x => x.PONo == poNo);
            List<TransactionDetail> transactionDetail = _context.TransactionDetail.Where(c => c.TransactionRef == poNo).ToList();

            if (poNo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (purchaseOrder == null)

            {
                return HttpNotFound();
            }

            var poDetailsViewModel = new RaisePOViewModel
            {
                PurchaseOrder = purchaseOrder,
                PODetails = transactionDetail
            };
            return View(poDetailsViewModel);
        }
        #endregion

    }
}