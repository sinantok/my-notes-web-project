//when page load
$(function () {

    //show likes 
    var noteidArray = [];
    $("div[data-note-id]").each(function (i, e) {
        noteidArray.push($(e).data("note-id"));
    });

    $.ajax({
        method: "POST",
        url: "/Note/GetLiked",
        data: { ids: noteidArray }
    }).done(function (data) {

        if (data.ress !== null && data.ress.length > 0) {

            for (var i = 0; i < data.ress.length; i++) {

                var id = data.ress[i];
                var likedNoteDiv = $("div[data-note-id=" + id + "]");
                var btnn = likedNoteDiv.find("button[data-liked]");
                var spann = btnn.find("span.like-heart");

                btnn.data("liked", true);
                spann.removeClass("glyphicon-heart-empty");
                spann.addClass("glyphicon-heart");
            }
        }

    }).fail(function () {

    });

    //----

    //SetLikes
    $("button[data-liked]").click(function () {
        var btnn = $(this);
        var likedBool = btnn.data("liked");
        var noteId = btnn.data("note-id");
        var heartSpan = btnn.find("span.like-heart");
        var countSpan = btnn.find("span.like-count");

        $.ajax({
            method: "POST",
            url: "/Note/SetLikes",
            data: { "noteId": noteId, "liked": !likedBool }
        }).done(function (data) {

            if (data.hasErrorr === true) {
                alert(data.errorMessagee);
            }
            else if (data.hasErrorr === false) {

                likedBool = !likedBool;
                btnn.data("liked", likedBool);
                countSpan.text(data.ress);

                heartSpan.removeClass("glyphicon-heart-empty");
                heartSpan.removeClass("glyphicon-heart");

                if (likedBool) {
                    heartSpan.addClass("glyphicon-heart");
                }
                else {
                    heartSpan.addClass("glyphicon-heart-empty");
                }
            }

        }).fail(function () {
            alert("Nottttt");
        });
    });

});