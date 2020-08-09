using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class GetIspitStavkeVM
    {
        public int PopravniIspitStavkaId { get; set; }
        public string Ucenik { get; set; }
        public string Odjeljenje { get; set; }
        public int BrUDnevniku { get; set; }
        public bool Pristupio { get; set; }
        public int Bodovi { get; set; }
    }
}
