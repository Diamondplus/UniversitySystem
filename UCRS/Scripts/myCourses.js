$(document).ready(function () {

    $(".regLink").click(function(event) {
        event.preventDefault();
        return false;
    });

    // ! Store Event after reload elements
    $('#unregCourses').on('click', '.unregLink', function () {
    //$(".unregLink").click(function(event) {
    //event.preventDefault();
        var postParams = {};
        postParams.studentId = $('#studentId').text();
        postParams.courseId = this.id;
        linkText = this.innerHTML;
        courseName = linkText.substring(linkText.indexOf('.') + 2);

        // Check course registrarion with UI / ... other way --> DB  
        var regCourseIDs = $('.regLink');
        var isCourseUnregistred = true;
        for (var i = 0; i < regCourseIDs.length; i++) {
            if (regCourseIDs[i].id == postParams.courseId) {
                isCourseUnregistred = false;
            }
        }
        if (isCourseUnregistred) {
            // load confirm modal
            document.getElementById('courseNameForConfirm').innerHTML = 'Course: ' + courseName;
            confirm("Do you really want to register this course?", function ()
            { 
                $.ajax({
                    type: "post",
                    dataType: "html",
                    url: "/Student/_AddRegCourse/",
                    data: { __RequestVerificationToken: AddAntiForgeryToken({ id: parseInt($(this).attr("title")) }), studentId: postParams.studentId, courseId: postParams.courseId },
                    success: function(response) {

                        data = JSON.parse(response);
                        var regCourseHTML = '';
                        for (var i = 0; i < data.length; i++) {
                            regCourseHTML += '<a id="' + data[i].CourseId + '" class="regLink" href="#"> ' + data[i].Name + '</a><br />';
                        }
                        document.getElementById('regCourses').innerHTML = regCourseHTML;
                    }
                });
            });
            return true;
        } else {
            // load info modal        
            document.getElementById('courseNameForInfo').innerHTML = courseName;
            $('#basic-modal-content').modal();
            return false;
        }      
    });

    function confirm(message, callback) {
        $('#confirm').modal({
            closeHTML: "<a href='#' title='Close' class='modal-close'>x</a>",
            position: ["20%", ],
            overlayId: 'confirm-overlay',
            containerId: 'confirm-container',
            onShow: function (dialog) {
                var modal = this;

                $('.message', dialog.data[0]).append(message);

                // if the user clicks "yes"
                $('.yes', dialog.data[0]).click(function () {
                    // call the callback
                    if ($.isFunction(callback)) {
                        callback.apply();
                    }
                    // close the dialog
                    modal.close(); // or $.modal.close();
                });
            }
        });
    }

    AddAntiForgeryToken = function (data) {
        data = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();
        return data;
    };


    $("#clearRegCoursesBtn").click(function (event) {
        postParams = {};
        postParams.studentId = $('#studentId').text();
        $.ajax({
            type: "post",
            dataType: "html",
            url: "/Student/_RemoveAllRegCourses/",
            data: { __RequestVerificationToken: AddAntiForgeryToken({ id: parseInt($(this).attr("title")) }), studentId: postParams.studentId },
            success: function (response) {

                data = JSON.parse(response);
                var regCourseHTML = '';
                for (var i = 0; i < data.length; i++) {
                    regCourseHTML += '<a id="' + data[i].CourseId + '" class="unregLink" href="#"> ' + data[i].Name + '</a><br />';
                }
                document.getElementById('unregCourses').innerHTML = regCourseHTML;
                document.getElementById('regCourses').innerHTML = '';
            }
        });
    });

});