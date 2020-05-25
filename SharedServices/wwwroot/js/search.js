$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();

    $.ajax({
        url: "/Search/Services",
        dataType: "json",
        type: "GET",
        success: function (data) {
            $('#search-input').mdbAutocomplete({
                data: data
            });
        },
        error: function () {

        }
    })
})

$(".search-message-a").on("click", function (event) {
    event.preventDefault();
    var id = this.getAttribute("id");
    retrieveInfos(id);
})

function retrieveInfos(target) {
    $.ajax({
        url: "Discussion/UserInfo",
        type: "GET",
        dataType: "html",
        data: { "target": target },
        success: function (data) {
            perform(data);
        },
        error: function () {
            toastr.error("Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît!");
        }
    })
}

function perform(data) {
    $(".message-content").html(data)
    $("#modal-message").modal("show");
}


function messageSending() {
    if ($("#message").val().trim() == "") {
        toastr.error("Veuillez saisir votre message avant l'envoi.");
    } else {
        sendMessage();
    }
}

function sendMessage() {
    var data = $("#form-message").serialize();
    $.ajax({
        url: "Discussion/Discuss",
        type: "post",
        dataType: "json",
        data: data,
        success: function (data) {
            if (data["status"]) {
                sendSuccess(data["message"]);
            } else {
                toastr.error(data["message"]);
            }
        },
        error: function () {
            toastr.error("Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît!");
        }
    })
}

function sendSuccess(message) {
    $("#modal-message").modal("hide");
    toastr.success(message);
}