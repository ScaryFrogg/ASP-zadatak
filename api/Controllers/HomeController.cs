
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using api.Models;
using System;

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
            return View();
        }
        [TempData]
        public string Korisnik { get; set; }
        [HttpPost]
        public IActionResult Index(string username, string password)
        {
     
            Korisnik korisnik = repo.Register(username, password);

            if (korisnik is object)
            {
                Korisnik = JsonConvert.SerializeObject(korisnik);
                return RedirectToAction("Posetilac", "Korisnik");
            }
            ViewBag.Err = "Korisnik vec postoji";
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            Korisnik korisnik = repo.Login(username,password);
            if(korisnik is object){
                Korisnik = JsonConvert.SerializeObject(korisnik);
                return RedirectToAction("Posetilac", "Korisnik");
            }


            ViewBag.Err = "Login";
            return RedirectToAction("Index","Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}