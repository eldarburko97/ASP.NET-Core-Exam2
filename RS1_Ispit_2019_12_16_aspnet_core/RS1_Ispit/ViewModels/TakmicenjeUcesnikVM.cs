using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class TakmicenjeUcesnikVM
    {
        public string Odjeljenje { get; set; }
        public int BrUdnevniku { get; set; }
        public bool Pristupio { get; set; }
        public float Bodovi { get; set; }
        public int TakmicenjeUcesnikId { get; set; }
    }
}
