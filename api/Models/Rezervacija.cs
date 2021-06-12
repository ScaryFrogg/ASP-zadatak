
namespace api.Models
{
    public class Rezervacija
    {
        public int id { get; set; }
        public string knjigaId { get; set; }
        public string korisnikId { get; set; }

        public bool odobrena {get;set;}
        public string istek{get;set;}
    }
}
