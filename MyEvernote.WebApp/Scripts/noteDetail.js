$(function () {

    $('#modal_noteDetail').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget);
        noteId = btn.data("note-id");

        $("#modal_noteDetail_body").load("/Note/GetNoteText/" + noteId);
    });

});