"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/rezervacijeHub").build();
const container = document.querySelector("#container")
const template = document.querySelector("template")

connection.on("NovaRezervacija", rezervacija => {
    const rez = JSON.parse(rezervacija)
    console.log("Nova rezervacija",rez)
    const temp = template.content.cloneNode(true);
    const p = temp.querySelectorAll("p");
    p[0].textContent += rez.knjigaId;
    p[1].textContent += rez.korisnikId;

    temp.querySelector("button").addEventListener("click",()=>{
        $.ajax({
            url: '/Korisnik/odobriRezervaciju',
            type: "post",
            contentType: 'application/x-www-form-urlencoded',
            data: {
                rezString: rezervacija
            },
            success: function (result) {
                console.log(result);
                connection.invoke("OdobrenaKnjiga", result)
                .then(_=>temp.querySelector("div").outterHTML = "")
                .catch(function (err) {
                    return console.error(err.toString());
                });
            }
        });
   
    })

    container.appendChild(temp);
});
connection.on("OdobrenaKnjiga", knjiga => {
    const k = JSON.parse(knjiga)
    console.log("Nova knjiga",k)
    const temp = template.content.cloneNode(true);
    const p = temp.querySelectorAll("p");
    p[0].textContent += k.naziv;
    p[1].textContent += k.autor.ime;

    container.appendChild(temp);
})

connection.start().then(function () {
    console.log("socket connected")
}).catch(function (err) {
    return console.error(err.toString());
});

// document.getElementById("sendButton").addEventListener("click", function (event) {
//     var user = document.getElementById("userInput").value;
//     var message = document.getElementById("messageInput").value;
//     connection.invoke("SendMessage", user, message).catch(function (err) {
//         return console.error(err.toString());
//     });
//     event.preventDefault();
// });