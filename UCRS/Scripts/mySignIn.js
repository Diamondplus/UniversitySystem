$(document).ready(function () {
    var fieldBgr = "<div class='fieldBgr'><div class='regionFieldBgr1'></div><div class='regionFieldBgr2'></div><div class='regionFieldBgr3'></div><div class='regionFieldBgr4'></div></div>";
    $('.editor-field').prepend(fieldBgr);

    $("li").each(function (index) {
        if ($(this).text() == "Unable to sign up - username already exists !") {
            $("#Email").addClass("input-validation-error");
        };
    });

    var isUsernameIncorrect = $('#Email').hasClass('input-validation-error');
    var isPasswordIncorrect = $('#Password').hasClass('input-validation-error');
    if (!isUsernameIncorrect && isPasswordIncorrect) {
        document.getElementById("Password").focus();
    }
    else {
        document.getElementById("Email").focus();
    }
});