using System;
using System.Linq;
namespace api.Models
{
    public class BibliotekaRepository
    {
        private BibliotekaContext context;
        private static BibliotekaRepository instance = new BibliotekaRepository();
        public static BibliotekaRepository Instance
        {
            get { return instance; }
        }
        private BibliotekaRepository()
        {
            context = new BibliotekaContext();
        }

        public Korisnik Register(String username, String password)
        {

            if (!context.Korisnici.Any(b => b.username == username))
            {
                //kreiraj korisnika
                Posetilac posetilac = new Posetilac(username, password);
                context.Korisnici.Add(posetilac);
                context.SaveChanges();
                return posetilac;

            }
            //korisnik postoji
            return null;
        }
        public Korisnik Login(String username, String password){
            Korisnik korisnik = context.Korisnici.Where(b => b.username == username).FirstOrDefault();
            if (korisnik is object)
            {
                return korisnik;
            }     
            //korisnik ne postoji
            return null;
        }
    }


}
