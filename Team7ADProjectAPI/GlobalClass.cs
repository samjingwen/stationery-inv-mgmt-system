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
            var disb = context.Disbursement.Where(x => x.DisbursementNo == disbno).ToList();
            
            if (disb != null)
            {
                var onedisb = disb.First();
                string reqid = onedisb.RequestId;
                StationeryRequest req = context.StationeryRequest.Find(reqid);
                if (req != null)
                {
                    req.Status = "Void";
                }
                foreach(Disbursement dd in disb)
                {
                    dd.Status = "Void";
                    context.SaveChanges();
                    var transactionDetail = context.TransactionDetail.Where(x => x.TransactionRef == dd.RequestId).ToList();
                    if (transactionDetail != null)
                    {
                        foreach(TransactionDetail detail in transactionDetail)
                        {
                            detail.Remarks = "Void";
                        }
                      
                    }
                    var transactionDetaildisb = context.TransactionDetail.Where(x => x.TransactionRef == dd.DisbursementId).ToList();
                    if (transactionDetaildisb != null)
                    {
                        foreach (TransactionDetail detail in transactionDetaildisb)
                        {
                            detail.Remarks = "Void";
                        }

                    }

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
            string depid = GetUserDepId(userid);
            var result = from x in context.StationeryRequest
                         where x.DepartmentId == depid
                         orderby x.RequestDate descending
                         select new StationeryRequestApiModel
                         {
                             RequestId = x.RequestId,
                             RequestedBy = x.AspNetUsers1.EmployeeName,
                             RequestDate = x.RequestDate,
                             ApprovedBy = x.ApprovedBy,
                             DepartmentId = x.DepartmentId,
                             Status = x.Status,
                             Userid = userid
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

            //var resultS = from x in context.StationeryRequest
            //              where x.RequestId == rid
            //              select x;

            //StationeryRequest ss = resultS.First();
            StationeryRequest ss = RetrieveReq(rid);
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
        //Get user DepartmentID
        public string GetUserDepId(string userid)
        {
            var user = from u in context.AspNetUsers where u.Id == userid select u;
            AspNetUsers aspNetUser = user.First();
            string depId = aspNetUser.DepartmentId;
            return depId;
        }
        //Approve Req
        public bool ApproveReq(StationeryRequestApiModel req)
        {
            string depid = GetUserDepId(req.Userid);
            var dep = from d in context.Department where d.DepartmentId == depid select d;
                                   Department department= dep.FirstOrDefault();
            StationeryRequest stationeryRequest = RetrieveReq(req.RequestId);
            if (stationeryRequest != null)
            {
                stationeryRequest.ApprovedBy = req.Userid;
                stationeryRequest.Status = "Pending Disbursement";
                DateTime nextMonday = DateTime.Today.AddDays(7-((int)DateTime.Today.DayOfWeek - (int)DayOfWeek.Monday));
                DateTime nextAD = (DateTime)department.NextAvailableDate;
                //S1:postpone has expired so if the day is before friday,collection date should be set to next monday
                if (DateTime.Today > nextAD && DateTime.Now.DayOfWeek < DayOfWeek.Friday)
                { stationeryRequest.CollectionDate = nextMonday; }
               
                //S2： postpone has expired,if request is raised in Friday or after Friday,date should be set to next next monday
                if (DateTime.Today > nextAD && DateTime.Now.DayOfWeek > DayOfWeek.Friday)
                { stationeryRequest.CollectionDate = nextMonday.AddDays(7); }

                //S3:postpone is avaliable so if the day is before friday,collection date should be set to postpone date
                if (DateTime.Today < nextAD.AddDays(-3))
                { stationeryRequest.CollectionDate = department.NextAvailableDate; }

                //S4:postpone is avaliable so if the day is before friday,collection date should be set to postpone date +7
                if (nextAD.AddDays(-3) <= DateTime.Today && DateTime.Today < nextAD)
                { stationeryRequest.CollectionDate = nextAD.AddDays(7); }
                context.SaveChanges();

                #region SendEmail
                StationeryRequest ss = RetrieveReq(req.RequestId);
                string recipientEmail, subject, content;
                //recipientEmail =ss.AspNetUsers1.Email;
                recipientEmail = "gaojiaxue@outlook.com";
                subject = " Request approved!";
                content = "Your Request was approved";
                Email.Send(recipientEmail, subject, content);
                #endregion

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
                #region SendEmail
                StationeryRequest ss = RetrieveReq(req.RequestId);
                string recipientEmail, subject, content;
                //recipientEmail =ss.AspNetUsers1.Email;
                recipientEmail = "gaojiaxue@outlook.com";
                subject = " Request rejected!";
                content = "Unfortunately, your Request was rejected";
                Email.Send(recipientEmail, subject, content);
                #endregion
                return true;
            }

            return false;
        }
        #endregion



        #region Author:Lynn Lynn Oo
        //for List_View
        //public List<ReturntoWarehouseApiModel> GetItemList()
        //{
            //List<ReturntoWarehouseApiModel> apiModels = new List<ReturntoWarehouseApiModel>();
            //List<StationeryRequest> requests = context.StationeryRequest.Where(m => m.Status == "Void" || m.Status == "Partially Returned").ToList();
            //foreach (StationeryRequest current in requests)
            //{
            //    ReturntoWarehouseApiModel apiModel = new ReturntoWarehouseApiModel
            //    {
            //        RequestId = current.RequestId,
            //        TransactionDetails = current.TransactionDetail.ToList()
            //    };
            //    apiModels.Add(apiModel);
            //}
            //return apiModels;
            
            //var showList = from x in context.StationeryRequest
            //               join y in context.TransactionDetail on x.RequestId equals y.TransactionRef
            //               join z in context.Stationery on y.ItemId equals z.ItemId
            //               join d in context.Department on x.DepartmentId equals d.DepartmentId
            //               join v in context.CollectionPoint on d.CollectionPointId equals v.CollectionPointId
            //               where (x.Status == "Void" || x.Status == "Partially Returned"||x.RequestId==y.TransactionRef)

            //               select new ReturntoWarehouseApiModel
            //               {
            //                   RequestId = x.RequestId,
            //                   ItemId = z.ItemId,
            //                   Description = z.Description,
            //                   Quantity = y.Quantity,
            //                   Department = d.DepartmentName,
            //                   Location = v.CollectionDescription
            //               };
            //return showList.ToList();
        //}

        
        //public string ReturnTo(ReturntoWarehouseApiModel apiModel)
        //{
           
        //    TransactionDetail transactionInDb = context.TransactionDetail.FirstOrDefault(m => m.TransactionRef == apiModel.RequestId && m.ItemId == apiModel.ItemId && m.Remarks == "Void");
        //    // Console.WriteLine("ttttttttttttttttttttttttteeeeeeeeeeeeeeeeesssssssssssssssttttttttttttt");
        //    if (transactionInDb == null)
        //        return "error";

        //    else
        //    {
        //        string status = "fail";
        //        var stationer = context.StationeryRequest.Where(x => x.RequestId == transactionInDb.TransactionRef).First();
        //        if (stationer.Status == "Void")
        //                {
        //            var item = context.Stationery.Where(x => x.ItemId == transactionInDb.ItemId).FirstOrDefault();
        //                  item.QuantityWarehouse += transactionInDb.Stationery.QuantityTransit;
        //                  item.QuantityTransit -= transactionInDb.Stationery.QuantityTransit;
        //                   transactionInDb.Remarks = "Returned";
        //            stationer.Status = "Returned";

        //            //check if all items are returned for stationery request
        //            //       StationeryRequest stationeryRequestInDb = context.StationeryRequest.FirstOrDefault(m => m.RequestId == apiModel.RequestId);
        //            //       bool allReturned = true;
        //            //       foreach (TransactionDetail current in stationeryRequestInDb.TransactionDetail)
        //            //        {
        //            //           if (current.Remarks != "Returned")
        //            //            {
        //            //              stationeryRequestInDb.Status = "Partially Returned";
        //            //              allReturned = false;
        //            //              break;
        //            //            }
        //            //        }
        //            //if (allReturned)
        //            //{
        //            //    stationeryRequestInDb.Status = "Returned";
        //            //    context.SaveChanges();
        //            //    status = "success";

        //            //}
        //            context.SaveChanges();
        //            status = "success";
        //        }
        //        return status;

        //    }
        //   // var stationer = context.StationeryRequest.Where(x => x.RequestId == transactionInDb.TransactionRef).First();
            
        ////
        //}

    }
    #endregion

}
