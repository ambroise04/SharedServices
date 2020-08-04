"use strict";
function validation(object) {
    window.event.preventDefault();
    let id = $(object).attr("data-target");
    validateRequest(id);
}


function validateRequest(id) {
    $.ajax({
        url: "/Request/ValidateRequest",
        type: "POST",
        data: { id: id },
        success: function (data) {
            if (data.status) {
                refreshViews(data.message); //Defined in manage-requests-acceptance.js
            } else {
                toastr.error(data.message);
            }
        },
        error: function (xhr) {
            if (xhr.status == 401) {
                window.location.href = "Account/Login";
            } else if (xhr.status == 403) {
                toastr.error("Cette opération ne peut être effectuée. Veuillez réessayer s'il vous plaît!");
            } else if (xhr.status == 404) {
                toastr.error("Données introuvables. Veuillez réessayer s'il vous plaît!");
            } else {
                toastr.error("Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît!");
            }
        }
    })
}