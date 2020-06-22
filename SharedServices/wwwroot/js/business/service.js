$(document).ready(function () {
    $("#btn-add-service").click(function (event) {
        event.preventDefault();
        addEvent();
    })
})

$('.mdb-select').materialSelect({
});

function addEvent() {
    var data = $("#form-add-service").serialize();
    $.ajax({
        url: "MyServices",
        type: "POST",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result["status"]) {
                toastr.success(result["message"]);
            } else {
                toastr.error(result["message"]);
            }
        },
        error: function (result) {
            toastr.error(result.messageText);
        }
    })
}