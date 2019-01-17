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

        // [Authorize(Roles = "Store Supervisor")]
        // GET: RaiseOrder
        public ActionResult Index()
        {
            //load supplier
            var suppliers = _context.Supplier.ToList();
            var stationeries = _context.Stationery.ToList();

            RaisePOViewModel viewModel = new RaisePOViewModel();
            viewModel.Suppliers = suppliers;
            viewModel.Stationeries = stationeries;



            viewModel.PONo = "PO" + DateTime.Now.Date.ToString("yy") + "/" + DateTime.Now.Date.ToString("MM") + "/" + "03" ;
            //viewModel.PONo = GetSerialNumber().ToString();
            ViewBag.PoNo = viewModel.PONo;
            viewModel.Categories = _context.Stationery.Select(m => m.Category).Distinct().ToList();


            //ViewBag.Cate = _context.Stationery.Select(m => m.Category).Distinct().ToList();


            return View(viewModel);
        }   

        public ActionResult ViewPo()
        {
            // to get the user ID of the current user
            string userId = User.Identity.GetUserId();
           // var query = _context.AspNetUsers.FirstOrDefault(x => x.Id == userId);

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

        // PO18/02/##
        //private static int GetSerialNumber()
        //{
        //    LogicDB context1 = new LogicDB();
        //    var lastPo = context1.PurchaseOrder.Select(m => m.PONo).LastOrDefault();
        //    string date = lastPo.Substring(2,10);
        //    string todayDate = DateTime.Today.ToString();
        //    if(date == todayDate)
        //    {
        //        string poSerial = lastPo.Substring(11);
        //        int poSerialInt = int.Parse(poSerial);
        //        return poSerialInt + 1;
        //    }

        //    else
        //    {
        //        return 1;
        //    }

           // string start = DateTime.Today.AddDays(-1).ToString();
            //return (DateTime.Now - DateTime.Parse(start)).Days;
        //}


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

            poModel.OrderedBy = "4e858936-0926-4bde-9a5f-76129ab96941"; // query.Id; //logic to get from login user
            poModel.ApprovedBy = null;

            //add transaction details input by clerk

            PurchaseOrder newPO = new PurchaseOrder();
            TransactionDetail newTD = new TransactionDetail();

            //newPO.PONo = "PO" + DateTime.Now.Date.ToString("yy") + "/" + DateTime.Now.Date.ToString("MM") + "/" + DateTime.Now.Date.ToString("dd");
            newPO.PONo = "PO19/01/03";
            newPO.OrderedBy = "4e858936-0926-4bde-9a5f-76129ab96941";
            newPO.ApprovedBy = poModel.ApprovedBy;
            newPO.SupplierId = poModel.SupplierId;     // from view
            newPO.Amount = (decimal)(poModel.Quantity * poModel.UnitPrice);
            newPO.Date = DateTime.Now;
            newPO.Status = "Pending";

            newTD.TransactionDate = newPO.Date;
            newTD.Quantity = poModel.Quantity;          // from view
            newTD.Remarks = poModel.Remarks;            // from view
            newTD.TransactionRef = newPO.PONo;
            newTD.TransactionDate = newPO.Date;
            newTD.UnitPrice = poModel.UnitPrice;        // from view
            newTD.ItemId = poModel.ItemId;              // from view

            newPO.TransactionDetail.Add(newTD);
            _context.PurchaseOrder.Add(newPO);

            _context.SaveChanges();
            //return View("Index");//check if naming is correct
            return "success";
        }
    }
}