
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using api.Models;
using System;

namespace api.Controllers
{
    public class KorisnikController : Controller
    {
        BibliotekaRepository repo = BibliotekaRepository.Instance;

        public KorisnikController(ILogger<KorisnikController> logger)
        {

        }
        //Bibliotekar
        public IActionResult Index()
        {
            // var knjiga = new Knjiga();
            // knjiga.autor = repo.GetAutor("Ivo", "Andric");
            // knjiga.naziv = "Na Drini cuprija";
            // repo.createKnjiga(knjiga);
            // Console.WriteLine(repo.sveKnjige().Count);
            // Console.WriteLine("Autori "+repo.sviAutori().Count);
            // repo.vratiKnjigu(2);
            // Console.WriteLine("Novo stanje"+repo.getKnjigaByID(2).stanje);
            
            Console.WriteLine("Admin");
            return View();
        }


        public IActionResult Posetilac()
        {
            if (!TempData.ContainsKey("Korisnik"))
            {
                //nije ulogovan
                ViewBag.Err = "Ulogujte se ili napravite nalog";
                return RedirectToAction("Home");
            }
            Posetilac posetilac = JsonConvert.DeserializeObject<Posetilac>(TempData["Korisnik"].ToString());
            // posetilac.knjige = 
            Console.WriteLine(posetilac.id);
            var res = repo.korisnikoveKnjige(posetilac.id);
            Console.WriteLine(res);
            ViewBag.Knjige = repo.sveKnjige();
            ViewBag.Korisnik = posetilac;
            return View();
        }
        [HttpPost]
        public string rezervacija(string korisnikId, string knjigaId){
            return JsonConvert.SerializeObject(repo.createRezervacija( int.Parse(korisnikId),int.Parse(knjigaId)));
        }
        public string odobriRezervaciju(string rezString){
            var rezervacija = JsonConvert.DeserializeObject<Rezervacija>(rezString);
            repo.odobriRezervaciju(rezervacija);
            return rezervacija.knjigaId.ToString();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        }
    }