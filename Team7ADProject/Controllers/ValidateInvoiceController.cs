using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.Service;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    #region Author:Gao Jiaxue
    [RoleAuthorize(Roles = "Store Clerk")]
    public class ValidateInvoiceController : Controller
    {
        ProcurementService pService = ProcurementService.Instance;
        //get Data
        private LogicDB _context;

        public ValidateInvoiceController()
        {
            _context = new LogicDB();
        }
        //Clean garbage
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: ValidateInvoice
        public ActionResult Index(int id = 0)
        {
            if (id == 1)
                ViewBag.successHandler = 1;
            else if (id == 2)
                ViewBag.successHandler = 2;

            return View();
        }

        public ActionResult GetSupplierDO(string id = null)
        {
            return View(pService.GetSupplierDelOrder(id));
        }

        public ActionResult GetSuppliers()
        {
            IEnumerable<BriefSupplier> suppliers = (from x in _context.Supplier
                                                    select new BriefSupplier
                                                    {
                                                        SupplierId = x.SupplierId,
                                                        SupplierName = x.SupplierName
                                                    }).Distinct().ToList();
            return Json(new { suppliers }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RenderTable(string id = null)
        {
            try
            {
                var query = pService.GetSupplierDelOrder(id);
                if (query.DelOrderDetails.Count > 0)
                {
                    return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "GetSupplierDO", query) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, html = "<h3>No Delivery Orders found!<h3>" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Create new Invoice and TransactionDetails
        [HttpPost]
        public ActionResult Validate(ValidateInvoiceViewModel model)
        {
            bool isSuccess = pService.CreateInvoice(model);

            return RedirectToAction("Index", isSuccess ? new { id = 1 } : new { id = 2 });
        }

        #endregion
    }
}
