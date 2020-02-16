'use strict';

$(function () {
  $('.pricing select').change(function (event) {
    var target = $(event.target);
    var value = parseInt(target.val());
    var container = target.parent();
    var price = container.prev();
    var label = price.prev();
    $('#' + label.text()).remove();

    if (value) {
      $('#summary').addClass('display-div').removeClass('hidden-div');
      var correctCost = price.text().substring(1);
      var calc = parseInt(value * correctCost);
      var msg = label.text() + ' ticket - ' + value + 'x' + price.text() + ' = <span class="sum">' + '$' + calc + '</span>';
      var row = $('<tr id="' + label.text() + '">');
      row.append($('<td>').html(msg));
      $('#totalAmount').append(row);
    }

    if ($('#totalAmount tr').length === 0) {
      $('#summary').addClass('hidden-div').removeClass('display-div');
      $('#formButtons input').prop('disabled', true);
      $('#comment').show();
    } else {
      $('#formButtons input').prop('disabled', false);
      $('#comment').hide();
    }

    calculateSum();
  });

  function calculateSum() {
    var sum = 0;
    $('#totalAmount tr .sum').each(function () {
      sum += +(+$(this).text().substring(1)).toFixed(2);
    });
    $('#sum').html('Total: $' + sum);
  }
});
