using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{

    public class PrikazPopravnihIspitaVM
    {
        public string  SkolskaGodina { get; set; }
        public string Skola { get; set; }
        public string Predmet { get; set; }
        public int SkolskaGodinaId { get; set; }
        public int SkolaId { get; set; }
        public int PredmetId { get; set; }
        public List<Row> Rows { get; set; }
    }

    public class Row
    {
        public int PopravniIspitId { get; set; }
        public string Datum { get; set; }
        public string Nastavnik { get; set; }
        public int BrUcenikaNaPoprIsp { get; set; }
        public int BrPolozenihUcenika { get; set; }
    }
}
