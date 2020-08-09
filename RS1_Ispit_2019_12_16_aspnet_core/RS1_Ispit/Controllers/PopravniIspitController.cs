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
    public class PopravniIspitController : Controller
    {
        private MojContext _context;
        public PopravniIspitController(MojContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            Korak1VM model = new Korak1VM
            {
                SkolskaGodina = _context.SkolskaGodina.Select(s => new SelectListItem
                {
                    Text = s.Naziv,
                    Value = s.Id.ToString()
                }).ToList(),
                Skola = _context.Skola.Select(s1 => new SelectListItem
                {
                    Text = s1.Naziv,
                    Value = s1.Id.ToString()
                }).ToList(),
                Predmet = _context.Predmet.Select(s2 => new SelectListItem
                {
                    Text = s2.Naziv,
                    Value = s2.Id.ToString()
                }).ToList()
            };
            return View(model);
        }

        public IActionResult PrikazPopravnihIspita(int SkolskaGodinaId, int SkolaId, int PredmetId)
        {
            PrikazPopravnihIspitaVM model = new PrikazPopravnihIspitaVM
            {
                SkolskaGodina=_context.SkolskaGodina.Find(SkolskaGodinaId).Naziv,
                Skola=_context.Skola.Find(SkolaId).Naziv,
                Predmet=_context.Predmet.Find(PredmetId).Naziv,
                SkolskaGodinaId=SkolskaGodinaId,
                SkolaId=SkolaId,
                PredmetId=PredmetId
            };
            model.Rows = _context.PopravniIspit.Where(w => w.SkolskaGodinaId == SkolskaGodinaId && w.SkolaId == SkolaId && w.PredmetId == PredmetId).Select(s => new Row
            {
                PopravniIspitId = s.Id,
                Datum = s.Datum.ToString(),
                Nastavnik = s.Nastavnik1.Ime+s.Nastavnik1.Prezime,
                BrUcenikaNaPoprIsp = _context.PopravniIspitStavka.Where(w1=>w1.PopravniIspitId==s.Id).Count(),
                BrPolozenihUcenika = _context.PopravniIspitStavka.Where(w2=>w2.PopravniIspitId==s.Id && w2.Bodovi>50).Count()
            }).ToList();




            return View(model);
        }

        public IActionResult Dodaj(int SkolskaGodinaId, int SkolaId, int PredmetId)
        {
            DodavanjePopravnogIspitaVM model = new DodavanjePopravnogIspitaVM
            {
                Clan = _context.Nastavnik.Select(s => new SelectListItem
                {
                    Text = s.Ime + " " + s.Prezime,
                    Value = s.Id.ToString()
                }).ToList(),
                Clan2 = _context.Nastavnik.Select(s2 => new SelectListItem
                {
                    Text = s2.Ime + " " + s2.Prezime,
                    Value = s2.Id.ToString()
                }).ToList(),
                Clan3 = _context.Nastavnik.Select(s3 => new SelectListItem
                {
                    Text = s3.Ime + " " + s3.Prezime,
                    Value = s3.Id.ToString()
                }).ToList(),
                SkolskaGodinaId = SkolskaGodinaId,
                SkolaId = SkolaId,
                PredmetId = PredmetId,
                Skola = _context.Skola.Find(SkolaId).Naziv,
                SkolskaGodina = _context.SkolskaGodina.Find(SkolskaGodinaId).Naziv,
                Predmet = _context.Predmet.Find(PredmetId).Naziv
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Dodaj(DodavanjePopravnogIspitaVM model)
        {
            PopravniIspit ispit = new PopravniIspit
            {
                NastavnikId = model.ClanId1,
                NastavnikId2 = model.ClanId2,
                NastavnikId3 = model.ClanId3,
                Datum = Convert.ToDateTime(model.Datum),
                SkolaId = model.SkolaId,
                SkolskaGodinaId = model.SkolskaGodinaId,
                PredmetId = model.PredmetId
            };
            _context.PopravniIspit.Add(ispit);
            _context.SaveChanges();

            var ucenici = _context.OdjeljenjeStavka.ToList();
            var predmeti = _context.DodjeljenPredmet.ToList();
            List<OdjeljenjeStavka> lista = new List<OdjeljenjeStavka>();
            foreach (var u in ucenici)
            {
                foreach (var dp in predmeti)
                {
                    if (u.Id == dp.OdjeljenjeStavkaId && dp.PredmetId == ispit.PredmetId && dp.ZakljucnoKrajGodine == 1)
                    {
                        lista.Add(u);
                    }
                }
            }


            foreach (var item in lista)
            {
                int broj = 0;
                // var ucenik = _context.DodjeljenPredmet.Where(w => w.OdjeljenjeStavkaId == item.Id && w.PredmetId == ispit.PredmetId && w.ZakljucnoKrajGodine == 1).Select(s => s.OdjeljenjeStavka).Single();

                PopravniIspitStavka stavka = new PopravniIspitStavka
                {
                    PopravniIspitId = ispit.Id,
                    OdjeljenjeStavkaId = item.Id
                };
                foreach (var p in predmeti)
                {
                    if (item.Id == p.OdjeljenjeStavkaId && p.ZakljucnoKrajGodine == 1)
                    {
                        broj++;
                    }
                }
                if (broj >= 3)
                {
                    stavka.Bodovi = 0;
                    stavka.Pristupio = false;
                }
                else
                {
                    stavka.Bodovi = 30;
                    stavka.Pristupio = true;
                }
               

                _context.PopravniIspitStavka.Add(stavka);
                _context.SaveChanges();
            }

            return RedirectToAction("PrikazPopravnihIspita", new { SkolskaGodinaId = ispit.SkolskaGodinaId, SkolaId = ispit.SkolaId, PredmetId = ispit.PredmetId });
        }

        public IActionResult EditIspit(int Id)
        {
            var ispit = _context.PopravniIspit.Where(w => w.Id == Id).Select(s => new EditIspitVM
            {
                PopravniIspitId = Id,
                SkolskaGodinaId = s.SkolskaGodinaId,
                SkolaId = s.SkolaId,
                PredmetId = s.PredmetId,
                Clan1 = s.Nastavnik1.Ime + s.Nastavnik1.Prezime,
                Clan2 = s.Nastavnik2.Ime + s.Nastavnik2.Prezime,
                Clan3 = s.Nastavnik3.Ime + s.Nastavnik3.Prezime,
                Datum = s.Datum.ToString(),
                Skola = s.Skola.Naziv,
                SkolskaGodina = s.SkolskaGodina.Naziv,
                Predmet = s.Predmet.Naziv
            }).Single();

            return View(ispit);
        }

        public IActionResult GetIspitStavke(int Id)
        {
            var list = _context.PopravniIspitStavka.Where(w => w.PopravniIspitId == Id).Select(s => new GetIspitStavkeVM
            {
                PopravniIspitStavkaId = s.Id,
                Ucenik = s.OdjeljenjeStavka.Ucenik.ImePrezime,
                Odjeljenje = s.OdjeljenjeStavka.Odjeljenje.Oznaka,
                BrUDnevniku = s.OdjeljenjeStavka.BrojUDnevniku,
                Pristupio = s.Pristupio,
                Bodovi = s.Bodovi
            }).ToList();

            return PartialView(list);
        }

        public IActionResult UcenikJeOdsutan(int Id)
        {
            PopravniIspitStavka stavka = _context.PopravniIspitStavka.Find(Id);
            stavka.Pristupio = !stavka.Pristupio;
            _context.PopravniIspitStavka.Update(stavka);
            _context.SaveChanges();
            return RedirectToAction("GetIspitStavke", new { Id = stavka.PopravniIspitId });
        }

        public IActionResult UcenikJePrisutan(int Id)
        {
            PopravniIspitStavka stavka = _context.PopravniIspitStavka.Find(Id);
            stavka.Pristupio = !stavka.Pristupio;
            _context.PopravniIspitStavka.Update(stavka);
            _context.SaveChanges();
            return RedirectToAction("GetIspitStavke", new { Id = stavka.PopravniIspitId });
        }

        public IActionResult AddUcenik(int Id)
        {
            PopravniIspit ispit = _context.PopravniIspit.Find(Id);
            AddEditUcenikVM model = new AddEditUcenikVM
            {
                PopravniIspitId = Id,
                Ucenici = _context.DodjeljenPredmet.Where(w => w.PredmetId == ispit.PredmetId).Select(s => new SelectListItem
                {
                    Text = s.OdjeljenjeStavka.Ucenik.ImePrezime,
                    Value = s.OdjeljenjeStavkaId.ToString()
                }).ToList()
            };
            return PartialView("AddEditUcenik", model);
        }

        [HttpPost]
        public IActionResult AddUcenik(AddEditUcenikVM model)
        {
            PopravniIspitStavka stavka = new PopravniIspitStavka
            {
                PopravniIspitId = model.PopravniIspitId,
                OdjeljenjeStavkaId = model.UcenikId,
                Bodovi = model.Bodovi
            };
            if (model.Bodovi > 0)
                stavka.Pristupio = true;
            else
                stavka.Pristupio = false;
            _context.PopravniIspitStavka.Add(stavka);
            _context.SaveChanges();
            return RedirectToAction("GetIspitStavke", new { Id = model.PopravniIspitId });
        }

        public IActionResult EditStavka(int Id)
        {
            AddEditUcenikVM model = _context.PopravniIspitStavka.Where(w => w.Id == Id).Select(s => new AddEditUcenikVM
            {
                PopravniIspitStavkaId = Id,
                Ucenik = s.OdjeljenjeStavka.Ucenik.ImePrezime,
                Bodovi = s.Bodovi
            }).Single();
            return PartialView("AddEditUcenik", model);
        }

        [HttpPost]
        public IActionResult EditStavka(AddEditUcenikVM model)
        {
            PopravniIspitStavka stavka = _context.PopravniIspitStavka.Find(model.PopravniIspitStavkaId);
            stavka.Bodovi = model.Bodovi;
            _context.PopravniIspitStavka.Update(stavka);
            _context.SaveChanges();
            return RedirectToAction("GetIspitStavke", new { Id = stavka.PopravniIspitId });
        }

        [HttpPost]
        public IActionResult EditBodovi(EditBodoviVM model)
        {
            PopravniIspitStavka stavka = _context.PopravniIspitStavka.Find(model.PopravniIspitStavkaId);
            stavka.Bodovi = model.Bodovi;
            _context.PopravniIspitStavka.Update(stavka);
            _context.SaveChanges();

            return RedirectToAction("GetIspitStavke", new { Id = stavka.PopravniIspitId });
        }


        //public IActionResult EditBodovi(string para1, string para2)
        //{
        //    int bodovi = Int32.Parse(para1);
        //    int PopravniIspitStavkaId = Int32.Parse(para2);
        //    PopravniIspitStavka stavka = _context.PopravniIspitStavka.Find(PopravniIspitStavkaId);
        //    stavka.Bodovi = bodovi;
        //    _context.PopravniIspitStavka.Update(stavka);
        //    _context.SaveChanges();
        //    return RedirectToAction("GetIspitStavke", new { Id = stavka.PopravniIspitId });
        //}
    }
}