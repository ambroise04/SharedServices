$(document).ready(function () {
    $('.stepper').mdbStepper();

    $("#service-check").on("change", function () {
        if ($(this).is(":checked")) {
            $(".select-dropdown").attr("disabled", true);
        } else {
            $(".select-dropdown").removeAttr("disabled");
        }
    })
})

$('.mdb-select').materialSelect({
});

function someFunction22() {
    setTimeout(function () {
        $('.stepper').nextStep();
    }, 2000);
}

$(function () {
    $("#fileupload").change(function () {
        if (typeof (FileReader) != "undefined") {
            var dvPreview = $("#dvPreview");
            dvPreview.html("");
            var regex = /^([\w\s\\.\-:\(\)])+(.jpg|.jpeg|.gif|.png|.bmp)$/;
            $($(this)[0].files).each(function () {
                var file = $(this);
                if (regex.test(file[0].name.toLowerCase())) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        var img = $("#preview");
                        img.attr("src", e.target.result);
                    }
                    reader.readAsDataURL(file[0]);
                } else {
                    error(file[0].name + " n'est pas un fichier image valide. Changez les caractères avec accent si le nom du ficher en contient.");
                    dvPreview.html("");
                    return false;
                }
            });
        } else {
            alert("This browser does not support HTML5 FileReader.");
        }
    });
});