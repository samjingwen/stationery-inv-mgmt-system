﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Team7ADProjectApi.Entities;
using Team7ADProjectApi.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Team7ADProjectApi.Models;

namespace Team7ADProjectApi.Controllers
{
    //Author: Teh Li Heng 22/1/2019
    //Delegate department head for Android
    public class DepartmentHeadController : ApiController
    {
        #region Initialization of EF same, should be same for all controllers except controller name
        private readonly LogicDB _context;

        public DepartmentHeadController()
        {
            _context = new LogicDB();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        #endregion

        [Authorize(Roles = "Department Head")]
        [HttpGet]
        [Route("api/managedepartmentRep/{id}")]
        public BriefManageDepRep GetDepartments(string id)//username
        {
            GlobalClass gc = new GlobalClass();

            BriefDepartment depinfo = gc.DepInfo(id);
            List<DepEmp> emplist = gc.ListEmp(id);
            BriefManageDepRep brief = new BriefManageDepRep();
            brief.depEmps = emplist;
          brief.depinfo = depinfo;
           
            return brief;
        }
        //[HttpGet]
        //[Route("api/managedepartmentRep/{id}")]
        //public BriefManageDepRep GetDepartmentsTest(string id)//username
        //{
        //    GlobalClass gc = new GlobalClass();

        //    BriefDepartment depinfo = gc.DepInfo(id);
        //    List<DepEmp> emplist = gc.ListEmp(id);
        //    BriefManageDepRep brief = new BriefManageDepRep();
        //    brief.depEmps = emplist;
        //    brief.depinfo = depinfo;

        //    return brief;
        //}

        #region Teh Li Heng

        [System.Web.Http.Authorize(Roles = RoleName.DepartmentHead)]
        [System.Web.Http.HttpGet]
        //[Route("api/departmenthead/getdepartmenthead/{id}")]
        public IHttpActionResult GetDepartmentHead(string id)
        {
            AspNetUsers user = _context.AspNetUsers.FirstOrDefault(m => m.Id == id);
            //AspNetRoles depHeadRoleList = _context.AspNetRoles.FirstOrDefault(m => m.Name == RoleName.ActingDepartmentHead);
            //AspNetUsers delegatedDepHead = depHeadRoleList.AspNetUsers.First();
            //DelegateDepHeadApiModel apiModel = new DelegateDepHeadApiModel();
            //apiModel.DepartmentName = user.Department.DepartmentName;

            //if (delegatedDepHead != null)
            //{
            //    apiModel.DelegatedDepartmentHeadName = delegatedDepHead.EmployeeName;
            //}
            return Ok(user);
        }

        [System.Web.Http.Authorize(Roles = RoleName.DepartmentHead)]
        [System.Web.Http.HttpPost]
        public IHttpActionResult DelegateDepartmentHead(string userId, string delegatedDepId, DateTime startDate, DateTime endDate)
        {
            AspNetUsers user = _context.AspNetUsers.FirstOrDefault(m => m.Id == userId);
            AspNetUsers delegatedDepHead = _context.AspNetUsers.First(m => m.Id == userId);
            DelegationOfAuthority doaInDb = new DelegationOfAuthority
            {
                DOAId = GenerateDelegationOfAuthorityId(),
                DelegatedBy = user.Id,
                DelegatedTo = delegatedDepHead.Id,
                StartDate = startDate,
                EndDate = endDate,
                DepartmentId = user.DepartmentId
            };

            _context.DelegationOfAuthority.Add(doaInDb);
            ApplicationUserManager userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            userManager.RemoveFromRole(doaInDb.DelegatedTo, RoleName.Employee);
            userManager.AddToRole(doaInDb.DelegatedTo, RoleName.ActingDepartmentHead);
            _context.SaveChanges();
            return Ok();
        }

        public int GenerateDelegationOfAuthorityId()
        {
            DelegationOfAuthority lastItem = _context.DelegationOfAuthority.OrderByDescending(m => m.DOAId).First();
            int lastRequestId = lastItem.DOAId;
            int newRequestId = lastRequestId + 1;
            return newRequestId;
        }
        #endregion
    }
}