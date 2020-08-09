using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class PopravniIspit
    {
        public int Id { get; set; }
        public Nastavnik Nastavnik1 { get; set; }
        public int NastavnikId { get; set; }

        [ForeignKey(nameof(Nastavnik2))]
        public int NastavnikId2 { get; set; }
        public Nastavnik Nastavnik2 { get; set; }
      
        [ForeignKey(nameof(Nastavnik3))]
        public int NastavnikId3 { get; set; }
        public Nastavnik Nastavnik3 { get; set; }
        public DateTime Datum { get; set; }
        public Skola Skola { get; set; }
        public int SkolaId { get; set; }
        public SkolskaGodina SkolskaGodina { get; set; }
        public int SkolskaGodinaId { get; set; }
        public Predmet Predmet { get; set; }
        public int PredmetId { get; set; }
    }
}
