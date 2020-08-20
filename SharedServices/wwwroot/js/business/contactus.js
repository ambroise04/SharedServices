function contactus() {
    $("#btnSendMessage").on("click", function (event) {
        event.preventDefault();
        var data = $("#contactForm").serialize();
        $.ajax({
            type: "POST",
            url: "/Home/Contact",
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            dataType: "json",
            data: data,
            success: function (data) {
                result = data["status"];
                if (result) {
                    $("form#contactForm").trigger("reset");
                    toastr.success(data["message"]);
                } else {
                    toastr.error(data["message"]);
                }
            },
            error: function () {
                toastr.error("Désolé, votre demande a échoué. Veuillez contacter l'administrateur.");
            }
        });
    })
}