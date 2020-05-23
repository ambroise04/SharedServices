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
        getDiscussion(this.getAttribute("id"), this.getAttribute("current"));
    })
})

function getDiscussion(id, current) {
    $.ajax({
        type: "GET",
        url: "Discussion/Contact",
        data: { "id": id },
        dataType: "json",
        success: function (data) {
            if (data["status"]) {
                fillContent(data["discussions"], current);
            } else {
                console.log(data["message"])
            }
        },
        error: function (data) {
            console.log(data);
        }
    })
}

function fillContent(data, current) {
    var content = "";
    var $container = $(".chat-content");
    $container.html("");

    $.each(data, function (index, message) {
        if (message.emitter == current) {
            content += `
                        <li class="d-flex justify-content-end mb-4">
                            <div class="chat-body white p-3 z-depth-1 rounded">
                                <div class="header">
                                    <strong class="primary-font">${message.emitterName}</strong>
                                    <small class="pull-right text-muted"><i class="far fa-clock"></i> ${message.date}</small>
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
                        <li class="d-flex justify-content-start mb-4">
                            <div class="chat-body white p-3 ml-2 z-depth-1 rounded">
                                <div class="header">
                                    <strong class="primary-font">${message.emitterName}</strong>
                                    <small class="pull-right text-muted"><i class="far fa-clock"></i>${message.date}</small>
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