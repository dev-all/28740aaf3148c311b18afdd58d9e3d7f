var baseUrl = (document.location.origin) ? document.location.origin : window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port : '');

(function ($) {

    jQuery.isEmpty = function (obj) {
        var isEmpty = false;

        if (typeof obj == 'undefined' || obj === null || obj === '') {
            isEmpty = true;
        }

        if (typeof obj == 'number' && isNaN(obj)) {
            isEmpty = true;
        }

        if (obj instanceof Date && isNaN(Number(obj))) {
            isEmpty = true;
        }

        return isEmpty;
    }

})(jQuery);
$(function () {
    
    $('#DateExpiration').bootstrapMaterialDatePicker({
        format: 'DD/MM/YYYY',
        time: false        
    });

    
    $(".SendFile12").on("click", function (e, i) {

        var archivos_adjuntos_items = $('.item-archivo-adjunto');    
        
        if (archivos_adjuntos_items.length != 0) {



            for (var i = 0, f; f = $('input:file')[i]; ++i) {

                if (f.files.length > 0) {
                    var idInput = f.name.substr(-1);
                    var filesForm = new FormData();
                    var dataExpiration = (!jQuery.isEmpty($("#" + f.name).attr('data-expiration')) ? $("#" + f.name).attr('data-expiration') : ' ');
                    var dataNotes = (!jQuery.isEmpty($("#" + f.name).attr('data-notes')) ? $("#" + f.name).attr('data-notes') : ' ');
                    var dataIsUnique = (!jQuery.isEmpty($("#" + f.name).attr('data-isUnique')) ? $("#" + f.name).attr('data-isUnique') : ' ');
                    $("#" + f.name).prop('files') && filesForm.append('dataFile', $("#" + f.name).prop('files')[0]);
                    filesForm.append('dataExpiration', dataExpiration);
                    filesForm.append('dataNotes', dataNotes);
                    filesForm.append('dataIsUnique', dataIsUnique);  
                    filesForm.append('Id', $("#Id").val());
                    filesForm.append('IdCustomer', $("#IdCustomer").val());
                    filesForm.append('jobname', $("#JobName").val());
                    var bar = '<div class="progress" id="' + i + '">' +
                        '<div id="progress-bar-' + i + '" class="progress-bar progress-bar-striped bg-success" role="progressbar" style="width: 25%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>' +
                    '</div> </br>'                 
                    $(".progress-container").append(bar);
                    //if (f.type.match(regEx)) {
                        //Function to upload single file
                        uploadFile(filesForm, i, f);
                    //} else {
                    //    $("#progress-bar-" + i).closest(".progress")
                    //        .addClass("progress-error");
                    //    $("#progress-title-" + i).text("Invalid file format");
                    //}
                    
                }

            }
        }       
    });


    //-----------------------------------------------------------------------------------
    var html_input_start = '<input',
        html_input_end = 'type="file" value="" />';


    $('.adj-archivo').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();

        var id = $('input:file').length + 1;
        var idInputFiles = [];


        var expiration = "";
        if (!jQuery.isEmpty($("#expiration").val())) {
            expiration = $("#expiration").val();
        }
        var notes = "";
        if (!jQuery.isEmpty($("#notes").val())) {
            notes = $("#notes").val();
        }
        var isUnique = "0";
        if ($('#isUnique').is(":checked")) {
            isUnique = 1;
        }

        for (var i = 0, f; f = $('input:file')[i]; ++i) {
            if (f.files.length == 0) {
                f.remove();
            } else {
                var idInput = f.name.substr(-1);
                idInputFiles.push(idInput);
            }
        }


        $('.inputs-hiddens').append(
            html_input_start + " id='file" + id + "' name='file" + id + "' data-expiration='" + expiration + "' " + "' data-notes='" + notes + "' " + "' data-isUnique='" + isUnique + "' " + html_input_end);
        $('#file' + id).click();
    });


    $('.inputs-hiddens').on('change', 'input:file', function (e) {
        e.preventDefault();

        var file_name = $(this)[0].files[0].name;
        var file_size = $(this)[0].files[0].size;
        var file_type = $(this)[0].files[0].type;
        var file_id = $(this).prop('id');

        $('.list-file-all').append('<li class="list-group-item d-flex justify-content-between lh-condensed item-archivo-adjunto">' +
            '<div><h6 class="my-0"><font style="vertical-align: inherit;">' + file_name + '</font></h6>' +
            '<small class="text-muted"><font style="vertical-align: inherit;"><font style="vertical-align: inherit;">' + Converts(file_size) + ' . ' + file_type + '</font></font></small></div>' +
            '<span class="text-muted"><font style="vertical-align: inherit;">' +
            '<button type="button" data-id="' + file_id + '" class="pull-right btn btn-danger btn-xs delete-file"><i class="fa fa-remove"></i></button>' +
            '</font ></span > </li > '
        );


        $(".lenfile").html($(".list-file-all").children().length);
      
        if ($(".list-file-all").children().length > 0) {
            $('.cordJobFile').css("width", "45em");            
            $('.col-file').removeClass("dn").addClass("db");
            $('.Yourfiles').removeClass("invisible").addClass("visible");            
            $('.col-Job').removeClass("col-md-12").addClass("col-md-6");
            $('.col-file').removeClass("col-md-0").addClass("col-md-6");
        } else {
            $('.cordJobFile').css("width", "20em");
            $('.col-Job').removeClass("col-md-6").addClass("col-md-12");
            $('.col-file').removeClass("col-md-0").addClass("col-md-6");
            $('.col-file').removeClass("db").addClass("dn");
            $('.Yourfiles').removeClass("visible").addClass("invisible");
        }

    });
    

    $('.list-file-all').on('click', '.delete-file', function (e) {
        e.preventDefault(); e.stopPropagation();
        var file_to_remove = $(this).data('id');
        if (document.querySelectorAll('.inputs-hiddens')[0].children.length == 1) {
            $(".inputs-hiddens").empty();
        }
        $('#' + file_to_remove).remove();
        $(this).closest('.item-archivo-adjunto').remove();

        $(".lenfile").html($(".list-file-all").children().length);
        if ($(".list-file-all").children().length > 0) {
            $('.cordJobFile').css("width", "45em");
            $('.col-file').removeClass("dn").addClass("db");
            $('.Yourfiles').removeClass("invisible").addClass("visible");
            $('.col-Job').removeClass("col-md-12").addClass("col-md-6");
            $('.col-file').removeClass("col-md-0").addClass("col-md-6");
        } else {
            $('.cordJobFile').css("width", "20em");
            $('.col-Job').removeClass("col-md-6").addClass("col-md-12");
            $('.col-file').removeClass("col-md-0").addClass("col-md-6");
            $('.col-file').removeClass("db").addClass("dn");
            $('.Yourfiles').removeClass("visible").addClass("invisible");


        }
    });


});



