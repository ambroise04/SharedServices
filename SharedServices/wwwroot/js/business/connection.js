"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/Notify").build();
connection.on("multicast", () => {
    getNotifications();
});

connection.start().catch(function (err) {
    return console.error(err.toString());
}).then(function () {
    connection.invoke("GetConnectionId");
})