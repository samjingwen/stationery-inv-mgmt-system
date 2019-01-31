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
        public int SetDepartmentRep([FromBody]BriefDepartment e)//username
        {
                   
            //Retrieve department head
            //string depHeadId;
            // var user = database.AspNetUsers.Where(x => x.Id == depHeadId).FirstOrDefault();
            //Retrieve department
            var dept =_context.Department.Where(x => x.DepartmentId == e.DepartmentId).FirstOrDefault();

            //Change department rep
            string oldEmpRepId = dept.DepartmentRepId;
            //string userId = model.UserId;
            dept.DepartmentRepId = e.DepartmentRepId;
           
            //Change previous Department Rep to employee
            ApplicationUserManager manager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            manager.RemoveFromRole(oldEmpRepId,RoleName.DepartmentRepresentative);
            manager.AddToRole(oldEmpRepId,RoleName.Employee);
            //Assign new employee to Department Rep
            manager.RemoveFromRole(e.DepartmentRepId, RoleName.Employee);
            manager.AddToRole(e.DepartmentRepId, RoleName.DepartmentRepresentative);

            _context.SaveChanges();
           
            return 1;
        }

        #endregion

        #region Teh Li Heng

        [Authorize(Roles = RoleName.DepartmentHead)]
        [HttpGet]
        [Route("api/departmenthead/getdepartmenthead/{*id}")]
        public IHttpActionResult GetDepartmentHeadListEmployees(string id)
        {
            AspNetUsers user = _context.AspNetUsers.FirstOrDefault(m => m.Id == id);
            AspNetRoles depHeadRoleList = _context.AspNetRoles.FirstOrDefault(m => m.Name == RoleName.ActingDepartmentHead);
            AspNetUsers delegatedDepHead = depHeadRoleList.AspNetUsers.FirstOrDefault(m=>m.DepartmentId==user.DepartmentId);
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
            string status = "Fail to delegate. Approve all request before delegating.";
            AspNetUsers user = _context.AspNetUsers.FirstOrDefault(m => m.Id == depFromJson.UserId);
            //check if there is any request pending approval, if yes, disallow them from changing
            bool anyPendingRequest = _context.StationeryRequest.Any(m =>
                m.DepartmentId == user.DepartmentId && m.Status == "Pending Approval");
            //StationeryRequest sr = _context.StationeryRequest.FirstOrDefault(m =>m.DepartmentId == user.DepartmentId && m.Status == "Pending Approval");
            if (!anyPendingRequest)
            //if (sr==null)
            {

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
                status = "Successfully delegated.";

                //emailing the delegated head
                //string delHeadEmail = _context.Department.FirstOrDefault(m => m.DepartmentId == pair.Key).AspNetUsers3.Email;
                string recipient = "team7logicdb@gmail.com"; //dummy email used
                string title = "You've been delegated as Acting head department from " + depFromJson.StartDate +
                               " to " + depFromJson.EndDate;
                string body =
                    "Dear employee, you have been delegated as Acting head department for the period stated above. Kindly approve/reject all request while the head is out of the office.";
                Email.Send(recipient, title, body);
            }

            return Ok(status);
        }

        [Authorize(Roles = RoleName.DepartmentHead)]
        [HttpPost]
        [Route("api/departmenthead/revoke/")]
        public IHttpActionResult RevokeHead()
        {
            string status = "Fail to revoke.";
            string currentUserId = User.Identity.GetUserId();

            //removing user from role
            AspNetUsers user = _context.AspNetUsers.FirstOrDefault(m => m.Id == currentUserId);
            AspNetRoles depHeadRoleList = _context.AspNetRoles.FirstOrDefault(m => m.Name == RoleName.ActingDepartmentHead);
            AspNetUsers delegatedDepHead = depHeadRoleList.AspNetUsers.FirstOrDefault(m => m.DepartmentId == user.DepartmentId);

            //Changing the date of Delegation of authority to cut short
            DelegationOfAuthority doaInDb = _context.DelegationOfAuthority.OrderByDescending(m => m.DOAId)
                .FirstOrDefault(m => m.DelegatedTo == delegatedDepHead.Id);
            doaInDb.EndDate=DateTime.Today.AddDays(-1);

            //removing delegate head from role
            UserManager<ApplicationUser> userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            userManager.AddToRole(delegatedDepHead.Id, "Employee");
            userManager.RemoveFromRole(delegatedDepHead.Id, "Acting Department Head");
            _context.SaveChanges();
            status = "Successfully revoked.";

            return Ok(status);
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