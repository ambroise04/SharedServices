﻿"use strict"

$(".search-testi-c").on("click", function (event) {
    event.preventDefault();
    var id = this.getAttribute("id");
    retrieveInfos(id);
})

function retrieveInfos(target) {
    $.ajax({
        url: "/Testimonial/UserInfos",
        type: "GET",
        dataType: "html",
        data: { "target": target },
        success: function (data) {
            performUserInfos(data);
        },
        error: function (xhr) {
            if (xhr.status == 401) {
                window.location.href = "/Account/Login";
            } else {
                toastr.error("Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît!");
            }
        }
    })
}

function performUserInfos(data) {
    $(".testimonial-content").html(data)
    $("#modal-testimonial").modal("show");
    $("#rate-user").mdbRate();

    rating();
}

function rating() {
    $("#rate-user i").on("click", function () {
        $("#rate").val(parseInt($(this).attr("data-index")) + 1);
    })

    $("#rate-user i").on("mouseover", function () {
        $("#rate").val(parseInt($(this).attr("data-index")) + 1);
    })
}

function sendRating() {
    var data = $("#form-testimonial").serialize();
    $.ajax({
        url: "/Testimonial/Rate",
        type: "post",
        dataType: "json",
        data: data,
        success: function (data) {
            if (data["status"]) {
                sendSuccessRating(data["message"]);
            } else {
                toastr.error(data["message"]);
            }
        },
        error: function () {
            toastr.error("Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît!");
        }
    })
}

function sendSuccessRating(message) {
    $("#form-testimonial").trigger("reset");
    $("#modal-testimonial").modal("hide");
    toastr.success(message);
}