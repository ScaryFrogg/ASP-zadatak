
namespace api.Models
{
    public class Rezervacija
    {
        public int id { get; set; }
        public Knjiga knjiga { get; set; }
        public Korisnik korisnik { get; set; }

        public bool odobrena {get;set;}
        public string istek{get;set;}
    }
}
