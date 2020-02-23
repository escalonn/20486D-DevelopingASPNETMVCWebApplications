$(function () {
    var path = window.location.pathname;

    $("li a").each(function (value) {
        var href = $(value).attr('href');
        if (path === href) {
            $(this).closest('li').addClass('active');
        }
    });
});
