$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
})

$(".add-service").click(function (event) {
    event.preventDefault();
    let id = $(this).attr("id");
    showServiceForm(id);
})

function showServiceForm(id) {
    $.ajax({
        url: "/Service/AddService",
        type: "GET",
        data: {"id": id},
        success: function (result) {
            $(".modal-add-service-content").html(result);
            $("#add-service-header").modal("show");
        },
        error: function (result) {
            toastr.error(result.messageText);
        }
    })
}

function serviceSending() {
    var data = $("#form-service").serialize();
    $.ajax({
        url: "/Service/AddService",
        type: "POST",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result.status) {
                toastr.success(result.message);
                $("#form-service").trigger("reset");
                $("#add-service-header").modal("hide");
            } else {
                toastr.error(result.message);
            }
        },
        error: function (result) {
            toastr.error(result.messageText);
        }
    })
}