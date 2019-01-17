﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
{
    
    public class OwnRequisitionHistoryViewModel
    {
        #region Lynn Lynn Oo
        public string ItemDescription { get; set; }
        public int ItemQuantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        #endregion
    }

}