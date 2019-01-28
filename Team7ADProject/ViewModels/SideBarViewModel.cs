using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.ViewModels
{
    public class SideBarViewModel
    {
        public string Roles { get; set; }
        public string Title { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public bool IsAction { get; set; }
        public string SubMenu { get; set; }

        public static List<SideBarViewModel> GenList()
        {
            List<SideBarViewModel> menulist = new List<SideBarViewModel>();
            //All Users
            menulist.Add(new SideBarViewModel() { Roles = "All", Title = "Index", SubMenu="Index", Action = "Index", Controller = "Home", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "All", Title = "About", SubMenu = "Index", Action = "About", Controller = "Home", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "All", Title = "Contact", SubMenu = "Index", Action = "Contact", Controller = "Home", IsAction = true });

            //Store Supervisor
            menulist.Add(new SideBarViewModel() { Roles = "Store Supervisor", SubMenu = "Stationery Management", Title = "Stock Adjustment", Action = "Index", Controller = "ApproveAdjustment", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Supervisor", SubMenu = "Stationery Management", Title = "Inventory Management", Action = "Index", Controller = "ManageStationery", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Supervisor", SubMenu = "Collection Management", Title = "Collection Point", Action = "Index", Controller = "ManagePoint", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Supervisor", SubMenu = "Collection Management", Title = "Collection Date", Action = "Index", Controller = "ManagePostponeCollectionDate", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Supervisor", Title = "Reporting", Action = "GenerateDashboard", Controller = "GenerateReport", IsAction = true });

            //Store Manager
            menulist.Add(new SideBarViewModel() { Roles = "Store Manager", SubMenu = "Stationery Management", Title = "Procurement", Action = "ViewPORecord", Controller = "ApproveOrder", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Manager", SubMenu = "Stationery Management", Title = "Stock Adjustment", Action = "Index", Controller = "ApproveAdjustment", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Manager", SubMenu = "Stationery Management", Title = "Supplier Management", Action = "Index", Controller = "ManageSupplier", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Manager", Title = "Collection Point", Action = "Index", Controller = "ManagePoint", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Manager", Title = "Reporting", Action = "GenerateDashboard", Controller = "GenerateReport", IsAction = true });

            //Store Clerk
            menulist.Add(new SideBarViewModel() { Roles = "Store Clerk", Title = "Collection Point", Action = "Index", Controller = "ManagePoint", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Clerk", SubMenu = "Stationery Management", Title = "Purchase Orders", Action = "Index", Controller = "RaiseOrder", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Clerk", SubMenu = "Stationery Management", Title = "Raise Stock Adjustment", Action = "Index", Controller = "RaiseAdjustment", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Clerk", SubMenu = "Stationery Management", Title = "Stationery Retrievals", Action = "Index", Controller = "ManageRetrieval", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Clerk", SubMenu = "Stationery Management", Title = "Validate Invoice", Action = "Index", Controller = "ValidateInvoice", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Clerk", Title = "Stationery Requests", Action = "Index", Controller = "RaiseRequest", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Store Clerk", Title = "Request History", Action = "Index", Controller = "RequisitionHistory", IsAction = true });

            //Department Head
            menulist.Add(new SideBarViewModel() { Roles = "Department Head", SubMenu = "Stationery Management", Title = "Collection Date", Action = "Index", Controller = "ManagePostponeCollectionDateDepartment", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Department Head", SubMenu = "Stationery Management", Title = "Stationery Requests", Action = "Index", Controller = "ManageRequest", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Department Head", SubMenu = "Stationery Management", Title = "Request History", Action = "Index", Controller = "History", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Department Head", SubMenu = "Stationery Management", Title = "Collection Point", Action = "Index", Controller = "ManagePostponeCollectionDateDepartment", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Department Head", Title = "Charge Back", Action = "ChargeBack", Controller = "ChargeBack", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Department Head", SubMenu = "Department Management", Title = "Manage Representative", Action = "Index", Controller = "ManageRep", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Department Head", SubMenu = "Department Management", Title = "Delegate Acting Head", Action = "Index", Controller = "ManageDelegate", IsAction = true });

            //Department Rep
            menulist.Add(new SideBarViewModel() { Roles = "Department Representative", Title = "Collection Date", Action = "Index", Controller = "ManagePostponeCollectionDateDepartment", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Department Representative", Title = "Stationery Requests", Action = "Index", Controller = "RaiseRequest", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Department Representative", Title = "Request History", Action = "Index", Controller = "RequisitionHistory", IsAction = true });

            //Employee
            menulist.Add(new SideBarViewModel() { Roles = "Employee", Title = "Stationery Requests", Action = "Index", Controller = "RaiseRequest", IsAction = true });
            menulist.Add(new SideBarViewModel() { Roles = "Employee", Title = "Request History", Action = "Index", Controller = "RequisitionHistory", IsAction = true });

            return menulist;
        }
        
    }
}