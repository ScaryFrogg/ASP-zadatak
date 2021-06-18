using System;
using System.Collections.Generic;

namespace api.Models
{
    public class Posetilac : Korisnik
    {
        public Posetilac(String username, String password) : base(){
            this.password = password;
            this.username = username;
        }
        public Posetilac(){
            
        }
        public List<Knjiga> knjige {get;set;}
    }
}
