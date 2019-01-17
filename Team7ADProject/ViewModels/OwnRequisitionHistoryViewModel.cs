using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    #region Lynn Lynn Oo
    public class OwnRequisitionHistoryViewModel
    {
        public string ReqID
        {
            set;
            get;
        }
        public List<Stationery> StationeryItems
        {
            get;
            set;
        }
        public StationeryRequest Request;
        public string searchstring { get; set; }
        public Stationery Station{
            get {
                LogicDB context = new LogicDB();
                List<TransactionDetail> tranlist = Details;
                List<Stationery> itemlist = new List<Stationery> ();
                foreach(TransactionDetail tranvar in tranlist)
                {
                    string tid = tranvar.ItemId;
                    itemlist.Add(context.Stationery.Where(x => x.ItemId == tid).First());

                }
                StationeryItems = itemlist;
                return null; }
           
        }
        public List<TransactionDetail> Details {
            get
            {
                LogicDB con = new LogicDB();
                List<TransactionDetail> list = con.TransactionDetail.Where(x => x.TransactionRef == ReqID).ToList();
                return list;
            }
        }
        #endregion
    }
}
