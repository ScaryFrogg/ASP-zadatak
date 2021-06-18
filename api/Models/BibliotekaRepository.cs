using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
            return context.Rezervacije.Where(r => !r.odobrena).ToList();
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
            // var query = $"SELECT * from Knjige WHERE id IN (SELECT knjigaId From Rezervacije WHERE odobrena = false AND korisnikId = {korinsikId})";
            var query = $"SELECT Knjige.id, Knjige.autorid, Knjige.naziv from Knjige INNER JOIN Rezervacije ON Knjige.id = knjigaId WHERE odobrena = true AND korisnikId = {korinsikId}";
            Console.WriteLine(query);
            return context.Knjige.FromSqlRaw(query).ToList();
        }

        //CREATE
        public bool createKnjiga(Knjiga knjiga)
        {
            context.Knjige.Add(knjiga);
            return context.SaveChanges() == 1;
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
        public Korisnik Login(String username, String password)
        {
            Korisnik korisnik = context.Korisnici.Include("knjige").Where(b => b.username == username).FirstOrDefault();
            if (korisnik is object)
            {
                return korisnik;
            }
            //korisnik ne postoji
            return null;
        }
        public Rezervacija createRezervacija(int korisnikId, int knjigaId)
        {
            if (iznajmiKnjigu(knjigaId) is object)
            {
                Console.WriteLine("pokusavam rezervaciju");
                Posetilac posetilac = (Posetilac)context.Korisnici.Include("knjige").Where(k => k.id == korisnikId).FirstOrDefault();
                Console.WriteLine(posetilac.ToString());
                Console.WriteLine(posetilac.knjige);
                Console.WriteLine(posetilac.username);
                Rezervacija rezervacija = new Rezervacija();
                if (posetilac.knjige.Count >= 5)
                {
                    vratiKnjigu(knjigaId);
                    return null;
                }
                else
                {
                    rezervacija.korisnikId = korisnikId;
                    rezervacija.knjigaId = knjigaId;
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
            createKnjiga(new Knjiga("Prokleta Avlija", GetAutor("Ivo", "Andric"), 2));
            createKnjiga(new Knjiga("Koreni", GetAutor("Dobrica", "Cosic"), 3));
            createKnjiga(new Knjiga("Deobe", GetAutor("Dobrica", "Cosic"), 1));
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

        public Knjiga dodajKnjigu(String naziv, Autor autor, int stanje)
        {
            var knjiga = new Knjiga(naziv, autor, stanje);
            context.Knjige.Add(knjiga);
            context.SaveChanges();
            return knjiga;
        }

        public Rezervacija odobriRezervaciju(Rezervacija rezervacija)
        {
            rezervacija.odobrena = true;
            rezervacija.istek = DateTime.Today.AddDays(14).ToString();
            var korisnik = (Posetilac)context.Korisnici.Where(k => k.id == rezervacija.korisnikId).FirstOrDefault();
            korisnik.knjige.Add(getKnjigaByID(rezervacija.knjigaId));
            context.SaveChanges();
            System.Console.WriteLine("Rezervacija istice " + rezervacija.istek);
            return rezervacija;
        }
    }


}
