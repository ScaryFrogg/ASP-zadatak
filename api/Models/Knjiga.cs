
namespace api.Models
{
    public class Knjiga
    {
        public int id { get; set; }
        public string naziv  {get;set;}
        public Autor autor  {get;set;}
        public int stanje {get;set;}
    }
}
