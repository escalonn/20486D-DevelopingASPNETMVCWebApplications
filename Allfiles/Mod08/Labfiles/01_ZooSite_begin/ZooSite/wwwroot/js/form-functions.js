'use strict';

function calculateSum() {
  var rows = document.querySelectorAll('#totalAmount tr .sum');
  var sum = 0;
  for (var i = 0; i < rows.length; i++) {
    sum += parseFloat(parseFloat(rows[i].innerHTML.substring(1, rows[i].innerHTML.length)).toFixed(2));
  }
  document.getElementById('sum').innerHTML = 'Total: $' + sum;
}
