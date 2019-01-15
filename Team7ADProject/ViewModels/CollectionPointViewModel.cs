using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

//Authors: Sam Jing Wen
namespace Team7ADProject.ViewModels
{
    #region Sam Jing Wen
    public class CollectionPointViewModel
    {
        public string SelectedCP { get; set; }
        public List<CollectionPoint> CollectionPoints
        {
            get
            {
                LogicDB context = new LogicDB();
                return context.CollectionPoint.ToList();
            }
        }
    }

    public class ManagePointViewModel
    {
        public int CollectionPointId { get; set; }
        public string CollectionDescription { get; set; }
        public DateTime Time { get; set; }
    }
    #endregion

}