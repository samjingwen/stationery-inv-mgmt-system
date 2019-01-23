using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.ViewModels;

namespace Team7ADProject.Controllers
{
    public class NaviController : Controller
    {
        // GET: Navi
        public ActionResult SideBar()
            
        {
            List<SideBarViewModel> menulist = new List<SideBarViewModel>();

            //All Users
            menulist.Add(new SideBarViewModel() { Roles = "All", Title="Index", Action = "Index", Controller = "Home", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "All", Title="About", Action = "About", Controller = "Home", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "All", Title="Contact", Action = "Contact", Controller = "Home", IsAction = true });

            //Store Supervisor
            menulist.Add(new SideBarViewModel() { Roles = "Store Supervisor", Title = "Stock Adjustment", Action = "Index", Controller = "ApproveAdjustment", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Supervisor", Title = "Validate Invoice", Action = "Index", Controller = "ValidateInvoice", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Supervisor", Title = "Inventory Management", Action = "Index", Controller = "ManageStationery", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Supervisor", Title = "Collection Point", Action = "Index", Controller = "ManagePoint", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Supervisor", Title = "Collection Date", Action = "Index", Controller = "ManagePostponeCollectionDate", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Supervisor", Title = "Reporting", Action = "GenerateDashboard", Controller = "GenerateReport", IsAction = true });

            //Store Manager
            menulist.Add(new SideBarViewModel() { Roles = "Store Manager", Title = "Procurement", Action = "ViewPORecord", Controller = "ApproveOrder", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Manager", Title = "Stock Adjustment", Action = "Index", Controller = "ApproveAdjustment", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Manager", Title = "Supplier Management", Action = "Index", Controller = "ManageSupplier", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Manager", Title = "Collection Point", Action = "Index", Controller = "ManagePoint", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Manager", Title = "Reporting", Action = "GenerateDashboard", Controller = "GenerateReport", IsAction = true });

            //Store Clerk
            menulist.Add(new SideBarViewModel() { Roles = "Store Clerk", Title = "Collection Point", Action = "Index", Controller = "ManagePoint", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Clerk", Title = "View Purchase Orders", Action = "Index", Controller = "RaiseOrder", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Clerk", Title = "Raise Purchase Orders", Action = "RaisePO", Controller = "RaiseOrder", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Clerk", Title = "Raise Stock Adjustment", Action = "Index", Controller = "RaiseAdjustment", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Clerk", Title = "Stationery Retrievals", Action = "Index", Controller = "ManageRetrieval", IsAction = true });

            //Department Head
            menulist.Add(new SideBarViewModel() { Roles = "Department Head", Title = "Collection Date", Action = "Index", Controller = "ManagePostponeCollectionDateDepartment", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Department Head", Title = "Stationery Requests", Action = "Index", Controller = "ManageRequest", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Department Head", Title = "Request History", Action = "Index", Controller = "History", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Department Head", Title = "Collection Point", Action = "Index", Controller = "ManagePostponeCollectionDateDepartment", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Department Head", Title = "Charge Back", Action = "ChargeBack", Controller = "ChargeBack", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Department Head", Title = "Manage Representative", Action = "Index", Controller = "ManageRep", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Department Head", Title = "Delegate Acting Head", Action = "Index", Controller = "DelegateHead", IsAction = true });

            //Department Rep
            menulist.Add(new SideBarViewModel() { Roles = "Department Representative", Title = "Collection Date", Action = "Index", Controller = "ManagePostponeCollectionDateDepartment", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Department Representative", Title = "Stationery Requests", Action = "Index", Controller = "RaiseRequest", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Department Representative", Title = "Request History", Action = "Index", Controller = "RequisitionHistory", IsAction = true });

            //Employee
            menulist.Add(new SideBarViewModel() { Roles = "Employee", Title = "Stationery Requests", Action = "Index", Controller = "RaiseRequest", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Employee", Title = "Request History", Action = "Index", Controller = "RequisitionHistory", IsAction = true });



            return PartialView("_SideBar",menulist);
        }
    }
}