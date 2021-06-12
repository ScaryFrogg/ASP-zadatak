
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
    }
}
