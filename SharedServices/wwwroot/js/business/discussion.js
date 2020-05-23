$(".contact").on("click", function () {
    $(".contact, .active").removeClass("grey");
    $(".contact, .active").removeClass("lighten-3");
    $(".contact, .active").removeClass("active");

    activate($(this));
})

function activate(htmlObject) {
    htmlObject.addClass("active");
    htmlObject.addClass("grey");
    htmlObject.addClass("lighten-3");
}

$(document).ready(function () {
    activate($(".friend-list").children("li").first());

    $(".contact a").on("click", function (event) {
        //event.preventDefault();
        getDiscussion(this.getAttribute("id"));
    })
})

function getDiscussion(id) {
    $.ajax({
        type: "GET",
        url: "Discussion/Contact",
        data: { "id": id },
        dataType: "json",
        success: function (data) {
            if (data["status"]) {
                fillContent(data["discussions"]);
            } else {
                console.log(data["message"])
            }
        },
        error: function (data) {
            console.log(data);
        }
    })
}

function fillContent(data) {
    var content = "";
    var $container = $(".chat-content");
    $container.html("");

    $.each(data, function (index, message) {
        if (discussion.Emitter.Equals(currentUser.Id)) {
            content += `
                        <li class="d-flex justify-content-between mb-4">
                            <div class="chat-body white p-3 z-depth-1">
                                <div class="header">
                                    <strong class="primary-font">${message.emitteruser.firstname} ${message.emitteruser.lastname}</strong>
                                    <small class="pull-right text-muted"><i class="far fa-clock"></i> ${message.datehour}</small>
                                </div>
                                <hr class="w-100">
                                <p class="mb-0">
                                    ${message.message}
                                </p>
                            </div>
                        </li>
                    `
        }
        else {
            content += `
                        <li class="d-flex justify-content-between mb-4">
                            <img src="Image" alt="avatar" class="avatar rounded-circle mr-2 ml-lg-3 ml-0 z-depth-1">
                            <div class="chat-body white p-3 ml-2 z-depth-1">
                                <div class="header">
                                    <strong class="primary-font">${message.emitteruser.firstname} ${message.emitteruser.lastname}</strong>
                                    <small class="pull-right text-muted"><i class="far fa-clock"></i>${message.datehour}</small>
                                </div>
                                <hr class="w-100">
                                <p class="mb-0">
                                    ${message.message}
                                </p>
                            </div>
                        </li>
                    `
        }
    })
    $container.html(content);
}