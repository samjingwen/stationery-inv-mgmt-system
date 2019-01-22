using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.ViewModels
{
    public class SideBarViewModel
    {
        public string Roles { get; set; }
        public string Title { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public bool IsAction { get; set; }
        public string Link { get; set; }
        public string Class { get; set; }
        public string SubMenu { get; set; }

    }
}