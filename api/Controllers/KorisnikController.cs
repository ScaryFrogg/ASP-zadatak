using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using api.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace api.Controllers
{
    public class KorisnikController : Controller
    {
        BibliotekaRepository repo = BibliotekaRepository.Instance;
        private readonly IHubContext<RezervacijeHub> _hubContext;
        public KorisnikController(IHubContext<RezervacijeHub> hubContext)
        {
            _hubContext = hubContext;
        }
        //Bibliotekar
        public IActionResult Index()
        {
            Console.Write("HttpContext.Session.GetInt32()" + HttpContext.Session.GetInt32("role"));
            if (HttpContext.Session.GetInt32("id") is null || HttpContext.Session.GetInt32("role") != 1)
            {
                TempData["Err"] = "Ulogujte se ili napravite nalog";
                return RedirectToAction("Index", "Home");
            }

            return View(repo.rezervacijeNaCekanju());
        }
        public IActionResult NovaKnjiga()
        {
            if (HttpContext.Session.GetInt32("id") is null || HttpContext.Session.GetInt32("role") != 1)
            {
                TempData["Err"] = "Ulogujte se ili napravite nalog";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public IActionResult NovaKnjiga(string naziv, string imeAutora, string prezimeAutora, string stanje)
        {
            if (HttpContext.Session.GetInt32("id") is null || HttpContext.Session.GetInt32("role") != 1)
            {
                TempData["Err"] = "Ulogujte se ili napravite nalog";
                return RedirectToAction("Index", "Home");
            }
            if (repo.dodajKnjigu(naziv, repo.GetAutor(imeAutora, prezimeAutora), int.Parse(stanje)))
                TempData["Success"] = "Knjiga uspesno dodata";
            else
                TempData["Err"] = "Doslo je do greske probajte ponovo";
            return View();
        }

        //Posetilac
        public IActionResult Posetilac()
        {

            if (HttpContext.Session.GetInt32("id") is null)
            {
                TempData["Err"] = "Ulogujte se ili napravite nalog";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Knjige = repo.sveKnjige();
                var korisnikoveKnjige = repo.korisnikoveKnjige(HttpContext.Session.GetInt32("id").Value);
                return View(korisnikoveKnjige);
            }
        }

        public string vratiKnjigu(int knjigaId, int korisnikId){
            return repo.izbrisiRezervaciju(knjigaId,korisnikId);
        }
        //HUB
        [HttpPost]
        public string rezervacija(string korisnikId, string knjigaId)
        {
            return JsonConvert.SerializeObject(repo.createRezervacija(int.Parse(korisnikId), int.Parse(knjigaId)));
        }
        public async void odobriRezervaciju(string rezId)
        {
            var rezervacija = repo.odobriRezervaciju(int.Parse(rezId));
            var knjiga = repo.getKnjigaByID(rezervacija.knjiga.id);
            //Posalji knjigu korisniku
            await _hubContext.Clients.Client(RezervacijeHub.konektovani[rezervacija.korisnik.id]).SendAsync("OdobrenaKnjiga", JsonConvert.SerializeObject(knjiga));
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}