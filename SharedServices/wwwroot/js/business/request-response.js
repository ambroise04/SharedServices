$(document).ready(function () {
    $(".request").on("click", function (event) {
        event.preventDefault();
        let request = this.getAttribute("request");
        postulate(request);
    })
})

function postulate(request) {
    $.ajax({
        url: "Postulate",
        type: "GET",
        data: { request: request },
        success: function (data) {
            if (data["status"]) {
                toastr.success(data["message"]);
            } else {
                toastr.warning(data["message"]);
            }
        },
        error: function (xhr) {
            if (xhr.status == 401) {
                window.location.href = "/Acount/Login";
            } else {
                toastr.error("Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît!");
            }
        }
    })
}