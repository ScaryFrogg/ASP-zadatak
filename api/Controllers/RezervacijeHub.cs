using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using api.Models;
using Newtonsoft.Json;

namespace api.Controllers
{
    public class RezervacijeHub : Hub
    {
        public async Task NovaRezervacija(string rezervacija)
        {
            
            await Clients.All.SendAsync("NovaRezervacija",rezervacija);
        }        
        public async Task OdobrenaKnjiga(string knjigaId)
        {
            System.Console.WriteLine("knjiga "+knjigaId);
            // namesti samo odredjenom korisniku da stize
            var knjiga = BibliotekaRepository.Instance.getKnjigaByID(int.Parse(knjigaId));
            await Clients.All.SendAsync("OdobrenaKnjiga",JsonConvert.SerializeObject(knjiga));
        }
    }
} 