"use strict";

function updateNotification(notification) {
    if (notification == 0) {
        $(".notifications__nav").removeClass("visible");
    } else {
        $(".nav-item__badge").addClass("visible");
        $(".notifications__nav-count").html(notification);
    }
}

function getNotifications() {
    $.ajax({
        type: "GET",
        url: "/Notification/CheckMulticastNotifications",
        dataType: "json",
        success: function (data) {
            if (data.status) {
                updateNotification(data.count);
            }
        },
        error: function (xhr) {
            console.error(xhr.responseText);
        }
    })
}

$(document).ready(function() {
    getNotifications();
})