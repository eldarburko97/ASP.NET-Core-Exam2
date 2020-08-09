using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class TakmicenjeController : Controller
    {
        private MojContext _context;

        public TakmicenjeController(MojContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<SelectListItem> skole = _context.Skola.Select(s => new SelectListItem
            {
                Text = s.Naziv,
                Value = s.Id.ToString()
            }).ToList();

            List<SelectListItem> razredi = new List<SelectListItem>();
            for (int i = 1; i <= 4; i++)
            {
                razredi.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            Korak1VM model = new Korak1VM
            {
                Skole = skole,
                razredi = razredi
            };
            return View(model);
        }

        

        public IActionResult PrikazTakmicenja(int SkolaId, int? RazredId)
        {
            Takmicenja model = new Takmicenja();
            model.takmicenja = new List<TakmicenjaVM>();
            model.takmicenja = _context.Takmicenje.Where(w => w.SkolaId == SkolaId).Where(w => RazredId.HasValue && w.Razred == RazredId.Value).Select(s => new TakmicenjaVM
            {
                TakmicenjeId = s.TakmicenjeId,
                Predmet = s.Predmet.Naziv,
                Razred = s.Razred,
                Datum = s.Datum.ToString(),
                BrUcesnikaKojiNisuPristupili = _context.TakmicenjeUcesnik.Where(w1 => w1.TakmicenjeId == s.TakmicenjeId && w1.Pristupio == false).Count()
            }).ToList();


            foreach (var takmicenje in model.takmicenja)
            {
                List<TakmicenjeUcesnik> takmicenjeUcesnik = _context.TakmicenjeUcesnik.Where(w => w.TakmicenjeId == takmicenje.TakmicenjeId).Include(i => i.OdjeljenjeStavka).ThenInclude(ii => ii.Odjeljenje).ThenInclude(iii => iii.Skola).Include(i => i.OdjeljenjeStavka).ThenInclude(i => i.Ucenik).ToList();
                float bodovi = 0;
                string skolica = "nema";
                string odjeljenje = "nema";
                string imePrezime = "nema";
                foreach (var ucesnik in takmicenjeUcesnik)
                {
                    if (ucesnik.Bodovi > bodovi)
                    {
                        bodovi = ucesnik.Bodovi;
                        skolica = ucesnik.OdjeljenjeStavka.Odjeljenje.Skola.Naziv;
                        odjeljenje = ucesnik.OdjeljenjeStavka.Odjeljenje.Oznaka;
                        imePrezime = ucesnik.OdjeljenjeStavka.Ucenik.ImePrezime;
                    }
                }
                takmicenje.NajboljiUcenik = skolica + " " + odjeljenje + "|" + imePrezime;
            }

            Skola skola = _context.Skola.Find(SkolaId);
            model.SkolaDomacin = skola.Naziv;
            if (RazredId != null)
                model.Razred = RazredId.Value;
            else
                model.Razred = 0;

            ViewData["Skola"] = SkolaId;


            /////////////////////////////////////////////////////////////
            ///


            //TestVM model = _context.Takmicenje.Where(w => w.SkolaId == SkolaId && RazredId.HasValue && w.Razred == RazredId).Select(s => new TestVM
            //{
            //    Skola = s.Skola.Naziv,
            //    Razred = s.Razred,
            //    Rows = _context.Takmicenje.Where(w2 => w2.SkolaId == SkolaId && RazredId.HasValue && w2.Razred == RazredId).Select(s1 => new Row
            //    {
            //        TakmicenjeId = s1.TakmicenjeId,
            //        Predmet = s1.Predmet.Naziv,
            //        Razred = s1.Razred,
            //        Datum = s1.Datum.ToString(),
            //        BrUcesnikaKojiNisuPristupili = _context.TakmicenjeUcesnik.Where(w3 => w3.TakmicenjeId == s1.TakmicenjeId && w3.Pristupio == false).Count(),
            //        NajboljiUcenik = "Eldarrrr"
            //    }).ToList()
            //}).Single();

            return View(model);
        }

        public IActionResult DodajTakmicenje(int SkolaId)
        {
            DodajTakmicenjeVM model = new DodajTakmicenjeVM();
            model.SkolaDomacin = _context.Skola.Where(w => w.Id == SkolaId).Select(s => s.Naziv).Single();
            model.Predmet = new List<SelectListItem>();
            model.Predmet = _context.Predmet.Select(s => new SelectListItem
            {
                Text = s.Naziv,
                Value = s.Id.ToString()
            }).ToList();

            model.Razred = new List<SelectListItem>();
            for (int i = 1; i <= 4; i++)
            {
                model.Razred.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            model.SkolaId = SkolaId;

            return View(model);
        }

        [HttpPost]
        public IActionResult DodajTakmicenje(DodajTakmicenjeVM model)
        {
            Takmicenje takmicenje = new Takmicenje
            {
                SkolaId = model.SkolaId,
                PredmetId = model.PredmetId,
                Datum = Convert.ToDateTime(model.Datum),
                Razred = model.RazredId
            };
            _context.Takmicenje.Add(takmicenje);
            _context.SaveChanges();

            List<OdjeljenjeStavka> stavke = _context.DodjeljenPredmet.Where(w => w.PredmetId == model.PredmetId && w.ZakljucnoKrajGodine == 5).Select(s => s.OdjeljenjeStavka).ToList();
            foreach (var item in stavke)
            {
                if (_context.DodjeljenPredmet.Where(w => w.OdjeljenjeStavkaId == item.Id).Average(a => a.ZakljucnoKrajGodine) >= 4)
                    _context.TakmicenjeUcesnik.Add(new TakmicenjeUcesnik
                    {
                        TakmicenjeId = takmicenje.TakmicenjeId,
                        OdjeljenjeStavkaId = item.Id,
                        Bodovi = 0,
                        Pristupio = false
                    });
            }

            _context.SaveChanges();

            return RedirectToAction("PrikazTakmicenja", new { SkolaId = model.SkolaId, RazredId = model.RazredId });
        }

        public IActionResult Rezultati(int Id)
        {
            TakmicenjeDetailsVM model = _context.Takmicenje.Where(w => w.TakmicenjeId == Id).Select(s => new TakmicenjeDetailsVM
            {
                TakmicenjeId = Id,
                SkolaId = s.SkolaId,
                SkolaDomacin = s.Skola.Naziv,
                Predmet = s.Predmet.Naziv,
                Razred = s.Razred,
                Datum = s.Datum.ToString("ddMMyyyy")
            }).Single();


            return View(model);
        }

        public IActionResult GetTakmicenjeUcesnik(int Id)
        {
            List<TakmicenjeUcesnikVM> model = _context.TakmicenjeUcesnik.Where(w => w.TakmicenjeId == Id).Select(s => new TakmicenjeUcesnikVM
            {
                TakmicenjeUcesnikId = s.TakmicenjeUcesnikId,
                Odjeljenje = s.OdjeljenjeStavka.Odjeljenje.Oznaka,
                BrUdnevniku = s.OdjeljenjeStavka.BrojUDnevniku,
                Pristupio = s.Pristupio,
                Bodovi = s.Bodovi
            }).ToList();
            ViewData["Takmicenje"] = Id;

            return PartialView(model);
        }  //TakmicenjeId

        public IActionResult UcesnikNijePristupio(int Id)
        {

            TakmicenjeUcesnik ucesnik = _context.TakmicenjeUcesnik.Find(Id);
            ucesnik.Pristupio = !ucesnik.Pristupio;
            _context.TakmicenjeUcesnik.Update(ucesnik);
            _context.SaveChanges();
            return RedirectToAction("GetTakmicenjeUcesnik", new { Id = ucesnik.TakmicenjeId });
        }

        public IActionResult UcesnikJePristupio(int Id)
        {

            TakmicenjeUcesnik ucesnik = _context.TakmicenjeUcesnik.Find(Id);
            ucesnik.Pristupio = !ucesnik.Pristupio;
            _context.TakmicenjeUcesnik.Update(ucesnik);
            _context.SaveChanges();
            return RedirectToAction("GetTakmicenjeUcesnik", new { Id = ucesnik.TakmicenjeId });
        }




        public IActionResult AddUcesnik(int Id)
        {


            AddEditUcesnikVM model = new AddEditUcesnikVM();
            model.Ucesnik = _context.OdjeljenjeStavka.Select(s => new SelectListItem
            {
                Text = s.Odjeljenje.Oznaka + "-" + s.Ucenik.ImePrezime,
                Value = s.Id.ToString()
            }).ToList();
            model.TakmicenjeId = Id;


            return PartialView("AddEditUcesnik", model);

        }

        [HttpPost]
        public IActionResult AddUcesnik(AddEditUcesnikVM model)
        {
            _context.TakmicenjeUcesnik.Add(new TakmicenjeUcesnik
            {
                TakmicenjeId = model.TakmicenjeId,
                OdjeljenjeStavkaId = model.OdjeljenjeStavkaId,
                Bodovi = model.Bodovi,
                Pristupio = true
            });
            _context.SaveChanges();

            return RedirectToAction("GetTakmicenjeUcesnik", new { Id = model.TakmicenjeId });
        }


        public IActionResult EditUcesnik(int Id)    //TakmicenjeUcesnikId
        {
            TakmicenjeUcesnik ucesnik = _context.TakmicenjeUcesnik.Find(Id);
            AddEditUcesnikVM model = new AddEditUcesnikVM
            {
                TakmicenjeId = ucesnik.TakmicenjeId,
                TakmicenjeUcesnikId = ucesnik.TakmicenjeUcesnikId,
                Ucesnik = _context.OdjeljenjeStavka.Select(s => new SelectListItem
                {
                    Text = s.Odjeljenje.Oznaka + "-" + s.Ucenik.ImePrezime,
                    Value = s.Id.ToString()
                }).ToList(),
                Bodovi = ucesnik.Bodovi
            };

            return PartialView("AddEditUcesnik", model);
        }

        [HttpPost]
        public IActionResult EditUcesnik(AddEditUcesnikVM model)
        {
            TakmicenjeUcesnik ucesnik = _context.TakmicenjeUcesnik.FirstOrDefault(f => f.TakmicenjeUcesnikId == model.TakmicenjeUcesnikId);
            ucesnik.OdjeljenjeStavkaId = model.OdjeljenjeStavkaId;
            ucesnik.Bodovi = model.Bodovi;
            _context.TakmicenjeUcesnik.Update(ucesnik);
            _context.SaveChanges();

            return RedirectToAction("GetTakmicenjeUcesnik", new { Id = model.TakmicenjeId });
        }

        [HttpPost]
        public IActionResult SetPoints(EditBodoviVM model)
        {
            TakmicenjeUcesnik ucesnik = _context.TakmicenjeUcesnik.Find(model.TakmicenjeUcesnikId);
            ucesnik.Bodovi = model.Bodovi;
            _context.TakmicenjeUcesnik.Update(ucesnik);
            _context.SaveChanges();

            return RedirectToAction("GetTakmicenjeUcesnik", new { Id = ucesnik.TakmicenjeId });
        }

    }
}