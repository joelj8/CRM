
$(document).ready(function() {
        // Mover la tabla al espacio deseado
        //$("#divTabla").css({ top: 40, left: 50, position: 'absolute' });

            // Configurando la variable para cambiar el idioma 
            var spanish ={"sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
		        "oPaginate": {
                "sFirst": "Primero",
            "sLast": "Último",
            "sNext": "Siguiente",
            "sPrevious": "Anterior"
            },
		        "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sSortDescending": ": Activar para ordenar la columna de manera descendente"
        }};

    debugger
    //Configurando el DataTable para la Tabla HTML
    $('#GridFiltroX').DataTable({
            lengthMenu: [ [5, 15, 25, -1], [5, 15, 25, "All"] ],
            language: spanish,
            dom: 'Blfrtip',
            buttons: ['copy', 'csv', 'excel', 'pdf', 'print']
    });

    $("#idGenero").change(function () {
        cargadatagrid();
    });

    cargadatagrid();

    //Ocultando los botonos por defecto
    HideButtons();
});



function cargadatagridv2() {
    $("#GridFiltro tbody tr").remove();
    var dataparam = {
        "nombre": "joel",
        "genero": $("#idGenero").val()
    };
    $.ajax({
        "url": "Customers/CargaData",
        "data": dataparam,
        "tye": "GET",
        "datatype": "json",
        "contentType": "application/json; charset=utf-8",
        success: function (data) {
            debugger
            $("#GridFiltro tbody tr").remove();
            //$('#GridFiltro').append('<tr><td>Columna 1.1</td></tr>');

            $.each(data["data"], function (i, item) {
                $('#GridFiltro tbody').append('<tr> <td>' + item.Name + '</td> <td>' + item.Phone + '</td > <td>' + item.Mail + '</td> <td>' + item.Address + '</td> <td>' + item.GenderGrl.GenderName + '</td> </tr> ');
            });

        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });


}

    function cargadatagrid() {
    debugger
    var spanish = {
        "sProcessing": "Procesando...",
        "sLengthMenu": "Mostrar _MENU_ registros",
        "sZeroRecords": "No se encontraron resultados",
        "sEmptyTable": "Ningún dato disponible en esta tabla",
        "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
        "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
        "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
        "sInfoPostFix": "",
        "sSearch": "Buscar:",
        "sUrl": "",
        "sInfoThousands": ",",
        "sLoadingRecords": "Cargando...",
        "oPaginate": {
            "sFirst": "Primero",
            "sLast": "Último",
            "sNext": "Siguiente",
            "sPrevious": "Anterior"
        },
        "oAria": {
            "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sSortDescending": ": Activar para ordenar la columna de manera descendente"
        }
    };

    var dataparam = {
        "nombre": "joel",
        "genero": $("#idGenero").val()
        };
        var urlAjax = "@Url.Action('Customers', 'CargaData')";
        urlAjax = "./Customers/CargaData";
        var myUrl = $("#myUrl").val();
        //alert(myUrl);
        //urlAjax = '@(Url.Action("CargaData","Customers"))';

    $("#GridFiltro").DataTable({
        lengthMenu: [[ 15, 25, 50, -1], [ 15, 25, 50, "All"]],
        language: spanish,
        dom: 'Blfrtip',
        buttons: ['copy', 'csv', 'excel', 'pdf', 'print'],
        destroy: true,
        
        "ajax": {
            "processing": true,
            "url": MyAppUrlSettings.MyUsefulUrl,
            "cache": false,
            "data": dataparam,
            "tye": "GET",
            "datatype": "json"
        },
        "columnDefs": [
            {
                "targets": [5], "data": "id", "render": function (data, type, row, meta) {
                    return '<a href="\Customers/Edit/replace" class= "btn btn-xs btn-primary" ><span class="glyphicon glyphicon-pencil"></span> </a>'.replace("replace", row.ID) + ' '
                        + '<a href="\Customers/Delete/replace" class= "btn btn-xs btn-danger" ><span class="glyphicon glyphicon-trash"></span> </a>'.replace("replace", row.ID);
                }
            }
            
        ],
        "columns": [
            { "data": "Name", "autowidth": true },
            { "data": "Phone", "autowidth": true },
            { "data": "Mail", "autowidth": true },
            { "data": "Address", "autowidth": true },
            { "data": "GenderGrl.GenderName", "autowidth": true }
        ]

    });    

    HideButtons();
    
}


    function clickExcel() {
        //alert("Click Excel");
        $("#btnExcel").click();
    }

	function clickPDF(){
        //alert("Click Excel");
        $("#btnPDF").click();
    }

    function HideButtons(){
	    var buttonsAll = $("button");
	    for (var i = 0; i < buttonsAll.length; i++){
                let buttonEvalua = buttonsAll[i].className;

            // Boton de Copy
            if (buttonEvalua.includes("copy")){
                buttonsAll[i].id = "btnCopy";
               $("#btnCopy").hide();
            }

            // Boton de Excel
	        if (buttonEvalua.includes("excel")){
                buttonsAll[i].id = "btnExcel";
                $("#btnExcel").hide();
             }

            // Boton de CSV
	        if (buttonEvalua.includes("csv")){
                buttonsAll[i].id = "btnCSV";
                $("#btnCSV").hide();
            }

             // Boton de PDF
	        if (buttonEvalua.includes("pdf")){
                buttonsAll[i].id = "btnPDF";
                $("#btnPDF").hide();
            }

            // Boton de Print
	        if (buttonEvalua.includes("print")){
                buttonsAll[i].id = "btnPrint";
                $("#btnPrint").hide();
            }

        }
    }



