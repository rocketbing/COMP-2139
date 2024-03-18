
function loadComments(projectId) {
    $ajax({
        url: '/ProjectManagement/ProjectComment/GetComments?projectId=' + projectId, method: 'GET',
        success: function (data) {
            var commentsHtml = '';
            for (var i = 0; i < data.length; i++) {
                // alert(data[i]);
                commentsHtml += '<div class="comment">';
                commentsHtml += '<p>' + data[i].content + '</p>';
                commentsHtml += '<span>Posted on: ' + new Date(data[i].datePosted).toLocaledString() + '</span>';
                commentsHtml += '</div>';
            }
            $('#commentsList').html(commentsHtml);
        }
    });
}

$(document).ready(function () {
    var projectId = $('#projectComments input[name="ProjectId"]').val();
    //alert(projectId)
    //call loadComments on page load
    loadComments(projectId);

    //submit event for addCommentForm
    $('#addCommentForm').submit(function (e) {
        e.preventDefault();// this is very important
        var formData = {
            ProjectId: projectId,
            Content: $('#projectComments textarea[name="Content"]').val()
        };
        $.ajax({
            url: '/ProjectManagement/ProjectComment/AddComments',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                if (response.success) {
                    $('#projectComments textarea[name="Content"]').val('');
                    loadComments(projectId);

                } else {
                    alert(response.message);
                }
            },
            error: function (xhr, status, error) {
                alert("Error: " + error);
            }
        });
    });
});