$(document).ready(function () {
    $("#filter").on("change", function () {
        var search = $(this).children("option:selected").val();
        filter(search);
    })
})
function filter(search) {
    $.ajax({
        url: "All",
        type: "GET",
        dataType: "html",
        data: { search: search },
        success: function (result) {
            
        },
        error: function (result) {
            toastr.error(result.messageText);
        }
    })
}