"use strict"

$(".search-testi-c").on("click", function (event) {
    event.preventDefault();
    var id = this.getAttribute("id");
    retrieveInfos(id);
})

function retrieveInfos(target) {
    $.ajax({
        url: "Testimonial/UserInfos",
        type: "GET",
        dataType: "html",
        data: { "target": target },
        success: function (data) {
            perform(data);
        },
        error: function (xhr) {
            console.log(xhr.responseText)
            toastr.error("Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît!");
        }
    })
}

function perform(data) {
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
        url: "Testimonial/Rate",
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
    $("#form-testimonial").trigger("reset");
    $("#modal-testimonial").modal("hide");
    toastr.success(message);
}