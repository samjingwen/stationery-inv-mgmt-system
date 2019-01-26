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


//Authors Zan Tun Khine
namespace Team7ADProject.Controllers
{
    //For SC to raise PO
    [RoleAuthorize(Roles = "Store Clerk")]
    public class RaiseOrderController : Controller
    {

        private LogicDB _context = new LogicDB();
        static string currentUserId = "";
        #region Zan Tun Khine

        #region For the Store Staff To View His/Her Own POs
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            currentUserId = userId;
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
            //Validation
            string result = "Error! Request Failed!";

            bool validQuantity = false;
            for (int i = 0; i < poModel.Length; i++)
            {
                validQuantity = poModel[i].Quantity > 0;
                if (validQuantity != true)
                    break;
            }
            if (!validQuantity)
            {
                result = "Invalid input! Kindly raise a valid request!";
            }


            //Creating new entries
            else
            {
                // to get the user ID of the current user
                string userId = User.Identity.GetUserId();
                var query = _context.AspNetUsers.FirstOrDefault(x => x.Id == userId);

                // to get the list of suppliers
                List<String> uniqueSupplier = poModel.Select(m => m.SupplierId).Distinct().ToList();

                foreach (String current in uniqueSupplier)
                {
                    string newPOnum = "PO" + DateTime.Now.Date.ToString("yy") + "/" + DateTime.Now.Date.ToString("MM") + "/" + GetSerialNumber();
   
                    decimal? amount = 0;
                    foreach (var po in poModel)
                    {
                        if (po.SupplierId == current)
                        {
                            decimal? total = (po.Quantity * po.UnitPrice);
                            amount = amount + total;
                            TransactionDetail newTD = new TransactionDetail
                            {                             
                                ItemId = po.Description,
                                Quantity = po.Quantity,
                                Remarks = "Pending Approval",
                                TransactionRef = newPOnum,
                                TransactionDate = DateTime.Today,
                                UnitPrice = po.UnitPrice
                            };
                            _context.TransactionDetail.Add(newTD);
                            _context.SaveChanges();                         
                        }
                    }
                    PurchaseOrder newPO = new PurchaseOrder()
                    {
                        PONo = newPOnum,
                        SupplierId = current,
                        OrderedBy = query.Id,
                        ApprovedBy = null,
                        Amount = Convert.ToDecimal(amount),
                        Date = DateTime.Today,
                        Status = "Pending Approval"
                    };
                    _context.PurchaseOrder.Add(newPO);
                    _context.SaveChanges();
                }
                SendEmail();
                result = "A PO has been raised successfully!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #endregion

        #region Email

        public void SendEmail()
        {
            
            LogicDB context = new LogicDB();
            // to get the user ID of the current user
            string userId = User.Identity.GetUserId();
            string sender = context.AspNetUsers.FirstOrDefault(x => x.Id == userId).EmployeeName;   
            string receipient = "zantunkhine@googlemail.com";
            string subject = "New Pending PO";
            string content = String.Format("Dear {0},{1}You have new Purchase Orders pending for approval.{2}{3} Regards,{4}{5}", "Manager", Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, sender);
            Email.Send(receipient,subject, content);
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

        #endregion

    }
}