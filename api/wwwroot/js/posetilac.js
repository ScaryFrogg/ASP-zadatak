"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/rezervacijeHub").build();
const container = document.querySelector("#container")
const template = document.querySelector("template")
const id = document.querySelector("#idHolder").getAttribute("data-id")

connection.on("OdobrenaKnjiga", knjigaString => {
    const knjiga = JSON.parse(knjigaString)
    const temp = template.content.cloneNode(true);
    const p = temp.querySelectorAll("p");
    p[0].textContent += knjiga.naziv;
    p[1].textContent += `${knjiga.autor.ime} ${knjiga.autor.prezime}`;
    temp.querySelector("div").setAttribute('data-id', knjiga.id)
    container.appendChild(temp);
})
connection.start().then(function () {
    connection.invoke("IDresponse", id).then(_ => {
        console.log("socket connected")
    }).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});
