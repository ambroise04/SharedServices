"use strict";
function removeRequest(object) {
    window.event.preventDefault();
    let id = $(object).attr("data-target");
    let source = $(object).attr("data-source");
    cancelRequest(id, source, $(object));
}


function cancelRequest(id, source, $object) {
    $.ajax({
        url: "/Request/CancelRequest",
        type: "POST",
        data: { id: id, source: source },
        success: function (data) {
            if (data.status) {
                doRefreshList(data.message, $object);
            } else {
                toastr.error(data.message);
            }
        },
        error: function (xhr) {
            if (xhr.status == 401) {
                window.location.href = "Account/Login";
            } else if(xhr.status == 403) {
                toastr.error("Cette opération ne peut être effectuée. Veuillez réessayer s'il vous plaît!");
            } else if (xhr.status == 404) {
                toastr.error("Données introuvables. Veuillez réessayer s'il vous plaît!");
            } else {
                toastr.error("Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît!");
            }
        }
    })
}

function doRefreshList(message, $object) {
    let $row = $object.closest(".row");
    let $hr = $row.prev("hr");
    $row.remove();
    $hr.remove();
    toastr.success(message);
}