
$(function () {

    $("#ra-profile-btn").on("click touch", function () {
        $(".ra-profile-menu").toggleClass("open");
    });

    $("#ra-menu-toggle").on("click touch", function () {
        $(".ra-context-menu").toggleClass("open");
    });

    $("#ra-menu-toggle").on("click touch", function () {
        $(this).toggleClass("open");
    });

});

$(document).mouseup(function (e) {
    var itemLg = $("#ra-profile-btn");
    var itemSm = $("#ra-menu-toggle");

    if (e.target.id != itemLg.attr('id') && !itemLg.has(e.target).length) {
        $(".ra-profile-menu").removeClass("open");
    }

    if (e.target.id != itemSm.attr('id') && !itemSm.has(e.target).length) {
        $(".ra-context-menu").removeClass("open");
    }
});