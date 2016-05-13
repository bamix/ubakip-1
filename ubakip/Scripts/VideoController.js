$("body").delegate(".video-btn", "click", function () {
    playPause($(this).parent().find("video"), $(this));
});

function playPause(myVideo, image) {
    if (myVideo.get(0).paused) {
        myVideo.get(0).play();
        image.attr("src", "../../Content/Images/pause.png");
    }
    else {
        myVideo.get(0).pause();
        image.attr("src", "../../Content/Images/play.png");
    }
}