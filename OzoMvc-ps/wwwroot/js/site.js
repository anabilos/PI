$(function () {
    $(document).on('click', '.delete', function (event) {
      if (!confirm("Obrisati zapis?")) {
        event.preventDefault();
      }
    });
  });

  function clearOldMessage() {
    $("#tempmessage").siblings().remove();
    $("#tempmessage").removeClass("alert-success");
    $("#tempmessage").removeClass("alert-danger");
    $("#tempmessage").html('');
  }
  