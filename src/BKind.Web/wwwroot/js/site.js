
$(function () {

    $("#ra-profile-btn").on("click", function () {
        $(".ra-profile-menu").toggleClass("open");
    });

});

$(document).mouseup(function (e) {
    var subject = $("#ra-profile-btn");

    if (e.target.id != subject.attr('id') && !subject.has(e.target).length) {
        // subject.fadeOut();
        $(".ra-profile-menu").removeClass("open");
    }
});