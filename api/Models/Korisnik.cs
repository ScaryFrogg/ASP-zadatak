
namespace api.Models
{
    abstract public class Korisnik
    {
        public int id { get; set; }
        public string username  {get;set;}
        public string password  {get;set;}
    }
}
