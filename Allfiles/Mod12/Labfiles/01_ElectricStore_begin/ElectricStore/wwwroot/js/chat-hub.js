$(function () {
  $('input[type="submit"]').prop('disabled', true);

  var connection = new signalR.HubConnectionBuilder().withUrl('/chatHub').build();

  connection.on('NewMessage', function (user, message) {
    var displayedMessage = user + '\'s' + ' message: ' + message;
    $('#messages-list').append('<li>' + displayedMessage + '</li>');
  });

  connection.start().catch(function (err) {
    return console.error(err.toString());
  });

  $('#send-message-form').submit(function (event) {
    event.preventDefault();
    var user = $('#input-user-name').val();
    var message = $('#input-message').val();
    connection.invoke('SendMessageAll', user, message).catch(function (err) {
      console.error(err.toString());
    });
  });

  $('input[id^="input"]').change(function () {
    var user = $('#input-user-name').val();
    var message = $('#input-message').val();
    $('input[type="submit"]').prop('disabled', !user || !message);
  });
});
