using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    #region Sam Jing Wen


    //For SC to generate retrieval and make amendments
    [Authorize(Roles = "Store Clerk")]
    public class ManageRetrievalController : Controller
    {
        // GET: ManageRetrieval
        public ActionResult Index()
        {
            List<RequestByItemViewModel> model = new List<RequestByItemViewModel>();
            LogicDB context = new LogicDB();
            var query = context.RequestByItemView.OrderBy(x => x.ItemId).ToList();
            foreach(var i in query)
            {
                var item = model.Find(x => x.ItemId == i.ItemId);
                var disb = context.DisbByDept.Where(x => x.ItemId == i.ItemId && x.DepartmentId == i.DepartmentId).FirstOrDefault();
                if (item != null)
                {
                    var newModel = new BreakdownByDeptViewModel
                    {
                        DepartmentId = i.DepartmentId,
                        DepartmentName = i.DepartmentName,
                        Quantity = disb == null ? (int)i.Quantity : ((int)i.Quantity - (int)disb.Quantity)
                    };
                    item.requestList.Add(newModel);
                }
                else
                {
                    RequestByItemViewModel requestByItemViewModel = new RequestByItemViewModel();
                    requestByItemViewModel.ItemId = i.ItemId;
                    requestByItemViewModel.Description = i.Description;
                    requestByItemViewModel.requestList = new List<BreakdownByDeptViewModel>();
                    requestByItemViewModel.requestList.Add(new BreakdownByDeptViewModel
                    {
                        DepartmentId = i.DepartmentId,
                        DepartmentName = i.DepartmentName,
                        Quantity = disb == null ? (int)i.Quantity : ((int)i.Quantity - (int)disb.Quantity)
                    });
                    model.Add(requestByItemViewModel);
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult GenerateDisbursement(List<RequestByItemViewModel> model, string RetrievedBy)
        {
            //Create new Retrieval
            string userId = User.Identity.GetUserId();
            List<RequestByItemViewModel> modModel = srService.SaveRetrieval(model, userId);

            //Generate Disbursement
            List<DisbursementByDeptViewModel> disbList = srService.GenerateDisbursement(modModel);

            //Save to Disbursement
            List<DisbursementByDeptViewModel> modDisbList = new List<DisbursementByDeptViewModel>(disbList);
            srService.SaveDisbursement(modDisbList, userId);

            return View(disbList);
        }
        


    }
    #endregion

}