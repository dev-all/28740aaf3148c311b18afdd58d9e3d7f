$(function () {

    var html_input_start = '<input',
        html_input_end = 'type="file" value="" />';

    $('.adj-archivo').on('click', function (e) {
        e.preventDefault();e.stopPropagation();

       var id = $('input:file').length + 1;
    
       if (id <= 4) {
            var nombreArchivo = "";
            if (!jQuery.isEmpty($("#nombreArchivo").val())) {
                nombreArchivo = $("#nombreArchivo").val();
            }

            var carpeta = "1";
            if (!jQuery.isEmpty($("#idServicioPorEspecialidad").val())) {
                carpeta = $("#idServicioPorEspecialidad").val();
            }

            var idInputFiles = [];
            for (var i = 0, f; f = $('input:file')[i]; ++i) {               
                if (f.files.length == 0) {
                    f.remove();                   
                } else {
                    var idInput = f.name.substr(-1);                  
                    idInputFiles.push(idInput);                
                }                                                 
            }        
            for (var x = 1 ; x < 5; x++) {
                if (idInputFiles.indexOf(x.toString()) == -1) {
                    id = x;                                      
                    break;
                }              
            }          
            $('.inputs-hiddens').append(html_input_start + " id='file" + id + "' name='file" + id + "' " + "' data='" + nombreArchivo + "' " + "' data-carpeta='" + carpeta + "' " + html_input_end);
            $('#file' + id).click();
       
        }
        else {
            toastr.error('No se puede adjuntar mas de 4 archivos');
        }
    });


    $('.inputs-hiddens').on('change', 'input:file', function (e) {
        e.preventDefault();
        var file_name = $(this)[0].files[0].name
       
        if (!jQuery.isEmpty($("#nombreArchivo").val())) {
            file_name = $("#nombreArchivo").val();
        } // else {  file_name = }

        var file_id = $(this).prop('id');
        
        $('.body-archivos-adj').append('<div class="item-archivo-adjunto">' +  file_name +
                                       '<button type="button" data-id="' + file_id +
                                       '" class="pull-right btn btn-danger btn-xs delete-file"><i class="fa fa-remove"></i></button>' +
                                       '</div>');

        $("#nombreArchivo").val('');
    });

    $('.body-archivos-adj').on('click', '.delete-file', function (e) {
        e.preventDefault(); e.stopPropagation();
        var file_to_remove = $(this).data('id');

        //console.log(file_to_remove);
        //console.log(document.querySelectorAll('.inputs-hiddens')[0].children.length);
        if (document.querySelectorAll('.inputs-hiddens')[0].children.length == 1)
        {
            $(".inputs-hiddens").empty();
       
        }

        $('#' + file_to_remove).remove();      
        $(this).closest('.item-archivo-adjunto').remove();
    });



    //$('#file1').on('change', function (event) {
    //    debugger;
    //    var target = event.target || event.srcElement;
    //    console.log(target, "changed.");
    //    console.log(event);
    //    if (target.value.length == 0) {
    //        console.log("Suspect Cancel was hit, no files selected.");
    //        if (numFiles == target.files.length) {
    //            cancelButton.onclick();
    //        }
    //    } else {
    //        console.log("File selected: ", target.value);
    //        numFiles = target.files.length;
    //    }
    //});
    //$('#file1').onblur = function (event) {
    //    debugger;
    //    var target = event.target || event.srcElement;
    //    console.log(target, "changed.");
    //    console.log(event);
    //    if (target.value.length == 0) {
    //        console.log("Suspect Cancel was hit, no files selected.");
    //        if (numFiles == target.files.length) {
    //            cancelButton.onclick();
    //        }
    //    } else {
    //        console.log("File selected: ", target.value);
    //        numFiles = target.files.length;
    //    }
    //}
    //function evalSelectFile(imputfile) {
    //       debugger;
    //    if (imputfile.files.length == 0) {
    //        console.log("Suspect Cancel was hit, no files selected.");
    //        cancelButton.onclick();
    //    } else {
    //        console.log("File selected: ", imputfile.files.length);
    //    }
    //};
    /*
    inputElement.onclick = function(event) {
      var target = event.target || event.srcElement;
      console.log(target, "clicked.");
      console.log(event);
      if (target.value.length == 0) {
        console.log("Suspect Cancel was hit, no files selected.");
        cancelButton.onclick();
      } else {
        console.log("File selected: ", target.value);
        numFiles = target.files.length;
      }
    }
    
    inputElement.onchange = function(event) {
      var target = event.target || event.srcElement;
      console.log(target, "changed.");
      console.log(event);
      if (target.value.length == 0) {
        console.log("Suspect Cancel was hit, no files selected.");
        if (numFiles == target.files.length) {
          cancelButton.onclick();
        }
      } else {
        console.log("File selected: ", target.value);
        numFiles = target.files.length;
      }
    }
        
    inputElement.onblur = function(event) {
      var target = event.target || event.srcElement;
      console.log(target, "changed.");
      console.log(event);
      if (target.value.length == 0) {
        console.log("Suspect Cancel was hit, no files selected.");
        if (numFiles == target.files.length) {
          cancelButton.onclick();
        }
      } else {
        console.log("File selected: ", target.value);
        numFiles = target.files.length;
      }
    }
    */





});