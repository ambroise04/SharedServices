$.fn.hasAttr = function (name) {
    return this.attr(name) !== undefined;
};

function activateEmail() {
    let $input = $("#email");
    if ($input.hasAttr("disabled")) {
        $input.removeAttr("disabled");
    } else {
        $input.attr("disabled", "disabled");
    }
}

function activateAddress() {
    let $input_fr = $("#address-fr");
    let $input_en = $("#address-en");
    if ($input_fr.hasAttr("disabled")) {
        $input_fr.removeAttr("disabled");
        $input_en.removeAttr("disabled");
    } else {
        $input_fr.attr("disabled", "disabled");
        $input_en.attr("disabled", "disabled");
    }
}

function activateDesc() {
    let $input_fr = $("#desc-fr");
    let $input_en = $("#desc-en");
    if ($input_fr.hasAttr("disabled")) {
        $input_fr.removeAttr("disabled");
        $input_en.removeAttr("disabled");
    } else {
        $input_fr.attr("disabled", "disabled");
        $input_en.attr("disabled", "disabled");
    }
}

function activatePhone() {
    let $input = $("#phone");
    if ($input.hasAttr("disabled")) {
        $input.removeAttr("disabled");
    } else {
        $input.attr("disabled", "disabled");
    }
}

function activatePoint() {
    let $input = $("#point");
    if ($input.hasAttr("disabled")) {
        $input.removeAttr("disabled");
    } else {
        $input.attr("disabled", "disabled");
    }
}

function activateAuthorInfos() {
    let $input = $("#link");
    if ($input.hasAttr("disabled")) {
        $input.removeAttr("disabled");
    } else {
        $input.attr("disabled", "disabled");
    }
}