function uploadFile(filesForm, i, file) {

   //https://developer.mozilla.org/en-US/docs/Web/API/XMLHttpRequest
    var ajax = $.ajax({
        url: baseUrl + '/Home/AjaxFile',
        type: "post",
        data: filesForm,
        processData: false,
        contentType: false,
        async:true,
        global:true,
        xhr: function (response) {
           
            var xhr = $.ajaxSettings.xhr();
            if (xhr.upload) {


                xhr.upload.addEventListener("progress", function (progress) {
                    if (progress.lengthComputable) {
                        var total = Math.round((progress.loaded / progress.total) * 100);
                        $("#progress-bar-" + i).css({ "width": total + "%" });
                        $("#progress-bar-" + i).text(file.files[0].name + ' - ' + total + "%");
                    } else {
                        $("#progress-bar-" + i).css({ "width": 0 + "%" });
                        $("#progress-bar-" + i).text(file.files[0].name + ' - Unable to get file size');
                    }
                }, false);

                xhr.addEventListener("error", function () {
                    $("#progress-bar-" + i).closest(".progress").addClass("progress-error").find("status-count").text("Error");
                    swal("Oops! An error occurred while transferring the file. check your data");
                    console.log(' addEventListener - error ');
                }, false);
                xhr.addEventListener("abort", function () {
                    $("#progress-bar-" + i).closest(".progress").fadeOut(3000, function () { $(this).remove(); });
                    swal("Oops! The transfer has been canceled by the user.");
                    console.log(' addEventListener - abort ');
                }, false);
                xhr.addEventListener("timeout", function () {
                    $("#progress-bar-" + i).closest(".progress").addClass("progress-timedout").find("status-count").text("Timed Out");
                    swal("Oops! There is an error. progress takes time");
                    console.log(' addEventListener - abort ');
                }, false);

                xhr.addEventListener("loadend", function () {
                    $("#progress-bar-" + i).closest(".progress").fadeOut(6000, function () { $(this).remove(); });
                    $.xhrPool.abortAll();
                }, false);

                xhr.addEventListener("load", function () { console.log('send ' + file.files[0].name)}, false);


         
            return xhr;

            }

        }

    });
   
}





function Converts(bytes) {
    var i = Math.floor(Math.log(bytes) / Math.log(1024)),
        sizes = ['B', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
    return (bytes / Math.pow(1024, i)).toFixed(2) * 1 + ' ' + sizes[i];
}

