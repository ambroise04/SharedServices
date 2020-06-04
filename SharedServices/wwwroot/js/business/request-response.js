
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
        dataType: "json",
        data: { request: request },
        success: function(data) {
            if (data["status"]) {
                toastr.success(data["message"]);
            } else {
                toastr.warning(data["message"]);
            }
        },
        error: function() {
            toastr.error("Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît!");
        }
    })
}