$(".user-question").on("click", function (event) {
    event.preventDefault();
    getQuestion(this.getAttribute("id"));
})

$("#btnSendResponse").on("click", function () {
    sendResponse();
})

function getQuestion(id) {
    $.ajax({
        type: "GET",
        url: "/Home/UserQuestions/" + id,
        dataType: "json",
        success: function (data) {
            if (data.status) {
                conversationInfo(data["question"]);
            } else {
                toastr.error(data["message"]);
            }
        },
        error: function () {
            toastr.error("Désolé, votre demande a échoué. Veuillez contacter l'administrateur.");
        }
    });
}

function conversationInfo(data) {
    $("#conversation-box").html("");
    var content = "";
    content += `
        <li class="d-flex justify-content-between mb-4">
            <img src="${data.userPicture}" alt="avatar" class="avatar rounded-circle mr-2 ml-lg-3 ml-0">
            <div class="chat-body white p-3 ml-2">
                <div class="header">
                    <strong class="primary-font">${data.user}</strong>
                    <small class="pull-right text-muted"><i class="far fa-clock"></i> ${data.date}</small>
                </div>
                <hr class="w-100">
                <p class="mb-0">
                   ${data.message}
                </p>
            </div>
        </li>
    `
    $.each(data.responses, function (index, response) {
        content += `
            <li class="d-flex justify-content-between mb-4 white-text">
                <div class="chat-body bg-color-green p-3">
                    <div class="header">
                        <strong class="primary-font">Admin</strong>
                        <small class="pull-right text-muted"><i class="far fa-clock"></i> ${response.date}</small>
                    </div>
                    <hr class="w-100">
                    <p class="mb-0">
                        ${response.message}
                    </p>
                </div>
                <img src="/images/favicon.png" alt="avatar" class="avatar rounded-circle mr-0 ml-3">
            </li>
        `
    })
    $("#conversation-box").html(content);
    $(".question").removeProp("value");
    $(".question").prop("value", data.id);
}

function sendResponse() {
    var data = $("#form-response").serialize();
    $.ajax({
        type: "POST",
        url: "/Home/FaqAnswer",
        dataType: "json",
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        data: data,
        success: function (data) {
            if (data.status) {
                updateConversation(data["response"]);
                $("form#form-response").trigger("reset");
            } else {
                toastr.error(data["message"]);
            }
        },
        error: function () {
            toastr.error("Désolé, votre demande a échoué. Veuillez contacter l'administrateur.");
        }
    });
}

function updateConversation(response) {    
    var content = `
        <li class="d-flex justify-content-between mb-4 white-text">
            <div class="chat-body bg-color-green p-3">
                <div class="header">
                    <strong class="primary-font">Admin</strong>
                    <small class="pull-right text-muted"><i class="far fa-clock"></i> ${response.date}</small>
                </div>
                <hr class="w-100">
                <p class="mb-0">
                    ${response.message}
                </p>
            </div>
            <img src="/images/favicon.png" alt="avatar" class="avatar rounded-circle mr-0 ml-3">
        </li>
    `
    var oldContent = $("#conversation-box").html();
    $("#conversation-box").html(oldContent + content);
}