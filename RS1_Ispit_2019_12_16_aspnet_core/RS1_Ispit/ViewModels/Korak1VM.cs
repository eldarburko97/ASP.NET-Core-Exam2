using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class Korak1VM
    {
        public List<SelectListItem> Skole { get; set; }
        public int SkolaId { get; set; }
        public List<SelectListItem> razredi { get; set; }
        public int? RazredId { get; set; }
    }
}
