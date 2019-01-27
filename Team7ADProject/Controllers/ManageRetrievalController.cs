﻿using Microsoft.AspNet.Identity;
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
    #region Sam Jing Wen


    //For SC to generate retrieval and make amendments
    [RoleAuthorize(Roles = "Store Clerk")]
    public class ManageRetrievalController : Controller
    {
        StationeryRequestService srService = StationeryRequestService.Instance;

        // GET: ManageRetrieval
        public ActionResult Index()
        {
            return View(srService.GetListRequestByItem());
        }

        [HttpPost]
        public ActionResult GenerateDisbursement(List<RequestByItemViewModel> model)
        {
            //Get user for the current http request
            string userId = User.Identity.GetUserId();

            //Generate Disbursement
            List<DisbursementByDeptViewModel> disbList = srService.GenerateDisbursement(model);

            //Update database
            srService.SaveAndDisburse(model, userId);

            return View(disbList);
        }
        

    }
    #endregion

}