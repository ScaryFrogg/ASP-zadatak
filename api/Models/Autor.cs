
namespace api.Models
{
    public class Autor
    {
        public int id { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }

        public string PunoIme()
        {
            return ime + " " + prezime;
        }
        public Autor(string ime,string prezime){
            this.ime= ime;
            this.prezime = prezime;
        }
        public Autor(){}
        
    }
}
