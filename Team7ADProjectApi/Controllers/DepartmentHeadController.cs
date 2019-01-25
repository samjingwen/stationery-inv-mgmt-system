using System;
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


        #region Author : Kay Thi Swe Tun
        [Authorize(Roles = "Department Head")]
        [HttpGet]
        [Route("api/managedepartmentRep/{id}")]
        public BriefDepartment GetDepartments(string id)//username
        {
            GlobalClass gc = new GlobalClass();

            BriefDepartment depinfo = gc.DepInfo(id);
           
            return depinfo;
        }

        [Authorize(Roles = "Department Head")]
        [HttpGet]
        [Route("api/managedepartmentEmp/{id}")]
        public IEnumerable<DepEmp>  GetDepartmentEmps(string id)//username
        {
            GlobalClass gc = new GlobalClass();

            List<DepEmp> emplist = gc.ListEmp(id);
            

            return emplist;
        }


        [Authorize(Roles = "Department Head")]
        [HttpPost]
        [Route("api/managedepartmentEmp")]
        public int GetDepartmentEmps([FromBody]BriefDepartment e)//username
        {
            GlobalClass gc = new GlobalClass();
            gc.assignDepRep(e);

            Console.WriteLine(e.DepartmentName);
            //List<DepEmp> emplist = gc.ListEmp(id);


            return 1;
        }

        #endregion
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

        [Authorize(Roles = RoleName.DepartmentHead)]
        [HttpGet]
        [Route("api/departmenthead/getdepartmenthead/{*id}")]
        public IHttpActionResult GetDepartmentHeadListEmployees(string id)
        {
            AspNetUsers user = _context.AspNetUsers.FirstOrDefault(m => m.Id == id);
            AspNetRoles depHeadRoleList = _context.AspNetRoles.FirstOrDefault(m => m.Name == RoleName.ActingDepartmentHead);
            AspNetUsers delegatedDepHead = depHeadRoleList.AspNetUsers.FirstOrDefault();
            DelegateDepHeadApiModel apiModel = new DelegateDepHeadApiModel();

            apiModel.DepartmentName = user.Department.DepartmentName;
            IEnumerable<AspNetRoles> employeesInDepartment = _context.AspNetRoles.Where(m => m.Name == RoleName.ActingDepartmentHead || m.Name==RoleName.Employee).ToList();
            List<AspNetUsers> employeesForDelegate = new List<AspNetUsers>();
            foreach (var currentRole in employeesInDepartment)
            {
                employeesForDelegate.AddRange(currentRole.AspNetUsers.ToList());
            }

            List<AspNetUsers> employeesForDelegateFilterDepartment =
                employeesForDelegate.Where(m => m.DepartmentId == user.DepartmentId).ToList();
            List<EmployeeDto> employeeDtos = new List<EmployeeDto>();
            foreach (AspNetUsers current in employeesForDelegateFilterDepartment)
            {
                EmployeeDto tempEmployeeDto = new EmployeeDto
                {
                    Name = current.EmployeeName,
                    Id = current.Id
                };
                employeeDtos.Add(tempEmployeeDto);
                apiModel.Employees = employeeDtos;
            }
            
            if (delegatedDepHead != null)
            {
                apiModel.DelegatedDepartmentHeadName = delegatedDepHead.EmployeeName;
            }
            return Ok(apiModel);
        }

        [Authorize(Roles = RoleName.DepartmentHead)]
        [HttpPost]
        [Route("api/departmenthead/setdepartmenthead")]
        public IHttpActionResult DelegateDepartmentHead(DelegateDepHeadApiModel depFromJson)
        {
            AspNetUsers user = _context.AspNetUsers.FirstOrDefault(m => m.Id == depFromJson.UserId);
            AspNetUsers delegatedDepHead = _context.AspNetUsers.FirstOrDefault(m => m.EmployeeName == depFromJson.DelegatedDepartmentHeadName);
            DelegationOfAuthority doaInDb = new DelegationOfAuthority
            {
                DOAId = GenerateDelegationOfAuthorityId(),
                DelegatedBy = user.Id,
                DelegatedTo = delegatedDepHead.Id,
                StartDate = depFromJson.StartDate,
                EndDate = depFromJson.EndDate,
                DepartmentId = user.DepartmentId
            };

            _context.DelegationOfAuthority.Add(doaInDb);
            ApplicationUserManager userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
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