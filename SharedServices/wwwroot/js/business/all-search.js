var current;
$(document).ready(function () {
    $("#filter").on("change", function () {
        var search = $(this).children("option:selected").val();
        if (search == "2" && navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                current = position;
                filter(search);
            })
        } else {
            filterless(search);
        }
    })
})
function filter(search) {    
    $.ajax({
        url: "Filter",
        type: "GET",
        data: { search: search, latitude: current.coords.latitude, longitude: current.coords.longitude },
        dataType: "html",
        success: function (data) {
            $("#dyn-content").html(data);
        },
        error: function (result) {
            toastr.error(result.messageText);
        }
    })
}

function filterless(search) {
    $.ajax({
        url: "Filter",
        type: "GET",
        data: { search: search },
        dataType:"html",
        success: function (data) {
            $("#dyn-content").html(data);
        },
        error: function (result) {
            toastr.error(result.messageText);
        }
    })
}