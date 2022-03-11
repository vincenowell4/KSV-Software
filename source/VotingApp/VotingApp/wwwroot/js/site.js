// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function FillValues(voteId) {
    $("#created-vote-id").val(voteId);
    $("#vote-title").val( $("#Title-" + voteId).html() );
    $("#vote-description").val( $("#Desc-" + voteId).html() );
}

function SaveEditedVote() {
    //escape any apostrophes (turn ' into \\' ) in the title and description, so there won't be an issue on the server side with JSON conversion
    let voteTitle = $('#vote-title').val().replace(/'/g, "\\\'");
    let voteDesc = $('#vote-description').val().replace(/'/g, "\\\'");
    let voteData = { voteData: "{ 'voteId': " + $('#created-vote-id').val() + ", 'voteTitle': '" + voteTitle + "', 'voteDesc': '" + voteDesc + "'}" };

    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/create/CreatedVotesReview",
        data: voteData,
        success: populateVoteResultsDiv,
        error: function (jqXHR, exception) {
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Not connect.\n Verify Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }
        }
    });

    //reset the edit dialog box values
    $("#created-vote-id").val(0);
    $("#vote-title").val('');
    $("#vote-description").val('');
}

function populateVoteResultsDiv(data) {
    var jsonStr = JSON.stringify(data);
    const obj = JSON.parse(jsonStr);

    if (obj.title != null && obj.title.length > 0 && obj.desc != null && obj.desc.length > 0) {
        $("#Title-" + obj.id).html(obj.title);
        $("#Desc-" + obj.id).html(obj.desc);
    }
    else {
        console.log("ERROR repository data is null");
        alert("Error getting repository data from server");
    }
}

function CheckFieldsValid() {
    if (document.getElementById("vote-title").value == '') {
        alert("Title field is required");
        return false;
    }
    if (document.getElementById("vote-description").value == '') {
        alert("Description field is required");
        return false;
    }
    $("#btnClose").click();
    SaveEditedVote();
    return true;
}
