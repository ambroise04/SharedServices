$(".add-group").click(function (event) {
    event.preventDefault();
    showGroupForm();
})

function showGroupForm() {
    $.ajax({
        url: "/Service/AddCategory",
        type: "GET",
        success: function (result) {
            $(".modal-add-group-content").html(result);
            $("#add-group-header").modal("show");
        },
        error: function (result) {
            toastr.error(result.messageText);
        }
    })
}

function groupSending() {
    var data = $("#form-group").serialize();
    $.ajax({
        url: "/Service/AddCategory",
        type: "POST",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result.status) {
                toastr.success(result.message);
                $("#form-group").trigger("reset");
                $("#add-group-header").modal("hide");
            } else {
                toastr.error(result.message);
            }
        },
        error: function (result) {
            toastr.error(result.messageText);
        }
    })
}