
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
        public IActionResult Index()
        {
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
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}