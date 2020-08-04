"use strict";
function acceptation(object) {
    window.event.preventDefault();
    let id = $(object).attr("data-target");
    acceptRequest(id, $(object));
}


function acceptRequest(id) {
    $.ajax({
        url: "/Request/AcceptRequest",
        type: "POST",
        data: { id: id },
        success: function (data) {
            if (data.status) {
                refreshViews(data.message);
            } else if (data.status == 403) {
                toastr.error("Cette opération ne peut être effectuée. Veuillez réessayer s'il vous plaît!");
            }else if (data.status == 404) {
                toastr.error("Données introuvables. Veuillez réessayer s'il vous plaît!");
            } else {
                toastr.error(data.message);
            }
        },
        error: function (xhr) {
            if (xhr.status == 401) {
                window.location.href = "Account/Login";
            } else {
                toastr.error("Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît!");
            }
        }
    })
}

function refreshReceived() {
    $.ajax({
        url: "/Request/RefreshReceivedView",
        type: "GET",
        dataType : "html",
        success: function (data) {
            let $container = $(".receivedCol");
            $container.html(data);
        },
        error: function (xhr) {
            if (xhr.status == 401) {
                window.location.href = "Account/Login";
            } else {
                toastr.error("Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît!");
            }
        }
    })
}

function refreshSent() {
    $.ajax({
        url: "/Request/RefreshSentView",
        type: "GET",
        dataType: "html",
        success: function (data) {
            let $container = $(".sentCol");
            $container.html(data);
        },
        error: function (xhr) {
            if (xhr.status == 401) {
                window.location.href = "Account/Login";
            } else {
                toastr.error("Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît!");
            }
        }
    })
}

function refreshViews(message) {
    toastr.success(message);
    refreshReceived();
    refreshSent();
}