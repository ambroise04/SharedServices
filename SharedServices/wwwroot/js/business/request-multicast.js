
$(document).ready(function () {
    initializeDatePicker(lang);

    $('[data-toggle="tooltip"]').tooltip();

    $(".dtp-select-hour").attr("fill", "#0e9c9c");

    $('.mdb-select').materialSelect();

    $("#btn-submit").on("click", function (event) {
        event.preventDefault();
        sendRequest();
    })
})


function initializeDatePicker(lang) {
    var $dateTimePicker;
    if (lang.toLowerCase().search("fr") != -1) {
        $dateTimePicker = $('#date').bootstrapMaterialDatePicker({
            format: 'DD/MM/YYYY HH:mm',
            shortTime: false,
            minDate: moment(),
            date: true,
            time: true,
            monthPicker: false,
            year: true,
            clearButton: false,
            nowButton: false,
            switchOnClick: true,
            cancelText: 'retour',
            nowText: 'maintenant',
            lang: 'fr',
            weekStart: 1,
        });
    } else {
        $dateTimePicker = $('#date').bootstrapMaterialDatePicker({
            format: 'MM-DD-YYYY HH:mm',
            shortTime: false,
            minDate: moment(),
            date: true,
            time: true,
            monthPicker: false,
            year: true,
            clearButton: false,
            nowButton: false,
            switchOnClick: true,
            cancelText: 'Cancel',
        });
    }

    $dateTimePicker.on("change", function (event, date) {
        var val = $('#date').val();
        $("#dateSubmit").val(val);
    })
}


function sendRequest() {
    var data = $("#form-request").serialize();
    $.ajax({
        url: "Multicast",
        type: "POST",
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
    $("#form-request").trigger("reset");
    toastr.success(message);
}