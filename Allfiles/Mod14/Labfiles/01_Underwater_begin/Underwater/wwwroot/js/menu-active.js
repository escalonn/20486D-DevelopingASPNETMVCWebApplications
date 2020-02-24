'use strict';

$(function () {
  var path = window.location.pathname;

  $("li a").each(function () {
    var href = $(this).attr('href');
    if (path === href) {
      $(this).closest('li').addClass('active');
    }
  });
});
