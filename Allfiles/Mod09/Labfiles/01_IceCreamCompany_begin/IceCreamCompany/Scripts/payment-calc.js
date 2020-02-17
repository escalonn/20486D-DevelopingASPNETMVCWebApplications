'use strict';

$(function () {
  var iceCreamFlavors = [
    'Vanilla Ice Cream with Caramel Ripple and Almonds',
    'Vanilla Ice Cream with Cherry Dark Chocolate Ice Cream',
    'Vanilla Ice Cream with Pistachio'
  ];

  var pricePerWeight = {};
  pricePerWeight['Select'] = 0;
  pricePerWeight[iceCreamFlavors[0]] = 5;
  pricePerWeight[iceCreamFlavors[1]] = 7;
  pricePerWeight[iceCreamFlavors[2]] = 4.5;

  var hashtableImages = {};
  hashtableImages['Select'] = null;
  hashtableImages[iceCreamFlavors[0]] = 'icecream-1.jpg';
  hashtableImages[iceCreamFlavors[1]] = 'icecream-2.jpg';
  hashtableImages[iceCreamFlavors[2]] = 'icecream-3.jpg';

  UnableToPurchase();

  $('.form-control').click(function () {
    var iceCreamQuantity = parseFloat($('#quantity').val());
    var iceCreamFlavor = $('#flavor').val();
    var calc = iceCreamQuantity * pricePerWeight[iceCreamFlavor];
    var iceCreamImage = hashtableImages[iceCreamFlavor];

    if (calc && iceCreamImage) {
      $('#totalAmount').html(calc + '$');
      var src = '/images/' + iceCreamImage;
      $('#iceCreamImage').attr('src', src);
      $('#formButton').prop('disabled', false);
    } else {
      UnableToPurchase();
    }
  });

  function UnableToPurchase() {
    $('#totalAmount').empty();
    $('#iceCreamImage').attr('src', '/images/empty.jpg');
    $('#formButton').prop('disabled', true);
  }
});
