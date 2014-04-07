$(document).ready(function () {

    $("#formBtn").click(function (event) {
        event.preventDefault();
        var postParams = {};
        postParams.Email = $("#Email").val();
        postParams.Password = $("#Password").val();

        $.ajax({
            type: "post",
            dataType: "html",
            url: "/Student/ValidateStudentData/",
            data: { __RequestVerificationToken: AddAntiForgeryToken({ id: parseInt($(this).attr("title")) }), Email: postParams.Email, Password: postParams.Password },
            success: function (data) {
                if ($.trim(data) == "") {
                    window.location.href = '/Home/Index';
                }
                else {
                    document.getElementById('validationResult').style.display = 'block';
                    $('#validationResult').html(data);
                }
            }
        });
    });

    AddAntiForgeryToken = function (data) {
        data = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();
        return data;
    };

    var fieldBgr = "<div class='fieldBgr'><div class='regionFieldBgr1'></div><div class='regionFieldBgr2'></div><div class='regionFieldBgr3'></div><div class='regionFieldBgr4'></div></div>";
    $('.editor-field').prepend(fieldBgr);

    document.getElementById("Email").focus();

});