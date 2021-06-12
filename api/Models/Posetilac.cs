using System;

namespace api.Models
{
    public class Posetilac : Korisnik
    {
        public Posetilac(String username, String password) : base(){
            this.password = password;
            this.username = username;
        }
        public Knjiga[] knjige {get;set;}
    }
}
