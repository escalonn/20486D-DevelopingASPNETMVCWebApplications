'use strict';

$(function () {
  $('#btn-post').click(function (e) {
    if ($('#submit-form').valid()) {
      var formData = {};
      $('#submit-form').serializeArray().map(function (item) {
        item.name = item.name[0].toLowerCase() + item.name.slice(1);
        formData[item.name] = item.value;
      });
      e.preventDefault();
      $.post({
        url: 'http://localhost:54517/api/job',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(formData),
      })
        .done(function () {
          location.href = 'http://localhost:54508/JobApplication/ThankYou';
        })
        .fail(function () {
          alert('An error has occurred')
        });
    }
  });
});
