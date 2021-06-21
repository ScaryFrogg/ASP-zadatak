"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/rezervacijeHub").build();
const container = document.querySelector("#container")
const template = document.querySelector("template")

connection.on("NovaRezervacija", rezervacija => {
    const rez = JSON.parse(rezervacija)
    const temp = template.content.cloneNode(true);
    const p = temp.querySelectorAll("p");
    p[0].textContent += rez.knjiga.naziv;
    p[1].textContent += rez.korisnik.username;
    temp.querySelector("div").setAttribute('data-id', rez.id)
    container.appendChild(temp);
});
connection.start().then(function () {
    console.log("socket connected")
}).catch(function (err) {
    return console.error(err.toString());
});
