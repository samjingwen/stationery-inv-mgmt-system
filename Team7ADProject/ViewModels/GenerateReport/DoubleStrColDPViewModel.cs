using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Team7ADProject.ViewModels.GenerateReport
{//Author: Elaine Chan
    [DataContract]
    public class DoubleStrColDPViewModel
    {
        public DoubleStrColDPViewModel(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public DoubleStrColDPViewModel(double y, string name, string color)
        {
            this.Y = y;
            this.Name = name;
            this.Color = color;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "x")]
        public Nullable<double> X = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "name")]
        public string Name = "";

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "color")]
        public string Color = null;
    }
}
}