using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Team7ADProject.ViewModels.Api
{
    public class RaiseRequestDTO
    {
        public List<string> UnitOfMeasure { get; set; }
        public List<string> ItemDescription { get; set; }

        public RaiseRequestDTO()
        {
            UnitOfMeasure = new List<string>();
            ItemDescription = new List<string>();
        }
    }
}