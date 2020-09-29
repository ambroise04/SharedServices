function editEmail() {
    $.ajax({
        type: "POST",
        url: "/Enterprise/EditEmail",
        data: { email: $("#email").val() },
        dataType: "json",
        success: function (data) {
            manageResponse(data);
            activateEmail();
        },
        error: function () {
            toastr.error("Désolé, votre demande a échoué. Veuillez contacter l'administrateur.");
        }
    });
}

function editAddress() {
    $.ajax({
        type: "POST",
        url: "/Enterprise/EditAddress",
        data: { addressFR: $("#address-fr").val(), addressEN: $("#address-en").val() },
        dataType: "json",
        success: function (data) {
            manageResponse(data);
            activateAddress();
        },
        error: function () {
            toastr.error("Désolé, votre demande a échoué. Veuillez contacter l'administrateur.");
        }
    });
}

function editDesc() {
    $.ajax({
        type: "POST",
        url: "/Enterprise/EditDesc",
        data: { descFR: $("#desc-fr").val(), descEN: $("#desc-en").val()},
        dataType: "json",
        success: function (data) {
            manageResponse(data);
            activateDesc();
        },
        error: function () {
            toastr.error("Désolé, votre demande a échoué. Veuillez contacter l'administrateur.");
        }
    });
}

function editPhone() {
    $.ajax({
        type: "POST",
        url: "/Enterprise/EditPhone",
        data: { phone : $("#phone").val()},
        dataType: "json",
        success: function (data) {
            manageResponse(data);
            activatePhone();
        },
        error: function () {
            toastr.error("Désolé, votre demande a échoué. Veuillez contacter l'administrateur.");
        }
    });
}

function editPoint() {
    $.ajax({
        type: "POST",
        url: "/Enterprise/EditPoint",
        data: { point : $("#point").val()},
        dataType: "json",
        success: function (data) {
            manageResponse(data);
            activatePoint();
        },
        error: function () {
            toastr.error("Désolé, votre demande a échoué. Veuillez contacter l'administrateur.");
        }
    });
}

function editAuthorInfos() {
    $.ajax({
        type: "POST",
        url: "/Enterprise/EditAuthorInfos",
        data: { link : $("#link").val()},
        dataType: "json",
        success: function (data) {
            manageResponse(data);
            activateAuthorInfos();
        },
        error: function () {
            toastr.error("Désolé, votre demande a échoué. Veuillez contacter l'administrateur.");
        }
    });
}

function manageResponse(data) {
    if (data.status) {
        toastr.success(data["message"]);        
    } else {
        toastr.error(data["message"]);
    }
}