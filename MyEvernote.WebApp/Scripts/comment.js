var noteId = -1;
var bodyId = "#modal_comment_body";

$('#modal_comment').on('show.bs.modal', function (e) {

    var btn = $(e.relatedTarget);
    noteId = btn.data("note-id");

    $(bodyId).load("/Comment/ShowNoteComments/" + noteId);
});

function doComment(btn, e, commentId, spanId) {

    var button = $(btn);
    var mode = button.data("edit-mode");
    var btnSpan = "";
    var txt = "";
    if (e === "edit_clicked") {
        if (!mode) {
            button.data("edit-mode", true);
            button.removeClass("btn-warning");
            button.addClass("btn-success");

            btnSpan = button.find("span");
            btnSpan.removeClass("glyphicon-edit");
            btnSpan.addClass("glyphicon-ok");

            $(spanId).addClass("editable");
            $(spanId).attr("contenteditable", true);
            $(spanId).focus();
        }
        else {
            button.data("edit-mode", false);
            button.addClass("btn-warning");
            button.removeClass("btn-success");

            btnSpan = button.find("span");
            btnSpan.addClass("glyphicon-edit");
            btnSpan.removeClass("glyphicon-ok");

            $(spanId).removeClass("editable");
            $(spanId).attr("contenteditable", false);

            txt = $(spanId).text();
            $.ajax({
                method: "POST",
                url: "/Comment/Edit/" + commentId,
                data: { text: txt }
            }).done(function (data) {

                if (data.sonuc) {
                    //Comment_partial again load
                    $(bodyId).load("/Comment/ShowNoteComments/" + noteId);  //The modal is already on and only the action is executed and the data is loaded.
                }
                else {
                    alert("Comment could not be update");
                }

            }).fail(function () {

                alert("Failed to connect to server");

            });
        }
    }
    else if (e === "delete_clicked") {
        var dialog_res = confirm("Delete comment?");
        if (!dialog_res) return false;

        $.ajax({
            method: "GET",
            url: "/Comment/Delete/" + commentId
        }).done(function (data) {

            if (data.result) {
                $(bodyId).load("/Comment/ShowNoteComments/" + noteId);
            } else {
                alert("Comment could not be deleted");
            }

        }).fail(function () {
            alert("Failed to connect to server");
        });

    }
    else if (e === "new_clicked") {

        txt = $("#new_comment_text").val();

        $.ajax({
            method: "POST",
            url: "/Comment/Create",
            data: { text: txt, "noteId": noteId }
        }).done(function (data) {

            if (data.ress) {
                $(bodyId).load("/Comment/ShowNoteComments/" + noteId);
            } else {
                alert("Comment could not be added");
            }

        }).fail(function () {
            alert("Failed to connect to server");
        });

    }
}