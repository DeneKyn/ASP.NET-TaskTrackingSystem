$(function () {
    $.ajaxSetup({ cache: false });
    $(".modal_show").click(function (e) {

        e.preventDefault();
        $.get(this.href, function (data) {
            $('#MyDialogContent').html(data);
            $('#myModal').modal('show');
        });
    });
})