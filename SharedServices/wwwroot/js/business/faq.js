$("#btnFaqQuestion").on("click", function () {
    sendquestion();
})

function sendquestion() {
    var data = $("#faq-form").serialize();
    $.ajax({
        type: "POST",
        url: "/Home/FAQs",
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        dataType: "json",
        data: data,
        success: function (data) {            
            if (data.status) {
                $("form#faq-form").trigger("reset");
                toastr.success(data["message"]);
            } else {
                toastr.error(data["message"]);
            }
        },
        error: function () {
            toastr.error("Désolé, votre demande a échoué. Veuillez contacter l'administrateur.");
        }
    });
}