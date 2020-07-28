$(window).ready(function () {
    initCounter();
});   

function setCounter(point) {
    $('.point-displayer').each(function() {
        var $this = $(this), countTo = point;
        $({
            countNum: $this.text()
        }).animate({
            countNum: countTo
        }, {
            duration: 2000,
            easing: 'swing',
            step: function() {
                $this.text(Math.floor(this.countNum));
            },
            complete: function() {
                $this.text(this.countNum);
            }
        });
    });
}

function initCounter() {
    $.ajax({
        type: "GET",
        url: "Points",
        dataType: "json",
        success: function (data) {
            if (data.status) {
                setCounter(data.points);
            } else {
                console.log("Error");
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}