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

        private LogicDB _context;

        public RaiseOrderController()
        {
            _context = new LogicDB();
        }

        //[Authorize(Roles = "Store Supervisor")]
        // GET: RaiseOrder
        public ActionResult Index()
        {
            //load supplier
            var suppliers = _context.Supplier.ToList();
            var stationeries = _context.Stationery.ToList();

            RaisePOViewModel viewModel = new RaisePOViewModel();
            viewModel.Suppliers = suppliers;
            viewModel.Stationeries = stationeries;

            viewModel.PONo = "PO" + DateTime.Now.Date.ToString("yy") + "/" + DateTime.Now.Date.ToString("MM") + "/" + GetSerialNumber();
            //viewModel.PONo = GetSerialNumber().ToString();
            ViewBag.PoNo = viewModel.PONo;
            viewModel.Categories = _context.Stationery.Select(m => m.Category).Distinct().ToList();

            //ViewBag.Cate = _context.Stationery.Select(m => m.Category).Distinct().ToList();


            return View(viewModel);
        }

        #region // For the Store Staff To View His/Her Own POs
        public ActionResult ViewPo()
        {
            // to get the user ID of the current user
            string userId = "5042e8ca-ef83-4458-a78c-07e4f6ba0d1d";// User.Identity.GetUserId();
            // var query = _context.AspNetUsers.FirstOrDefault(x => x.Id == userId);
            //ViewBag.UserName = _context.AspNetUsers.FirstOrDefault(x => x.Id == userId).UserName;
            var po = _context.PurchaseOrder.Where(x => x.OrderedBy == userId).ToList().
                Select(x => new RaisePOViewModel()
                {
                    PONo = x.PONo,
                    SupplierId = x.SupplierId,
                    ApprovedBy = x.ApprovedBy,
                    Amount = x.Amount,
                    Date = x.Date,
                    Status = x.Status
                }
                );

            return View(po);
        }

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


        //[HttpGet]
        //public JsonResult GetStatList(string category)
        //{
        //    _context.Configuration.ProxyCreationEnabled = false;
        //    var stationeries = _context.Stationery.Where(x => x.Category == category).ToList();

        //    List<String> itemlist = new List<string>();
        //    foreach (Stationery stat in stationeries)
        //    {
        //        itemlist.Add(stat.Description);
        //    }

        //    return Json(itemlist, JsonRequestBehavior.AllowGet);

        //}

        [HttpPost]
        public string Save(RaisePOViewModel poModel)
        {
            // to get the user ID of the current user
            string userId = User.Identity.GetUserId();
            var query = _context.AspNetUsers.FirstOrDefault(x => x.Id == userId);

            poModel.OrderedBy = query.Id; //logic to get from login user
            poModel.ApprovedBy = null;

            //add transaction details input by clerk

            PurchaseOrder newPO = new PurchaseOrder();
            TransactionDetail newTD = new TransactionDetail();

            newPO.PONo = "PO" + DateTime.Now.Date.ToString("yy") + "/" + DateTime.Now.Date.ToString("MM") + "/" + GetSerialNumber();
            newPO.OrderedBy = poModel.OrderedBy;
            newPO.ApprovedBy = poModel.ApprovedBy;
            newPO.SupplierId = poModel.SupplierId;     // from view
            newPO.Amount = (decimal)(poModel.Quantity * poModel.UnitPrice);
            newPO.Date = DateTime.Now;
            newPO.Status = "Pending Approval";

            newTD.TransactionDate = newPO.Date;
            newTD.Quantity = poModel.Quantity;          // from view
            newTD.Remarks = poModel.Remarks;            // from view
            newTD.TransactionRef = newPO.PONo;
            newTD.TransactionDate = newPO.Date;
            newTD.UnitPrice = poModel.UnitPrice;        // from view
            newTD.ItemId = "C001";// poModel.ItemId;              // from view

            newPO.TransactionDetail.Add(newTD);
            _context.PurchaseOrder.Add(newPO);

            _context.SaveChanges();
            //return View("Index");//check if naming is correct
            return "success";
        }

        #region View Details of PO
        // GET: RaiseOrder/Details/id
        public ActionResult Details(string poNo)
        {
            if (poNo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PurchaseOrder purchaseOrder = _context.PurchaseOrder.SingleOrDefault(x => x.PONo == poNo);
            List<TransactionDetail> transactionDetail = _context.TransactionDetail.Where(c => c.TransactionRef == poNo).ToList();
            if (purchaseOrder == null)
            { return HttpNotFound(); }
            var poDetailsViewModel = new PoDetailsViewModel
            { PurchaseOrder = purchaseOrder, PODetails = transactionDetail };
            return View(poDetailsViewModel);
        }
        #endregion

    }
}