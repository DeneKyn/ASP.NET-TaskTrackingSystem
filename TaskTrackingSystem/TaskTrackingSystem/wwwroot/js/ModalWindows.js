window.onload = () => {
    $(function () {
        $.ajaxSetup({ cache: false });
        $(".modal_show").on("click", function (e) {
            e.preventDefault();
            $('#myModalContent').load(this.href, function () {
                $('#myModal').modal({
                    keyboard: true
                }, 'show');
                bindForm(this);
            });
            return false;
        });
    });
};

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    location.reload();
                    console.log('Привет от Кекса!');
                } else {
                    $('#myModalContent').html(result);
                    bindForm();
                    console.log('лолкек');
                }
            }
        });
        return false;
    });
}