using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class AddEditUcenikVM
    {
        public int PopravniIspitId { get; set; }
        public int PopravniIspitStavkaId { get; set; }
        public List<SelectListItem> Ucenici { get; set; }
        public int UcenikId { get; set; }
        public string Ucenik { get; set; }
        public int Bodovi { get; set; }
    }
}
