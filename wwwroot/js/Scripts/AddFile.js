$(function () {

    var html_input_start = '<input',
        html_input_end = 'type="file" value="" />';

    $('.adj-archivo').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();

        var id = $('input:file').length + 1;
        var idInputFiles = [];

        debugger;

        for (var i = 0, f; f = $('input:file')[i]; ++i) {
            if (f.files.length == 0) {
                f.remove();
            } else {
                var idInput = f.name.substr(-1);
                idInputFiles.push(idInput);
            }
        }
        for (var x = 1; x < 15; x++) {
            if (idInputFiles.indexOf(x.toString()) == -1) {
                id = x;
                break;
            }
        }

        $('.inputs-hiddens').append(
            html_input_start + " id='file" + id + "' name='file" + id + "' " + "' data='" + nombreArchivo + "' " + "' data-carpeta='" + carpeta + "' " + html_input_end);
        $('#file' + id).click();
    });




    $('.inputs-hiddens').on('change', 'input:file', function (e) {
        e.preventDefault();

        debugger;

        var file_name = $(this)[0].files[0].name

        var file_id = $(this).prop('id');

        $('.body-archivos-adj').append('<div class="item-archivo-adjunto">' + file_name +
            '<button type="button" data-id="' + file_id + '" class="pull-right btn btn-danger btn-xs delete-file"><i class="fa fa-remove"></i></button>' +
            '</div>');
    });


    $('.body-archivos-adj').on('click', '.delete-file', function (e) {
        e.preventDefault(); e.stopPropagation();
        var file_to_remove = $(this).data('id');
        if (document.querySelectorAll('.inputs-hiddens')[0].children.length == 1) {
            $(".inputs-hiddens").empty();
        }
        $('#' + file_to_remove).remove();
        $(this).closest('.item-archivo-adjunto').remove();
    });


});

