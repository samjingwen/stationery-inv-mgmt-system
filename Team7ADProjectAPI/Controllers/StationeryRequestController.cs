using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Team7ADProjectApi.Entities;
using Team7ADProjectApi.Models;
using Team7ADProjectApi.ViewModels;

//Author:Gao Jiaxue
namespace Team7ADProjectApi.Controllers
{
    #region Gao Jiaxue
    public class StationeryRequestController : ApiController
    {
        #region List all the requests
        //Get all StationeryRequest under this department
        //[Authorize(Roles = RoleName.DepartmentHead)]
        //[Authorize(Roles = RoleName.ActingDepartmentHead)]
        [HttpGet]
        [Route("api/stationeryrequest/getall/{*userid}")]
        public List<StationeryRequestApiModel> GetStationeryRequestList(string userid)
        { 
            GlobalClass gc = new GlobalClass();
            return gc.GetAllStationeryRequestList(userid);
        }
        //Get a selected request
        //[Authorize(Roles = RoleName.DepartmentHead)]
        //[Authorize(Roles = RoleName.ActingDepartmentHead)]
        [HttpGet]
        [Route("api/stationeryrequest/getselected/{*rid}")]
        public StationeryRequestApiModel GetSelectedStationeryRequest(string rid)
        {
            GlobalClass gc = new GlobalClass();
            return gc.SelectedStationeryRequest(rid);
        }
        #endregion
        #region Approve Req
        [HttpPost]
        [Route("api/stationeryrequest/approve")]
        public bool ApproveReq([FromBody]StationeryRequestApiModel req)
        {
            GlobalClass gc = new GlobalClass();
            return gc.ApproveReq(req);
        }
        #endregion

        #region Reject PO
        [HttpPost]
        [Route("api/stationeryrequest/reject")]
        public bool RejectPO([FromBody]StationeryRequestApiModel req)
        {
            GlobalClass gc = new GlobalClass();
            return gc.RejectReq(req);
        }
        #endregion

    }
    #endregion
}
