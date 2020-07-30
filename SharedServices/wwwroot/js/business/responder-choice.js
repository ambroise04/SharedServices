$(document).ready(function () {
    $(".btn-choose").on("click", function (event) {
        event.preventDefault();
        let request = this.getAttribute("request");
        let target = this.getAttribute("target");
        choose(request, target);
    })
})

function choose(request, target) {
    $.ajax({
        url: "Choose",
        type: "POST",
        dataType: "json",
        data: { request: request, target: target },
        success: function(data) {
            if (data["status"]) {
                toastr.success(data["message"]);
            } else {
                toastr.error(data["message"]);
            }
        },
        error: function() {
            toastr.error("Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît!");
        }
    })
}