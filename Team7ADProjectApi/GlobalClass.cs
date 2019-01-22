using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProjectApi.Entities;
using Team7ADProjectApi.ViewModels;

namespace Team7ADProjectApi
{
    public class GlobalClass
    {
        LogicDB context = new LogicDB();

        #region Author: Sam Jing Wen

        public List<BriefDepartment> ListDepartment(string id)
        {
            var query = from x in context.Department
                        join y in context.AspNetUsers
                        on x.DepartmentRepId equals y.Id
                        where x.DepartmentId == id
                        select new BriefDepartment
                        {
                            DepartmentId = x.DepartmentId,
                            DepartmentName = x.DepartmentName,
                            DepartmentRepName = y.EmployeeName,
                            DepartmentRepId = x.DepartmentRepId

                        };
            return query.ToList();
        }

        public List<RequestItems> ListRequestByItem()
        {
            var query = (from x in context.RequestByItemView select new RequestItems
                            {
                               ItemId = x.ItemId,
                               Description = x.Description
                            }).Distinct().ToList();
            return query;
        }

        #endregion



        #region Author : Kay Thi Swe Tun
        public List<DepEmp> ListEmp(string id)
        {
            var depid = getDepId(id);
            var query = from y in context.AspNetUsers
                        where y.DepartmentId == depid
                        select new DepEmp
                        {
                          EName=y.EmployeeName,
                          Empid=y.Id,
                          Email=y.Email,
                          phone=y.PhoneNumber
                         };
            return query.ToList();
        }
        public string getDepId(string eid)
        {
            return context.AspNetUsers.Where(x => x.Id == eid).Select(x=>x.DepartmentId).First();
            

        }
        public BriefDepartment DepInfo(string id)
        {
            var depid = getDepId(id);
        
            var query = from x in context.Department
                        join y in context.AspNetUsers
                        on x.DepartmentRepId equals y.Id
                        where x.DepartmentId == depid
                        select new BriefDepartment
                        {
                            DepartmentId = x.DepartmentId,
                            DepartmentName = x.DepartmentName,
                            DepartmentRepName = y.EmployeeName,
                            DepartmentRepId = x.DepartmentRepId

                        };
            return query.First();
        }


        #endregion



        #region Author : Zan Tun Khine

        //retrieve all the POs
        public List<PendingPO> AllPOList()
        {
            var result = from x in context.PurchaseOrder
                         join y in context.AspNetUsers
                         on x.OrderedBy equals y.Id
                         join z in context.Supplier
                         on x.SupplierId equals z.SupplierId
                         select new PendingPO
                         {
                             PONo = x.PONo,
                             SupplierId = z.SupplierName,
                             Status = x.Status,
                             OrderedBy = y.EmployeeName,
                             Date = x.Date,
                             Amount = x.Amount
                         };
            return result.ToList();
        }

        //retrieve all the pending POs
        public List<PendingPO> PendingPOList()
        {
            var result = from x in context.PurchaseOrder
                         join y in context.AspNetUsers
                         on x.OrderedBy equals y.Id
                         join z in context.Supplier
                         on x.SupplierId equals z.SupplierId
                         where x.Status == "Pending Approval"
                         select new PendingPO
                         {
                             PONo = x.PONo,
                             SupplierId = z.SupplierName,
                             Status = x.Status,
                             OrderedBy = y.EmployeeName,
                             Date = x.Date,
                             Amount = x.Amount
                         };
            return result.ToList();
        }

        //retrieve all the pending PO details
        public List<PendingPODetails> SelectedPODetails(String poNo)
        {
            var result = from x in context.TransactionDetail         // TransactionDetail ==> x
                         join y in context.PurchaseOrder             // PurchaseOrder ==> y
                         on x.TransactionRef equals y.PONo
                         join z in context.Supplier                  // Supplier ==> z
                         on y.SupplierId equals z.SupplierId
                         join w in context.AspNetUsers               // AspNetUsers ==> w
                         on y.OrderedBy equals w.Id
                         where x.TransactionRef == poNo
                         select new PendingPODetails
                         {
                             ItemId = x.ItemId,
                             PONo = y.PONo,
                             TransactionRef = x.TransactionRef,
                             SupplierId = z.SupplierName,
                             Status = y.Status,
                             OrderedBy = w.EmployeeName,
                             Quantity = x.Quantity,
                             UnitPrice = x.UnitPrice,
                             Remarks = x.Remarks,
                             Amount = y.Amount,
                             UnitAmount = x.UnitPrice * x.Quantity
                         };
            return result.ToList();
        }

        public PurchaseOrder RetrievePO(string poNum)
        {
            return context.PurchaseOrder.Where(x => x.PONo == poNum).FirstOrDefault();
        }


        //Approve PO
        public bool ApprovePO(PurchaseOrder po)
        {
            PurchaseOrder purchaseorder = RetrievePO(po.PONo);
            if (purchaseorder != null)
            {
                purchaseorder.ApprovedBy = po.ApprovedBy;
                purchaseorder.Status = "Pending Delivery";
                context.SaveChanges();
                return true;
            }

            return false;
        }

        //Reject PO
        public bool RejectPO(PurchaseOrder po)
        {
            PurchaseOrder purchaseorder = RetrievePO(po.PONo);
            if (purchaseorder != null)
            {
                purchaseorder.ApprovedBy = po.ApprovedBy;
                purchaseorder.Status = "Rejected";
                context.SaveChanges();
                return true;
            }

            return false;
        }
        #endregion


    }
}