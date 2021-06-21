using System.Collections.Generic;

namespace api.Models
{
    public class Knjiga
    {
        public int id { get; set; }
        public string naziv { get; set; }
        public Autor autor { get; set; }
        public int stanje { get; set; }
        public Knjiga() { }
        public Knjiga(string naziv, Autor autor, int stanje)
        {
            this.naziv = naziv;
            this.autor = autor;
            this.stanje = stanje;
        }
        public void updateKnjigaStanje(int change)
        {
            stanje += change;
        }

        public IEnumerable<Posetilac> korisnici { get; set; }
    }
}
