using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace api.Models
{
    public class BibliotekaRepository
    {
        private static BibliotekaContext context;
        private static BibliotekaRepository instance = new BibliotekaRepository();
        public static BibliotekaRepository Instance
        {
            get { return instance; }
        }
        private BibliotekaRepository()
        {
            context = new BibliotekaContext();
        }

        //AUTH
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
        public Korisnik Login(String username, String password)
        {
            Korisnik korisnik = context.Korisnici.Include("knjige").Where(b => b.username == username && b.password == password).FirstOrDefault();
            if (korisnik is object)
            {
                return korisnik;
            }
            //korisnik ne postoji
            return null;
        }

        //SELCT
        public List<Korisnik> sviKorisnici()
        {
            return context.Korisnici.ToList();
        }
        public List<Knjiga> sveKnjige()
        {
            return context.Knjige.Include(k => k.autor).ToList();
        }
        public List<Autor> sviAutori()
        {
            return context.Autori.ToList();
        }
        public Knjiga getKnjigaByID(int knjigaId)
        {
            return context.Knjige.Where(k => knjigaId == k.id).FirstOrDefault();
        }
        public List<Rezervacija> rezervacijeNaCekanju()
        {
            return context.Rezervacije.Include(r => r.knjiga).Include(r => r.korisnik).Where(r => !r.odobrena).ToList();
        }
        public Autor GetAutor(string imeAutora, string prezimeAutora)
        {
            Autor autor = context.Autori.Where(autor => autor.ime == imeAutora && autor.prezime == prezimeAutora).FirstOrDefault();
            if (autor is null)
            {
                Console.WriteLine("Autor ne postoji");
                autor = new Autor();
                autor.ime = imeAutora;
                autor.prezime = prezimeAutora;
                context.Autori.Add(autor);
                context.SaveChanges();
            }
            return autor;
        }
        public List<Knjiga> korisnikoveKnjige(int korinsikId)
        {
            var query = $"SELECT * from Knjige WHERE id IN (SELECT knjigaId From Rezervacije WHERE odobrena = true AND korisnikId = {korinsikId})";
            return context.Knjige.FromSqlRaw(query).AsNoTracking().Include(k => k.autor).ToList();
        }

        //CREATE
        public bool dodajKnjigu(String naziv, Autor autor, int stanje)
        {
            try
            {
                var knjiga = new Knjiga(naziv, autor, stanje);
                context.Knjige.Add(knjiga);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Rezervacija createRezervacija(int korisnikId, int knjigaId)
        {
            if (context.Rezervacije.AsNoTracking().Where(r => r.korisnik.id == korisnikId && r.knjiga.id == knjigaId).FirstOrDefault() is object)
            {
                Console.WriteLine("rezervacija vec postoji");
                return null;
            }
            var knjiga = iznajmiKnjigu(knjigaId);
            if (knjiga is object)
            {
                Posetilac posetilac = (Posetilac)context.Korisnici.Where(k => k.id == korisnikId).FirstOrDefault();
                Rezervacija rezervacija = new Rezervacija();
                if (korisnikoveKnjige(korisnikId).Count >= 5)
                {
                    vratiKnjigu(knjigaId);
                    return null;
                }
                else
                {
                    rezervacija.korisnik = posetilac;
                    rezervacija.knjiga = knjiga;
                    context.Rezervacije.Add(rezervacija);
                    System.Console.WriteLine("Rezervacija kreirana " + rezervacija);
                }
                context.SaveChanges();
                return rezervacija;
            }
            return null;
        }
        public int dummyData()
        {
            context.Korisnici.Add(new Bibliotekar { id = 0, username = "admin@admin.com", password = "admin" });
            context.Korisnici.Add(new Posetilac { id = 1, username = "vojin@vojin.com", password = "vojin", knjige = new List<Knjiga>() });
            context.Knjige.Add(new Knjiga("Prokleta Avlija", GetAutor("Ivo", "Andric"), 2));
            context.Knjige.Add(new Knjiga("Koreni", GetAutor("Dobrica", "Cosic"), 3));
            context.Knjige.Add(new Knjiga("Deobe", GetAutor("Dobrica", "Cosic"), 1));
            return context.SaveChanges();
        }
        //UPDATE
        public Knjiga iznajmiKnjigu(int knjigaId)
        {
            var knjiga = getKnjigaByID(knjigaId);

            if (knjiga is object && knjiga.stanje > 0)
            {
                knjiga.updateKnjigaStanje(-1);
                return knjiga;
            }
            return null;
        }
        public Knjiga vratiKnjigu(int knjigaId)
        {
            var knjiga = getKnjigaByID(knjigaId);
            if (knjiga is object)
            {
                knjiga.updateKnjigaStanje(1);
                return knjiga;
            }
            return null;
        }
        public Rezervacija odobriRezervaciju(int rezId)
        {
            var rezervacija = context.Rezervacije.Include(r => r.knjiga).Include(r => r.korisnik).Where(r => r.id == rezId).FirstOrDefault();
            rezervacija.odobrena = true;
            rezervacija.istek = DateTime.Today.AddDays(14).ToString();
            context.SaveChanges();
            return rezervacija;
        }
        //DELETE
        public string izbrisiRezervaciju(int knjigaId, int korisnikId)
        {
            try
            {
                var rezervacija = context.Rezervacije.Where(r => r.knjiga.id == knjigaId && r.korisnik.id == korisnikId).FirstOrDefault();
                context.Remove(rezervacija);
                vratiKnjigu(knjigaId);
                context.SaveChanges();
                return "ok";
            }
            catch
            {
                return "err";
            }
        }
    }
}
