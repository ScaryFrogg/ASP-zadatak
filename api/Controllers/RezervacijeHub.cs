using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using api.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;

namespace api.Controllers
{
    public class RezervacijeHub : Hub
    {
        public static Dictionary<int, string> konektovani = new Dictionary<int, string>();
        public async Task NovaRezervacija(string rezervacija)
        {
            await Clients.All.SendAsync("NovaRezervacija", rezervacija);
        }
        public string IDresponse(string korisnikId)
        {
            konektovani.Add(int.Parse(korisnikId), Context.ConnectionId);
            return "ok";
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var item = konektovani.First(k => k.Value == Context.ConnectionId);
                konektovani.Remove(item.Key);
            }
            catch (InvalidOperationException)
            {
                // korisnik je administrator nije bio sacuvan u recniku
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}