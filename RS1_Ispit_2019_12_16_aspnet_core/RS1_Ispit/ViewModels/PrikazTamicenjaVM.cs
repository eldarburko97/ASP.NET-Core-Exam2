using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{

    public class PrikazTakmicenjaVM
    {
        public List<Rows> Takmicenja { get; set; }
        public string SkolaDomacin { get; set; }
    }
    public class Rows
    {
        public int TakmicenjeId { get; set; }
        public string Predmet { get; set; }
        public int Razred { get; set; }
        public string Datum { get; set; }
        public int BrUcesnikaKojiNisuPristupili { get; set; }
        public string NajboljiUcenik { get; set; }
       
    }
}
