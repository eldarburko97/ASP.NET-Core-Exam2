using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class EditIspitVM
    {
        public int PopravniIspitId { get; set; }
        public int SkolskaGodinaId { get; set; }
        public int SkolaId { get; set; }
        public int PredmetId { get; set; }
        public string Clan1 { get; set; }
        public string Clan2 { get; set; }
        public string Clan3 { get; set; }
        public string Datum { get; set; }
        public string Skola { get; set; }
        public string SkolskaGodina { get; set; }
        public string Predmet { get; set; }
    }
}
