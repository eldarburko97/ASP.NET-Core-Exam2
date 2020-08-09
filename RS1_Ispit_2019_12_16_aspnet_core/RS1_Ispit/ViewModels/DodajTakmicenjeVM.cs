using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DodajTakmicenjeVM
    {
        public int SkolaId { get; set; }
        public string SkolaDomacin { get; set; }
        public List<SelectListItem> Predmet { get; set; }
        public int PredmetId { get; set; }
        public List<SelectListItem> Razred { get; set; }
        public int RazredId { get; set; }
        public string Datum { get; set; }
    }
}
