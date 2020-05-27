var $date_input;
$(document).ready(function () {
    $(".request").on("click", function (event) {
        event.preventDefault();
        var service = this.getAttribute("service");
        var flag = this.getAttribute("flag");
        getForm(service, flag);
    })
})

function requestSending() {
    sendRequest();
}

function getForm(service, flag) {
    $.ajax({
        url: "Request/Create",
        type: "GET",
        dataType: "html",
        data: { service: service, flag: flag },
        success: function(data) {
            $(".request-modal-content").html(data);
            var lang = navigator.language;
            console.log(lang);
            if (lang.toLowerCase().search("fr") != -1) {
                $date_input = $(".request-date").bootstrapMaterialDatePicker({ format: 'DD MMMM YYYY - HH:mm', lang: 'fr', cancelText: 'ANNULER' });
                $date_input.on("change", function (e, date) {
                    alert(date);
                });
            } else {
                $date_input = $(".request-date").bootstrapMaterialDatePicker({ format: 'DD MMMM YYYY - HH:mm' });
            }
            $("#request-modal").modal("show");
        },
        error: function() {
            toastr.error("Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît!");
        }
    })
}


function sendRequest() {
    var data = $("#request-form").serialize();
    $.ajax({
        url: "Request/Create",
        type: "post",
        dataType: "json",
        data: data,
        success: function(data) {
            if (data["status"]) {
                sendSuccess(data["message"]);
            } else {
                toastr.error(data["message"]);
            }
        },
        error: function() {
            toastr.error("Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît!");
        }
    })
}

function sendSuccess(message) {
    $("#request-modal").modal("hide");
    toastr.success(message);
}