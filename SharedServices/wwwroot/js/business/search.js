$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();

    $.ajax({
        url: "/Search/Services",
        dataType: "json",
        type: "GET",
        success: function (data) {
            $('#search-input').mdbAutocomplete({
                data: data
            });
        }
    })
})