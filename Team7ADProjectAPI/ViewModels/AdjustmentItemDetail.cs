using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProjectApi.ViewModels
{
    public class AdjustmentItemDetail
    {
        public string itemId;

        public string category;

        public string description;

        public string unitOfMeasure;

        public int quantityWareHouse;

        public decimal price;

        public AdjustmentItemDetail(string itemId, string category, string description, string unitOfMeasure, int quantityWareHouse, decimal price)
        {
            this.itemId = itemId;
            this.category = category;
            this.description = description;
            this.unitOfMeasure = unitOfMeasure;
            this.quantityWareHouse = quantityWareHouse;
            this.price = price;
        }
    }
}