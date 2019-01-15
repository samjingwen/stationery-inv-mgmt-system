using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Team7ADProject.ViewModels
{
    public class ManageRepViewModel
    {
        [Required]
        [StringLength(128)]
        public string UserId { get; set; }
        public IEnumerable<SelectListItem> UserList { get; set; }

    }
}