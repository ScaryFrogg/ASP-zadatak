using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using api.Models;
using Microsoft.AspNetCore.Http;

namespace api.Controllers
{
    public class HomeController : Controller
    {
        BibliotekaRepository repo = BibliotekaRepository.Instance;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id") is not null)
            {
                if (HttpContext.Session.GetInt32("role") == 1)
                    return RedirectToAction("Index", "Korisnik");
                else
                    return RedirectToAction("Posetilac", "Korisnik");
            }
            return View();
        }

        //Register
        [HttpPost]
        public IActionResult Index(string username, string password)
        {

            Korisnik korisnik = repo.Register(username, password);

            if (korisnik is object)
            {
                setSession(korisnik);
                return RedirectToAction("Posetilac", "Korisnik");
            }
            TempData["Err"] = "Korisnik vec postoji";
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            Korisnik korisnik = repo.Login(username, password);
            if (korisnik is object)
            {
                setSession(korisnik);
                if (korisnik is Bibliotekar)
                {
                    return RedirectToAction("Index", "Korisnik");
                }
                return RedirectToAction("Posetilac", "Korisnik");
            }
            TempData["Err"] = "Pogresna lozinka ili sifra";
            return RedirectToAction("Index", "Home");
        }
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public string dummyData()
        {
            try
            {
                repo.dummyData();
                return "ok";
            }
            catch
            {
                return "already created";
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void setSession(Korisnik korisnik)
        {
            HttpContext.Session.SetString("username", korisnik.username);
            HttpContext.Session.SetInt32("role", (korisnik is Bibliotekar ? 1 : 0));
            HttpContext.Session.SetInt32("id", korisnik.id);
        }
    }
}