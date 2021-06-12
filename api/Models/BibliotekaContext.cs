using Microsoft.EntityFrameworkCore;
namespace api.Models
{
    public class BibliotekaContext : DbContext
    {
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Knjiga> Knjige { get; set; }
        public DbSet<Autor> Autori { get; set; }
        public DbSet<Rezervacija> Rezervacije { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                @"Data Source=Application.db;Cache=Shared");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bibliotekar>();
            modelBuilder.Entity<Posetilac>();
        }
    }
}
