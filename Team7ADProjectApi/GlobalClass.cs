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
                            EName = y.EmployeeName,
                            Empid = y.Id,
                            Email = y.Email,
                            phone = y.PhoneNumber
                        };
            return query.ToList();
        }

        public void assignDepRep(BriefDepartment e)
        {
            
           // //Retrieve department head
           // //string depHeadId;
           // // var user = database.AspNetUsers.Where(x => x.Id == depHeadId).FirstOrDefault();
           // //Retrieve department
           // var dept = context.Department.Where(x => x.DepartmentId == e.DepartmentId).FirstOrDefault();

           // //Change department rep
           // string oldEmpRepId = dept.DepartmentRepId;
           // //string userId = model.UserId;
           // dept.DepartmentRepId = e.DepartmentRepId;
           // context.SaveChanges();
           // //Change previous Department Rep to employee
           ////manager.RemoveFromRole(oldEmpRepId, "Department Representative");
           //// manager.AddToRole(oldEmpRepId, "Employee");
           // //Assign new employee to Department Rep
           //// manager.RemoveFromRole(userId, "Employee");
           // //t6manager.AddToRole(userId, "Department Representative");







        }

        public bool VoidDisbursement(string disbno)
        {
            Disbursement disb = context.Disbursement.Where(x => x.DisbursementNo == disbno).FirstOrDefault();



            if (disb != null)
            {

                string reqid = disb.RequestId;
                StationeryRequest req = context.StationeryRequest.Find(reqid);
                if (req != null)
                {
                    req.Status = "Void";
                }

                disb.Status = "Void";

                TransactionDetail transactionDetail = context.TransactionDetail.Where(x => x.TransactionRef == disb.DisbursementId).FirstOrDefault();
                if (transactionDetail != null)
                {
                    transactionDetail.Remarks = "Void";
                }

                context.SaveChanges();
                return true;
            }

            return false;




            throw new NotImplementedException();
        }


        public string getDepId(string eid)
        {
            return context.AspNetUsers.Where(x => x.Id == eid).Select(x => x.DepartmentId).First();


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
                             Amount = x.Amount,
                             ApprovedBy = x.ApprovedBy
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
                             Amount = x.Amount,
                             ApprovedBy = x.ApprovedBy
                         };
            return result.ToList();
        }

        //retrieve the selected PO details for Approve Or Reject PO
        public List<PendingPODetails> SelectedPODetails(String poNo)
        {
            var result = from x in context.TransactionDetail         // TransactionDetail ==> x
                         join y in context.PurchaseOrder             // PurchaseOrder ==> y
                         on x.TransactionRef equals y.PONo
                         join z in context.Supplier                  // Supplier ==> z
                         on y.SupplierId equals z.SupplierId
                         join w in context.AspNetUsers               // AspNetUsers ==> w
                         on y.OrderedBy equals w.Id
                         join v in context.Stationery                // Stationery ==> v
                         on x.ItemId equals v.ItemId
                         where x.TransactionRef == poNo
                         select new PendingPODetails
                         {
                             Description = v.Description,
                             TransactionId = x.TransactionId,
                             ItemId = x.ItemId,
                             Quantity = x.Quantity,
                             Remarks = x.Remarks,
                             TransactionRef = x.TransactionRef,
                             UnitPrice = x.UnitPrice,
                             PONo = y.PONo,
                             SupplierId = z.SupplierName,
                             Status = y.Status,
                             OrderedBy = w.EmployeeName,
                             Date = y.Date,
                             Amount = y.Amount,
                             UnitAmount = x.UnitPrice * x.Quantity                           

                         };
            return result.ToList();
        }

        public PurchaseOrder RetrievePO(string poNum)
        {
            return context.PurchaseOrder.Where(x => x.PONo == poNum).FirstOrDefault();
        }

        //List PONo where status is "Pending Delivery"
        public List<String> PendingDeliveryPoList()
        {
            return context.PurchaseOrder.Where(y => y.Status == "Pending Delivery").Select(x => x.PONo).ToList();
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

        //Create a Single DO 
        public bool CreateDO(DeliveryOrder dOrder)
        {
            try
            {
                context.DeliveryOrder.Add(dOrder);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Create multiple DOs

        public bool CreateMDO(List<DeliveryOrder> dOrderlist)
        {
            try
            {
                foreach (DeliveryOrder doitem in dOrderlist)
                {
                    context.DeliveryOrder.Add(doitem);
                }
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        //To retrieve stationery based on the itemID
        public Stationery RetrieveStationery(string itemID)
        {
            return context.Stationery.Where(x => x.ItemId == itemID).FirstOrDefault();
        }

        public bool CreateMDO2(List<AckDeliveryDetails> itemlist)
        {
            //string userId = itemlist[0].AcceptedBy;
            //string poNum = itemlist[0].PONo;
            //List<String> Items = itemlist.Select(x => x.PONo).ToList();
            String PONum = "";
            String SupplierId = "";
            if (itemlist != null)
            {
                string newDOId = GenerateDoId();            
                foreach (var item in itemlist)
                {
                    Stationery stat = RetrieveStationery(item.ItemId);
                    stat.QuantityWarehouse = stat.QuantityWarehouse + item.Quantity;
                    PurchaseOrder po = RetrievePO(item.PONo);
                    po.Status = "Delivered";
                    PONum = po.PONo;
                    SupplierId = po.SupplierId;

                    TransactionDetail transac = new TransactionDetail
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        Remarks = "Delivered",
                        TransactionRef = newDOId,
                        TransactionDate = DateTime.Today,
                        UnitPrice = context.TransactionDetail.Where(x => x.ItemId == item.ItemId && x.TransactionRef == item.PONo).Select(y => y.UnitPrice).FirstOrDefault()

                    };          
                    context.TransactionDetail.Add(transac);
                    context.SaveChanges();
                }

                DeliveryOrder newDo = new DeliveryOrder
                {
                    DelOrderId = newDOId,
                    DelOrderNo = itemlist[0].DelOrderNo,
                    SupplierId = SupplierId,
                    AcceptedBy = itemlist[0].AcceptedBy,
                    Date = DateTime.Today,
                    PONo = itemlist[0].PONo,
                    Status = "UNBILLED"
                };
            context.DeliveryOrder.Add(newDo);
                context.SaveChanges();
            }
           return true;
     
        }

    // Generate DOId 
    // Format : GRN19-####

    public string GenerateDoId()
        {
            var lastDoNo = context.DeliveryOrder.OrderByDescending(x => x.DelOrderId).FirstOrDefault();
            string current = lastDoNo.DelOrderId.Substring(6);
            int newSerial = Int32.Parse(current) + 1;  
            return ("GRN" + DateTime.Now.Date.ToString("yy") + "-" + newSerial.ToString("0000"));
        }

        #endregion


        #region Gao Jiaxue

        public List<StationeryRequestApiModel> GetAllStationeryRequestList(string userid)
        {
            //var result = from m in context.TransactionDetail
            //             join x in context.StationeryRequest 
            //             on m.TransactionRef equals x.RequestId
            //             join y in context.AspNetUsers
            //             on x.RequestedBy equals y.Id
            //             join z in context.Department
            //             on y.DepartmentId equals z.DepartmentId
            //             where y.Id == userid
            //             //&&x.Status== "Pending Approval"
            //             select new StationeryRequestApiModel {
            //                 RequestId = x.RequestId,
            //                 RequestedBy = x.AspNetUsers1.EmployeeName,
            //                 RequestDate = x.RequestDate,
            //                 ApprovedBy=x.ApprovedBy,
            //                 DepartmentId=x.DepartmentId,
            //                 Status=x.Status,               
            //             };
            var result = from x in context.StationeryRequest
                         join y in context.AspNetUsers
                         on x.RequestedBy equals y.Id
                         join z in context.Department
                         on y.DepartmentId equals z.DepartmentId
                         where y.Id == userid
                         //&&x.Status== "Pending Approval"
                         select new StationeryRequestApiModel
                         {
                             RequestId = x.RequestId,
                             RequestedBy = x.AspNetUsers1.EmployeeName,
                             RequestDate = x.RequestDate,
                             ApprovedBy = x.ApprovedBy,
                             DepartmentId = x.DepartmentId,
                             Status = x.Status,
                         };
                   return result.ToList();
        }


        public StationeryRequestApiModel SelectedStationeryRequest(String rid)
        {
            var resultT = from x in context.TransactionDetail
                          where x.TransactionRef == rid
                          select new RequestTransactionDetailApiModel {
                              TransactionId=x.TransactionId,
                              TransactionRef=rid,
                              ItemId=x.ItemId,
                              Quantity=x.Quantity,
                              UnitPrice=x.UnitPrice
                          };

            var resultS = from x in context.StationeryRequest
                          where x.RequestId == rid
                          select x;

            StationeryRequest ss = resultS.First();
           List<RequestTransactionDetailApiModel> tt = resultT.ToList();
          
            StationeryRequestApiModel stModel = new StationeryRequestApiModel();
            stModel.RequestId = rid;
            stModel.RequestedBy = ss.AspNetUsers1.EmployeeName;
            stModel.RequestDate = ss.RequestDate;
            stModel.Status = ss.Status;
            stModel.requestTransactionDetailApiModels= tt;

            return stModel;
        }
        //get request ID
        public StationeryRequest RetrieveReq(string rid)
        {
            return context.StationeryRequest.Where(x => x.RequestId == rid).FirstOrDefault();
        }

        //Approve Req
        public bool ApproveReq(StationeryRequestApiModel req)
        {
            StationeryRequest stationeryRequest = RetrieveReq(req.RequestId);
            if (stationeryRequest != null)
            {
                stationeryRequest.ApprovedBy = req.ApprovedBy;
                stationeryRequest.Status = "Pending Disbursement";
                context.SaveChanges();
                return true;
            }

            return false;
        }

        //Reject Req
        public bool RejectReq(StationeryRequestApiModel req)
        {
            StationeryRequest stationeryRequest = RetrieveReq(req.RequestId);
            if (stationeryRequest != null)
            {
                stationeryRequest.ApprovedBy = req.ApprovedBy;
                stationeryRequest.Status = "Rejected";
                context.SaveChanges();
                return true;
            }

            return false;
        }
        #endregion



        #region Author:Lynn Lynn Oo
        //for List_View
        public List<ReturntoWarehouseApiModel> GetItemList()
        {
            var showList = from x in context.StationeryRequest
                           join y in context.TransactionDetail on x.RequestId equals y.TransactionRef
                           join z in context.Stationery on y.ItemId equals z.ItemId
                           join d in context.Department on x.DepartmentId equals d.DepartmentId
                           join v in context.CollectionPoint on d.CollectionPointId equals v.CollectionPointId
                           where (x.Status == "Void" || x.Status == "Partially Returned")

                           select new ReturntoWarehouseApiModel
                           {
                               RequestId = x.RequestId,
                               ItemId = z.ItemId,
                               Description = z.Description,
                               Quantity = y.Quantity,
                               Department = d.DepartmentName,
                               Location = v.CollectionDescription,
                           };
            return showList.ToList();
        }

        
        public string Return(ReturntoWarehouseApiModel apiModel)
        {
            TransactionDetail transactionInDb = context.TransactionDetail.FirstOrDefault(m => m.TransactionRef == apiModel.RequestId && m.ItemId == apiModel.ItemId && m.Remarks == "Void");
            if (transactionInDb.StationeryRequest.Status == "Void")
            {
                string status = "fail";
                transactionInDb.Stationery.QuantityWarehouse += transactionInDb.Stationery.QuantityTransit;
                transactionInDb.Stationery.QuantityTransit -= transactionInDb.Stationery.QuantityTransit;
                transactionInDb.Remarks = "Returned";
                //check if all items are returned for stationery request
                StationeryRequest stationeryRequestInDb = context.StationeryRequest.FirstOrDefault(m => m.RequestId == apiModel.RequestId);
                bool allReturned = true;
                foreach (TransactionDetail current in stationeryRequestInDb.TransactionDetail)
                {
                    if (current.Remarks != "Returned")
                    {
                        stationeryRequestInDb.Status = "Partially Returned";
                        allReturned = false;
                        break;
                    }
                }
                if (allReturned)
                    stationeryRequestInDb.Status = "Returned";
                context.SaveChanges();
                status = "success";
                return status;
            }
            return "error";
        }

    }
    #endregion

}
