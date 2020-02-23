'use strict';

$(function () {
  $.getJSON('http://localhost:54517/api/RestaurantWantedAd')
    .done(function (data) {
      $.each(data, function (index, item) {
        var html = '<div class="photo-index-card-data">' +
                   '  <div class="image-wrapper">' +
                   '    <img class="photo-display-img" src="/images/white-plate.jpg">' +
                   '  </div>' +
                   '  <div class="display-picture-data">' +
                   '    <h6 class="display-title">Job title:</h6>' +
                   '    <h6>' + item.jobTitle + '</h6>' +
                   '    <h6 class="display-title">Hourly payment:</h6>' +
                   '    <h6>$' + item.pricePerHour + '</h6>' +
                   '    <h6 class="display-title">Job description:</h6>' +
                   '    <h6>' + item.jobDescription + '</h6>' +
                   '  </div>' +
                   '</div>';
        $('.container').append(html);
      });
    })
    .fail(function () {
      alert('An error has occurred');
    });
});
