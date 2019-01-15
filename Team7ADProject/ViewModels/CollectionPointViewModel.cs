using System;
using System.Collections.Generic;
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
    #endregion

}