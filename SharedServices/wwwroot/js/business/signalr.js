"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/params").build();
$(document).ready(function () {
    connection.start().then(function () {
        connection.invoke("LogParams")
    });
    connection.on("NoService", function () {
        toastr.warning("")
    })
})