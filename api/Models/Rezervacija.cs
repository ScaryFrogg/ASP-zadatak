
namespace api.Models
{
    public class Rezervacija
    {
        public int id { get; set; }
        public int knjigaId { get; set; }
        public int korisnikId { get; set; }

        public bool odobrena {get;set;}
        public string istek{get;set;}
    }
}
