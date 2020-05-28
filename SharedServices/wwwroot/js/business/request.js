﻿var $date_input;
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
            initializeDatePicker(lang);
            if (lang.toLowerCase().search("fr") != -1) {
                
            } else {
                
            }
            $("#request-modal").modal("show");
        },
        error: function() {
            toastr.error("Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît!");
        }
    })
}

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
            cancelText: 'annuler',
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