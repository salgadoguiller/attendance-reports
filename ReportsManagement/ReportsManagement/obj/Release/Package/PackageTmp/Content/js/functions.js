// ------------------------------------------------------------------------------
// Funcion:     inicializando los datepickers y datatables.
// Parametros:  no recibe ningun parametro.
// ------------------------------------------------------------------------------
$(document).ready(function () {
    $('#divDate').datepicker({
        autoclose: true,
        format: 'mm/dd/yyyy'
    });
    $('#startDateE').datepicker({
        autoclose: true,
        format: 'mm/dd/yyyy'
    });
    $('#endDateE').datepicker({
        autoclose: true,
        format: 'mm/dd/yyyy'
    });
    $('#startDateT').datepicker({
        autoclose: true,
        format: 'mm/dd/yyyy'
    });
    $('#endDateT').datepicker({
        autoclose: true,
        format: 'mm/dd/yyyy'
    });
    $('#startDateD').datepicker({
        autoclose: true,
        format: 'mm/dd/yyyy'
    });
    $('#endDateD').datepicker({
        autoclose: true,
        format: 'mm/dd/yyyy'
    });
    $('#date-table').dataTable({
        "bPaginate": true,
        "bLengthChange": true,
        "bFilter": true,
        "bSort": true,
        "bInfo": true,
        "bAutoWidth": true,
        "iDisplayStart": 0,
        "iDisplayLength": 10,
        "responsive": true,
        "scrollX": false,
        "columnDefs": [
            { responsivePriority: 1, targets: 0 },
            { responsivePriority: 2, targets: -1 }
        ],
        "language": {
            processing: "Procesando...",
            search: "Buscar: ",
            lengthMenu: "Mostrar _MENU_ Elementos.",
            info: "Elementos del _START_ al _END_ de un total de _TOTAL_.",
            infoEmpty: "No hay datos para mostrar.",
            infoFiltered: "(Filtrado de un total de _MAX_ elementos.)",
            infoPostFix: "",
            loadingRecords: "Cargando...",
            zeroRecords: "No se encontarron registros.",
            emptyTable: "Ningún dato disponible en esta tabla.",
            paginate: {
                first: "Primero",
                previous: "Anterior",
                next: "Siguiente",
                last: "Último"
            },
            aria: {
                sortAscending: ": Acrivar para ordenar columna en forma ascendente",
                sortDescending: ": Activar para ordenar columna en forma descendente"
            }
        }
    });

    $('#date-table-2').dataTable({
        "bPaginate": true,
        "bLengthChange": true,
        "bFilter": true,
        "bSort": true,
        "bInfo": true,
        "bAutoWidth": true,
        "iDisplayStart": 0,
        "iDisplayLength": 10,
        "responsive": true,
        "scrollX": false,
        "columnDefs": [
            { responsivePriority: 1, targets: 0 },
            { responsivePriority: 2, targets: -1 }
        ],
        "language": {
            processing: "Procesando...",
            search: "Buscar: ",
            lengthMenu: "Mostrar _MENU_ Elementos.",
            info: "Elementos del _START_ al _END_ de un total de _TOTAL_.",
            infoEmpty: "No hay datos para mostrar.",
            infoFiltered: "(Filtrado de un total de _MAX_ elementos.)",
            infoPostFix: "",
            loadingRecords: "Cargando...",
            zeroRecords: "No se encontarron registros.",
            emptyTable: "Ningún dato disponible en esta tabla.",
            paginate: {
                first: "Primero",
                previous: "Anterior",
                next: "Siguiente",
                last: "Último"
            },
            aria: {
                sortAscending: ": Acrivar para ordenar columna en forma ascendente",
                sortDescending: ": Activar para ordenar columna en forma descendente"
            }
        }
    });

    $('#date-table-3').dataTable({
        "bPaginate": true,
        "bLengthChange": true,
        "bFilter": true,
        "bSort": true,
        "bInfo": true,
        "bAutoWidth": true,
        "iDisplayStart": 0,
        "iDisplayLength": 10,
        "responsive": true,
        "scrollX": false,
        "columnDefs": [
            { responsivePriority: 1, targets: 0 },
            { responsivePriority: 2, targets: -1 }
        ],
        "language": {
            processing: "Procesando...",
            search: "Buscar: ",
            lengthMenu: "Mostrar _MENU_ Elementos.",
            info: "Elementos del _START_ al _END_ de un total de _TOTAL_.",
            infoEmpty: "No hay datos para mostrar.",
            infoFiltered: "(Filtrado de un total de _MAX_ elementos.)",
            infoPostFix: "",
            loadingRecords: "Cargando...",
            zeroRecords: "No se encontarron registros.",
            emptyTable: "Ningún dato disponible en esta tabla.",
            paginate: {
                first: "Primero",
                previous: "Anterior",
                next: "Siguiente",
                last: "Último"
            },
            aria: {
                sortAscending: ": Acrivar para ordenar columna en forma ascendente",
                sortDescending: ": Activar para ordenar columna en forma descendente"
            }
        }
    });

    $('#date-table-4').dataTable({
        "bPaginate": true,
        "bLengthChange": true,
        "bFilter": true,
        "bSort": true,
        "bInfo": true,
        "bAutoWidth": true,
        "iDisplayStart": 0,
        "iDisplayLength": 10,
        "responsive": true,
        "scrollX": false,
        "columnDefs": [
            { responsivePriority: 1, targets: 0 },
            { responsivePriority: 2, targets: -1 }
        ],
        "language": {
            processing: "Procesando...",
            search: "Buscar: ",
            lengthMenu: "Mostrar _MENU_ Elementos.",
            info: "Elementos del _START_ al _END_ de un total de _TOTAL_.",
            infoEmpty: "No hay datos para mostrar.",
            infoFiltered: "(Filtrado de un total de _MAX_ elementos.)",
            infoPostFix: "",
            loadingRecords: "Cargando...",
            zeroRecords: "No se encontarron registros.",
            emptyTable: "Ningún dato disponible en esta tabla.",
            paginate: {
                first: "Primero",
                previous: "Anterior",
                next: "Siguiente",
                last: "Último"
            },
            aria: {
                sortAscending: ": Acrivar para ordenar columna en forma ascendente",
                sortDescending: ": Activar para ordenar columna en forma descendente"
            }
        }
    });

    $('#date-table-log-1').dataTable({
        "bPaginate": true,
        "bLengthChange": true,
        "bFilter": true,
        "bSort": false,
        "bInfo": true,
        "bAutoWidth": true,
        "iDisplayStart": 0,
        "iDisplayLength": 10,
        "responsive": true,
        "scrollX": false,
        "columnDefs": [
            { responsivePriority: 1, targets: 0 },
            { responsivePriority: 2, targets: -1 }
        ],
        "language": {
            processing: "Procesando...",
            search: "Buscar: ",
            lengthMenu: "Mostrar _MENU_ Elementos.",
            info: "Elementos del _START_ al _END_ de un total de _TOTAL_.",
            infoEmpty: "No hay datos para mostrar.",
            infoFiltered: "(Filtrado de un total de _MAX_ elementos.)",
            infoPostFix: "",
            loadingRecords: "Cargando...",
            zeroRecords: "No se encontarron registros.",
            emptyTable: "Ningún dato disponible en esta tabla.",
            paginate: {
                first: "Primero",
                previous: "Anterior",
                next: "Siguiente",
                last: "Último"
            },
            aria: {
                sortAscending: ": Acrivar para ordenar columna en forma ascendente",
                sortDescending: ": Activar para ordenar columna en forma descendente"
            }
        }
    });

    $('#date-table-log-2').dataTable({
        "bPaginate": true,
        "bLengthChange": true,
        "bFilter": true,
        "bSort": false,
        "bInfo": true,
        "bAutoWidth": true,
        "iDisplayStart": 0,
        "iDisplayLength": 10,
        "responsive": true,
        "scrollX": false,
        "columnDefs": [
            { responsivePriority: 1, targets: 0 },
            { responsivePriority: 2, targets: -1 }
        ],
        "language": {
            processing: "Procesando...",
            search: "Buscar: ",
            lengthMenu: "Mostrar _MENU_ Elementos.",
            info: "Elementos del _START_ al _END_ de un total de _TOTAL_.",
            infoEmpty: "No hay datos para mostrar.",
            infoFiltered: "(Filtrado de un total de _MAX_ elementos.)",
            infoPostFix: "",
            loadingRecords: "Cargando...",
            zeroRecords: "No se encontarron registros.",
            emptyTable: "Ningún dato disponible en esta tabla.",
            paginate: {
                first: "Primero",
                previous: "Anterior",
                next: "Siguiente",
                last: "Último"
            },
            aria: {
                sortAscending: ": Acrivar para ordenar columna en forma ascendente",
                sortDescending: ": Activar para ordenar columna en forma descendente"
            }
        }
    });

    $('#date-table-log-3').dataTable({
        "bPaginate": true,
        "bLengthChange": true,
        "bFilter": true,
        "bSort": false,
        "bInfo": true,
        "bAutoWidth": true,
        "iDisplayStart": 0,
        "iDisplayLength": 10,
        "responsive": true,
        "scrollX": false,
        "columnDefs": [
            { responsivePriority: 1, targets: 0 },
            { responsivePriority: 2, targets: -1 }
        ],
        "language": {
            processing: "Procesando...",
            search: "Buscar: ",
            lengthMenu: "Mostrar _MENU_ Elementos.",
            info: "Elementos del _START_ al _END_ de un total de _TOTAL_.",
            infoEmpty: "No hay datos para mostrar.",
            infoFiltered: "(Filtrado de un total de _MAX_ elementos.)",
            infoPostFix: "",
            loadingRecords: "Cargando...",
            zeroRecords: "No se encontarron registros.",
            emptyTable: "Ningún dato disponible en esta tabla.",
            paginate: {
                first: "Primero",
                previous: "Anterior",
                next: "Siguiente",
                last: "Último"
            },
            aria: {
                sortAscending: ": Acrivar para ordenar columna en forma ascendente",
                sortDescending: ": Activar para ordenar columna en forma descendente"
            }
        }
    });

    $('#date-table-simple').dataTable({
        "bPaginate": false,
        "bLengthChange": false,
        "bFilter": false,
        "bSort": false,
        "bInfo": false,
        "bAutoWidth": true,
        "iDisplayStart": 0,
        "iDisplayLength": 10,
        "responsive": true,
        "scrollX": false,
        "columnDefs": [
            { responsivePriority: 1, targets: 0 },
        ],
        "language": {
            processing: "Procesando...",
            search: "Buscar: ",
            lengthMenu: "Mostrar _MENU_ Elementos.",
            info: "Elementos del _START_ al _END_ de un total de _TOTAL_.",
            infoEmpty: "No hay datos para mostrar.",
            infoFiltered: "(Filtrado de un total de _MAX_ elementos.)",
            infoPostFix: "",
            loadingRecords: "Cargando...",
            zeroRecords: "No se encontarron registros.",
            emptyTable: "Ningún dato disponible en esta tabla.",
            paginate: {
                first: "Primero",
                previous: "Anterior",
                next: "Siguiente",
                last: "Último"
            },
            aria: {
                sortAscending: ": Acrivar para ordenar columna en forma ascendente",
                sortDescending: ": Activar para ordenar columna en forma descendente"
            }
        }
    });
});


// ------------------------------------------------------------------------------
// Funcion:     mostrar una alerta para confirmar o cancelar el envio del formulario.
// Parametros:  no recibe ningun parametro.
// ------------------------------------------------------------------------------
function confirmForm(id) {
    swal({
        title: "¿Esta seguro?",
        text: "Presione \"Confirmar\" si desea continuar.",
        type: "warning",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Confirmar",
        closeOnConfirm: false
    },
    function () {
        document.getElementById(id).submit();
        // document.update.submit();
    });
}


// ------------------------------------------------------------------------------
// Funcion:     mostrar un mensaje de error al enviar un formulario.
// Parametros:  no recibe ningun parametro.
// ------------------------------------------------------------------------------
function noChange() {
    swal("¡Error!", "No ha realizado ningún cambio.", "warning")
}


// ------------------------------------------------------------------------------
// Funcion:     ocultar el popover donde se muestran mensajes de error en los formularios.
// Parametros:
//              id  ->  identificador del elemento que contiene el popover.
// ------------------------------------------------------------------------------
function hidePopover(id) {
    $('#' + id).popover('hide');
}


// ------------------------------------------------------------------------------
// Funcion:     validar los campos de los formularios para generar reportes.
// Parametros:
//              type    ->  tipo de formulario (empleados, equipos o departamentos).
// ------------------------------------------------------------------------------
function validateFormReports(type) {
    // Validando que este seleccionado por lo menos un item (empleado, equipo o departamento) o todos.
    if (document.getElementById("listItem" + type).selectedIndex == -1 && !document.getElementById("selectAll" + type).checked) {
        $('#select' + type).popover('show');
        setTimeout(function () {
            $('#select' + type).popover('hide');
        }, 2500);
        return false;
    }

    // Validando que al menos un checkbox este seleccionado.
    if (!document.getElementById("chkEmail" + type).checked && !document.getElementById("chxNewEmail" + type).checked && !document.getElementById("chkDownload" + type).checked) {
        $('#options' + type).popover('show');
        setTimeout(function () {
            $('#options' + type).popover('hide');
        }, 2500);
        return false;
    }

    // Validando las fechas.
    var startDate = new Date(document.getElementById("startDate" + type).value);
    var endDate = new Date(document.getElementById("endDate" + type).value);
    var now = new Date();
    if (startDate > endDate) {
        $('#startDate' + type).popover('show');
        setTimeout(function () {
            $('#startDate' + type).popover('hide');
        }, 2500);
        return false;
    }
    if(startDate > now)
    {
        $('#startDate' + type).popover('show');
        setTimeout(function () {
            $('#startDate' + type).popover('hide');
        }, 2500);
        return false;
    }
}


// ------------------------------------------------------------------------------
// Funcion:     validar las credenciales del usuario que quiere realizar una accion critica (eliminar o revertir).
// Parametros:
//              message -> mensaje que se mostrara al validar las credenciales del usuario.
//              id      -> identificador del elemento (accion) que se esta validando.
// ------------------------------------------------------------------------------
function validatePassword(message, id) {
    swal({
        title: "Validando",
        text: message,
        type: "input",
        inputType: "password",
        showCancelButton: true,
        closeOnConfirm: false,
        showLoaderOnConfirm: true,
        animation: "slide-from-top",
        inputPlaceholder: "Contraseña"
    }, function (password) {
        if (password === false) return false;
        if (password === "") {
            swal.showInputError("Por favor ingrese su contraseña!");
            return false
        }
        setTimeout(function () {
            // Creando el request de la peticion a realizar al servidor.
            request = "password=" + password;

            // Creando el path de la peticion.
            path = "http://reportslno.hnsc.net/Account/passwordVerify";

            // Ejecutando la peticion AJAX.
            validatePasswordRequestAJAX(request, path, id);
        }, 2000);
    });
}


// ------------------------------------------------------------------------------
// Funcion:     realizar peticiones asincronas al servidor para validar contraseñas.
// Parametros:
//              request -> representa la peticion que se va realizar.
//              url     -> representa la direccion a la que se realizara la peticion.
//              id      -> representa el elemento del DOM que se va a modificar.
// ------------------------------------------------------------------------------
function validatePasswordRequestAJAX(request, url, id) {
    // Creando objeto XMLHttpRequest
    connect = window.XMLHttpRequest ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
    connect.onreadystatechange = function () {
        if (connect.readyState == 4 && connect.status == 200) {
            if (parseInt(connect.responseText) == 1) {
                //EXITO
                document.getElementById(id).click();
            }
            else {
                //ERROR
                validatePassword("Contraseña incorrecta! Por favor intentelo de nuevo.", id);
            }
        }
        else if (connect.readyState != 4) {
            //PROCESANDO
        }
    }
    // Haciendo la peticion.
    connect.open("POST", path, true);
    connect.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    connect.send(request);
}


// ------------------------------------------------------------------------------
// Funcion:    validar los campos del formulario de editar empleados.
// Parametros: no recibe ningun parametro.
// ------------------------------------------------------------------------------
function validateFormEmployees()
{
    // Obteniendo los valores de los input.
    var temp = document.getElementById("id");
    var idEmp = temp.value;
    var temp = document.getElementById("name");
    var name = temp.value;
    var temp = document.getElementById("email");
    var email = temp.value;
    var temp = document.getElementById("team");
    var team = temp.value;

    // Creando el request de la peticion a realizar al servidor.
    request = "idEmp=" + idEmp + "&name=" + name + "&email=" + email + "&team=" + team;

    // Creando el path de la peticion.
    path = "http://reportslno.hnsc.net/Management/employeeVerify";

    // Ejecutando la peticion AJAX.
    validateFormRequestAJAX(request, path);
}


// ------------------------------------------------------------------------------
// Funcion:    validar los campos del formulario de editar equipos.
// Parametros: no recibe ningun parametro.
// ------------------------------------------------------------------------------
function validateFormTeams()
{
    // Validando nombre del departamento
    if (document.getElementById("name").value.length == 0) {
        $('#divName').popover('show');
        setTimeout(function () {
            $('#divName').popover('hide');
        }, 2500);
        return;
    }

    // Validando opcion 'otro'
    opt = document.getElementById("inputOther");
    if (opt.checked) {
        if (document.getElementById("responsable").value != document.getElementById("oldSubName").value) {
            confirmForm('update');
            return;
        }

        if (document.getElementById("email").value != document.getElementById("oldSubEmail").value) {
            confirmForm('update');
            return;
        } else {
            //ERROR
            noChange();
        }
    }

    // Validando opcion 'employee' o 'anyone'
    opt1 = document.getElementById("inputEmployee");
    opt2 = document.getElementById("inputAnyone");
    if (opt1.checked || opt2.checked) {
        // Obteniendo los valores de los input.
        var temp = document.getElementById("idDept");
        var idDept = temp.value;
        var temp = document.getElementById("idTeam");
        var idTeam = temp.value;
        var temp = document.getElementById("name");
        var name = temp.value;
        var temp = document.getElementById("employee");
        var employee = temp.value;
        var temp = document.getElementById("empty");
        if (opt2.checked) {
            var empty = "empty";
        }
        else {
            var empty = "no-empty";
        }

        // Creando el request de la peticion a realizar al servidor.
        request = "idDept=" + idDept + "&idTeam=" + idTeam + "&name=" + name + "&employee=" + employee + "&empty=" + empty;

        // Creando el path de la peticion.
        path = "http://reportslno.hnsc.net/Management/teamVerify";

        // Ejecutando la peticion AJAX.
        validateFormRequestAJAX(request, path);
    }
}


// ------------------------------------------------------------------------------
// Funcion:    validar los campos del formulario de editar departamentos.
// Parametros: no recibe ningun parametro.
// ------------------------------------------------------------------------------
function validateFormDepartments()
{
    // Validando nombre del departamento
    if (document.getElementById("name").value.length == 0) {
        $('#divName').popover('show');
        setTimeout(function () {
            $('#divName').popover('hide');
        }, 2500);
        return;
    }



    // Validando opcion 'otro'
    opt = document.getElementById("inputOther");
    if (opt.checked) {
        if (document.getElementById("responsable").value != document.getElementById("oldResponsable").value) {
            confirmForm('update');
            return;
        }

        if (document.getElementById("email").value != document.getElementById("oldEmail").value) {
            confirmForm('update');
            return;
        } else {
            //ERROR
            noChange();
        }
    }

    // Validando opcion 'employee' o 'anyone'
    opt1 = document.getElementById("inputEmployee");
    opt2 = document.getElementById("inputAnyone");
    if (opt1.checked || opt2.checked) {
        // Obteniendo los valores de los input.
        var temp = document.getElementById("idDept");
        var idDept = temp.value;
        var temp = document.getElementById("name");
        var name = temp.value;
        var temp = document.getElementById("employee");
        var employee = temp.value;
        if (opt2.checked) {
            var empty = "empty";
        }
        else {
            var empty = "no-empty";
        }

        // Creando el request de la peticion a realizar al servidor.
        request = "idDept=" + idDept + "&name=" + name + "&employee=" + employee + "&empty=" + empty;

        // Creando el path de la peticion.
        path = "http://reportslno.hnsc.net/Management/departmentVerify";

        // Ejecutando la peticion AJAX.
        validateFormRequestAJAX(request, path);
    }
}


// ------------------------------------------------------------------------------
// Funcion:     realizar peticiones asincronas al servidor para validar formularios.
// Parametros:
//              request -> representa la peticion que se va realizar.
//              url     -> representa la direccion a la que se realizara la peticion.
// ------------------------------------------------------------------------------
function validateFormRequestAJAX(request, url)
{
    // Creando objeto XMLHttpRequest
    connect = window.XMLHttpRequest ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
    connect.onreadystatechange = function () {
        if (connect.readyState == 4 && connect.status == 200) {
            if (parseInt(connect.responseText) == 1) {
                //EXITO
                confirmForm('update');
            }
            else {
                //ERROR
                noChange();
            }
        }
        else if (connect.readyState != 4) {
            //PROCESANDO
        }
    }
    // Haciendo la peticion.
    connect.open("POST", path, true);
    connect.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    connect.send(request);
}


// ------------------------------------------------------------------------------
// Funcion:    validar que un departamento tiene subdepartamentos (equipos).
// Parametros:
//              id      ->  identificador del elemento del DOM que se presiono y que desplegara
//                          un popover con informacion de retroalimentacion para el usuario.
//              idDept      ->  identificador del departamento o subdepartamento (equipo) que se
//                              verificara,
//              idAnchor    ->  ademas identificador del elemento del DOM que se
//                              activara segun la verificacion.
// ------------------------------------------------------------------------------
function subDepartmentsVerify(id, idDept, idAnchor) {
    // Creando el request de la peticion a realizar al servidor.
    request = "idDept=" + idDept;

    // Creando el path de la peticion.
    path = "http://reportslno.hnsc.net/Management/subDepartmentsVerify";

    // Ejecutando la peticion AJAX.
    haveItemsRequestAJAX(request, path, id, idAnchor);
}


// ------------------------------------------------------------------------------
// Funcion:    validar que un departamento equipo o subequipo tiene empleados.
// Parametros:
//              id          ->  identificador del elemento del DOM que se presiono y que desplegara
//                              un popover con informacion de retroalimentacion para el usuario.
//              idDept      ->  identificador del departamento o subdepartamento (equipo) que se
//                              verificara,
//              idAnchor    ->  ademas identificador del elemento del DOM que se
//                              activara segun la verificacion.
// ------------------------------------------------------------------------------
function haveEmployeesVerify(id, idDept, idAnchor) {
    // Creando el request de la peticion a realizar al servidor.
    request = "idDept=" + idDept;

    // Creando el path de la peticion.
    path = "http://reportslno.hnsc.net/Management/haveEmployeesVerify";

    // Ejecutando la peticion AJAX.
    haveItemsRequestAJAX(request, path, id, idAnchor);
}


// ------------------------------------------------------------------------------
// Funcion:     realizar peticiones asincronas al servidor para verificar si un
//              departamento tiene subdepartamentos (equipos).
// Parametros:
//              request -> representa la peticion que se va realizar.
//              url     -> representa la direccion a la que se realizara la peticion.
//              id      -> identificador del elemento del DOM que se va a modificar.
// ------------------------------------------------------------------------------
function haveItemsRequestAJAX(request, url, id, idAnchor) {
    // Creando objeto XMLHttpRequest
    connect = window.XMLHttpRequest ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
    connect.onreadystatechange = function () {
        if (connect.readyState == 4 && connect.status == 200) {
            if (parseInt(connect.responseText) == 1) {
                //EXITO
                document.getElementById(idAnchor).click();
            }
            else {
                //ERROR
                $('#' + id).popover('show');
                setTimeout(function () {
                    $('#' + id).popover('hide');
                }, 5000);
            }
        }
        else if (connect.readyState != 4) {
            //PROCESANDO
        }
    }
    // Haciendo la peticion.
    connect.open("POST", path, true);
    connect.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    connect.send(request);
}


// ------------------------------------------------------------------------------
// Funcion:     activar evento click de un boton.
// Parametros:
//              id      -> identificador del boton en el cual se activara el evento click.
// ------------------------------------------------------------------------------
function clickBtn(id) {
    document.getElementById(id).click();
}


// ------------------------------------------------------------------------------
// Funcion:     mostrar la opcion de departamentos del nav (empleados, equipos, departamentos y reportes especiales),
//              y ocultar cualquier otra opcion que en el momento en que se ejecute la funcion
//              este visible.
// Parametros:  no recibe ningun parametro.
// ------------------------------------------------------------------------------
function showDepartments() {
    var departments = document.getElementById("departments");
    departments.style.display = "block";
    var departmentsli = document.getElementById("departmentsli");
    departmentsli.classList.add("active2");

    var employees = document.getElementById("employees");
    employees.style.display = "none";
    var employeesli = document.getElementById("employeesli");
    employeesli.classList.remove("active2");

    var teams = document.getElementById("teams");
    teams.style.display = "none";
    var teamsli = document.getElementById("teamsli");
    teamsli.classList.remove("active2");

    var specialR = document.getElementById("specialReports");
    specialR.style.display = "none";
    var specialRli = document.getElementById("specialReportsli");
    specialRli.classList.remove("active2");
}


// ------------------------------------------------------------------------------
// Funcion:     mostrar la opcion de equipos del nav (empleados, equipos, departamentos y reportes especiales),
//              y ocultar cualquier otra opcion que en el momento en que se ejecute la funcion
//              este visible.
// Parametros:  no recibe ningun parametro.
// ------------------------------------------------------------------------------
function showTeams() {
    var teams = document.getElementById("teams");
    teams.style.display = "block";
    var teamsli = document.getElementById("teamsli");
    teamsli.classList.add("active2");

    var employees = document.getElementById("employees");
    employees.style.display = "none";
    var employeesli = document.getElementById("employeesli");
    employeesli.classList.remove("active2");

    var departments = document.getElementById("departments");
    departments.style.display = "none";
    var departmentsli = document.getElementById("departmentsli");
    departmentsli.classList.remove("active2");

    var specialR = document.getElementById("specialReports");
    specialR.style.display = "none";
    var specialRli = document.getElementById("specialReportsli");
    specialRli.classList.remove("active2");
}


// ------------------------------------------------------------------------------
// Funcion:     mostrar la opcion de empleados del nav (empleados, equipos, departamentos y reportes especiales),
//              y ocultar cualquier otra opcion que en el momento en que se ejecute la funcion
//              este visible.
// Parametros:  no recibe ningun parametro.
// ------------------------------------------------------------------------------
function showEmployees() {
    var employees = document.getElementById("employees");
    employees.style.display = "block";
    var employeesli = document.getElementById("employeesli");
    employeesli.classList.add("active2");

    var teams = document.getElementById("teams");
    teams.style.display = "none";
    var teamsli = document.getElementById("teamsli");
    teamsli.classList.remove("active2");

    var departments = document.getElementById("departments");
    departments.style.display = "none";
    var departmentsli = document.getElementById("departmentsli");
    departmentsli.classList.remove("active2");

    var specialR = document.getElementById("specialReports");
    specialR.style.display = "none";
    var specialRli = document.getElementById("specialReportsli");
    specialRli.classList.remove("active2");
}


// ------------------------------------------------------------------------------
// Funcion:     mostrar la opcion de reportes especiales del nav (empleados, equipos, departamentos y reportes especiales),
//              y ocultar cualquier otra opcion que en el momento en que se ejecute la funcion
//              este visible.
// Parametros:  no recibe ningun parametro.
// ------------------------------------------------------------------------------
function showSpecialReports() {
    var specialR = document.getElementById("specialReports");
    specialR.style.display = "block";
    var specialRli = document.getElementById("specialReportsli");
    specialRli.classList.add("active2");

    var employees = document.getElementById("employees");
    employees.style.display = "none";
    var employeesli = document.getElementById("employeesli");
    employeesli.classList.remove("active2");

    var teams = document.getElementById("teams");
    teams.style.display = "none";
    var teamsli = document.getElementById("teamsli");
    teamsli.classList.remove("active2");

    var departments = document.getElementById("departments");
    departments.style.display = "none";
    var departmentsli = document.getElementById("departmentsli");
    departmentsli.classList.remove("active2");
}


// ------------------------------------------------------------------------------
// Funcion:     mostrar u ocultar input de correo electronico en formularios de generacion de reportes
//              segun las opciones seleccionadas por el usuario.
// Parametros:  no recibe ningun parametro.
// ------------------------------------------------------------------------------
function showInput(idInput, idlbl, idchx) {
    if (document.getElementById(idchx).checked) {
        var element = document.getElementById(idInput);
        element.setAttribute("type", "email");
        element.disabled = false;
        document.getElementById(idlbl).style.display = "block";
    }
    else {
        var element = document.getElementById(idInput);
        element.setAttribute("type", "hidden");
        element.disabled = true;
        document.getElementById(idlbl).style.display = "none";
    }
}


// ------------------------------------------------------------------------------
// Funcion:     mostrar u ocultar formulario de agregar empleado a equipo.
// Parametros:  no recibe ningun parametro.
// ------------------------------------------------------------------------------
function showHideForm() {
    if ($("#spanAdd").attr("itemId") == "show") {
        $("#spanAdd").attr("itemId", "hide");
        $("#spanAdd").attr("class", "glyphicon glyphicon-minus");
        $("#add").attr("class", "btn btn-danger");
    }
    else {
        $("#spanAdd").attr("itemId", "show");
        $("#spanAdd").attr("class", "glyphicon glyphicon-plus");
        $("#add").attr("class", "btn btn-success");
    }
}


// ------------------------------------------------------------------------------
// Funcion:     mostrar u ocultar input de correo electronico en formularios de generacion de reportes
//              segun las opciones seleccionadas por el usuario.
// Parametros:
//              idSelect        -> identificador del select ha habilitar o desabilitar.
//              idCheckReasign  -> identificador del checkbox ha habilitar o desabilitar.
//              idCheck         -> identificador del checkbox que habilita o desabilita los otros inputs.
// ------------------------------------------------------------------------------
function emptyResponsable(groupSelect, groupCheck, idChekReasign, idCheck) {
    if (document.getElementById(idCheck).checked) {
        var element = document.getElementById(groupSelect);
        element.style.display = "none";
        var element = document.getElementById(groupCheck);
        element.style.display = "none";
        var element = document.getElementById(idChekReasign);
        element.disabled = true;
    }
    else {
        var element = document.getElementById(groupSelect);
        element.style.display = "block";
        var element = document.getElementById(groupCheck);
        element.style.display = "block";
        var element = document.getElementById(idChekReasign);
        element.disabled = false;
    }
}


// ------------------------------------------------------------------------------
// Funcion:     validar que se hayan ingresado todos los inputs necesarios para agregar un
//              departamento, equipo o subequipo.
// Parametros:
//              idName      -> identificador del input donde se ingresa el nombre.
//              idForm      -> identificador del formulario que se esta enviando.
//              idPopover   -> identificador del elemento en donde se mostrara un mensaje
//                             si el input esta vacio.
function validateFormAdd(idName, idForm, idPopover){
    // Validando que se haya ingresado un nombre.
    if (document.getElementById(idName).value.length == 0) {
        $('#'+idPopover).popover('show');
        setTimeout(function () {
            $('#'+idPopover).popover('hide');
        }, 2500);
    }
    else {
        confirmForm(idForm);
    }
}


// ------------------------------------------------------------------------------
// Funcion:     validar que se hayan ingresado todos los inputs necesarios para agregar un
//              departamento, equipo o subequipo.
// Parametros:
//              idName      -> identificador del input donde se ingresa el nombre.
//              idForm      -> identificador del formulario que se esta enviando.
//              idPopover   -> identificador del elemento en donde se mostrara un mensaje
//                             si el input esta vacio.
function validateFormAdd(idName, idForm, idPopover) {
    // Validando que se haya ingresado un nombre.
    if (document.getElementById(idName).value.length == 0) {
        $('#' + idPopover).popover('show');
        setTimeout(function () {
            $('#' + idPopover).popover('hide');
        }, 2500);
    }
    else {
        confirmForm(idForm);
    }
}


// ------------------------------------------------------------------------------
// Funcion:     mostrar los items segun de donde se llame al metodo (empleados, departamentos, equipos, subequipos)
//              y ocultar cualquier otra opcion que en el momento en que se ejecute la funcion
//              este visible.
// Parametros:  no recibe ningun parametro.
// ------------------------------------------------------------------------------
function showItems() {
    var element = document.getElementById("items");
    element.style.display = "block";
    var element = document.getElementById("itemsli");
    element.classList.add("active2");

    var element = document.getElementById("create");
    element.style.display = "none";
    var element = document.getElementById("createli");
    element.classList.remove("active2");
}


// ------------------------------------------------------------------------------
// Funcion:     mostrar la opcion de crear items segun de donde se llame al metodo (empleados, departamentos, equipos, subequipos)
//              y ocultar cualquier otra opcion que en el momento en que se ejecute la funcion
//              este visible.
// Parametros:  no recibe ningun parametro.
// ------------------------------------------------------------------------------
function showCreate() {
    var element = document.getElementById("create");
    element.style.display = "block";
    var element = document.getElementById("createli");
    element.classList.add("active2");

    var element = document.getElementById("items");
    element.style.display = "none";
    var element = document.getElementById("itemsli");
    element.classList.remove("active2");
}


// ------------------------------------------------------------------------------
// Funcion:     validar que se hayan ingresado todos los inputs necesarios para agregar un
//              usuario al sistema.
// Parametros:
//              idName      -> identificador del input donde se ingresa el nombre.
//              idPopoverN  -> identificador del elemento en donde se mostrara un mensaje
//                             si el input esta vacio.
//              idPass      -> identificador del input donde se ingresa la contraseña
//              idPopoverP  -> identificador del elemento en donde se mostrara un mensaje
//                             si el input esta vacio.
//              idPass2     -> identificador del input donde se confirma la contraseña
//              idPopoverP2 -> identificador del elemento en donde se mostrara un mensaje
//                             si el input esta vacio.
//              idPopoverP2 -> identificador del elemento en donde se mostrara un mensaje
//                             si las contraseñas no coinciden.
//              idForm      -> identificador del formulario que se esta enviando.
function validateFormAddUser(idName, idPopoverN, idPass, idPopoverP, idPass2, idPopoverP2, idPopoverNC, idForm) {
    // Validando que se haya ingresado un nombre.
    if (document.getElementById(idName).value.length == 0) {
        $('#' + idPopoverN).popover('show');
        setTimeout(function () {
            $('#' + idPopoverN).popover('hide');
        }, 2500);
        return;
    }

    if (document.getElementById(idPass).value.length == 0) {
        $('#' + idPopoverP).popover('show');
        setTimeout(function () {
            $('#' + idPopoverP).popover('hide');
        }, 2500);
        return;
    }

    if (document.getElementById(idPass2).value.length == 0) {
        $('#' + idPopoverP2).popover('show');
        setTimeout(function () {
            $('#' + idPopoverP2).popover('hide');
        }, 2500);
        return;
    }

    if (document.getElementById(idPass).value != document.getElementById(idPass2).value) {
        document.getElementById(idPass).value = '';
        document.getElementById(idPass2).value = '';
        $('#' + idPopoverNC).popover('show');
        setTimeout(function () {
            $('#' + idPopoverNC).popover('hide');
        }, 2500);
        return;
    }

    confirmForm(idForm);
}


// ------------------------------------------------------------------------------
// Funcion:     mostrar u ocultar input tipo time segun el radio button seleccionado.
// Parametros:  no recibe ningun parametro.
function optionsAttendance() {
    opt = document.getElementById("inputIn");
    if (opt.checked) {
        var element = document.getElementById("in");
        element.style.display = "block";
        var element = document.getElementById("out" );
        element.style.display = "none";
    }

    opt = document.getElementById("inputOut");
    if (opt.checked) {
        var element = document.getElementById("out");
        element.style.display = "block";
        var element = document.getElementById("in");
        element.style.display = "none";
    }

    opt = document.getElementById("inputTwo");
    if (opt.checked) {
        var element = document.getElementById("out");
        element.style.display = "block";
        var element = document.getElementById("in");
        element.style.display = "block";
    }
}


// ------------------------------------------------------------------------------
// Funcion:     validar los campos del formulario de marcaje de empleados.
// Parametros:
//              idForm    ->  formulario que se envia.
// ------------------------------------------------------------------------------
function validateFormAttendance(idForm) {
    // Validando la fecha.
    var date = new Date(document.getElementById("date").value);
    if (date == "Invalid Date") {
        $('#divDate').popover('show');
        setTimeout(function () {
            $('#divDate').popover('hide');
        }, 2500);
        return;
    }
    else {
        // Siguientes validaciones
    }

    // Entrada
    opt = document.getElementById('inputIn');
    if (opt.checked) {
        var inHour = document.getElementById("inHour").value;
        if (inHour == "") {
            $('#divInHour').popover('show');
            setTimeout(function () {
                $('#divInHour').popover('hide');
            }, 2500);
            return;
        }
        else {
            confirmForm(idForm);
        }
    }

    // Salida
    opt = document.getElementById('inputOut');
    if (opt.checked) {
        var outHour = document.getElementById("outHour").value;
        if (outHour == "") {
            $('#divOutHour').popover('show');
            setTimeout(function () {
                $('#divOutHour').popover('hide');
            }, 2500);
            return;
        }
        else {
            confirmForm(idForm);
        }
    }

    // Ambos
    // Salida
    opt = document.getElementById('inputTwo');
    if (opt.checked) {
        var inHour = document.getElementById("inHour").value;
        var outHour = document.getElementById("outHour").value;
        if (inHour == "" || outHour == "") {
            if (inHour == "") {
                $('#divInHour').popover('show');
                setTimeout(function () {
                    $('#divInHour').popover('hide');
                }, 2500);
                return;
            }
            else {
                $('#divOutHour').popover('show');
                setTimeout(function () {
                    $('#divOutHour').popover('hide');
                }, 2500);
                return;
            }
        }
        else {
            confirmForm(idForm);
        }
    }

}


// ------------------------------------------------------------------------------
// Funcion:     mostrar u ocultar inputs de nuevo responsable segun la opcion seleccionada.
// Parametros:  no recibe ningun parametro.
function optionsEdit() {
    opt = document.getElementById("inputEmployee");
    if (opt.checked) {
        var element = document.getElementById("divEmployee");
        element.style.display = "block";
        var element = document.getElementById("divOther");
        element.style.display = "none";
        var element = document.getElementById("divAnyone");
        element.style.display = "none";
    }

    opt = document.getElementById("inputOther");
    if (opt.checked) {
        var element = document.getElementById("divOther");
        element.style.display = "block";
        var element = document.getElementById("divAnyone");
        element.style.display = "none";
        var element = document.getElementById("divEmployee");
        element.style.display = "none";

    }

    opt = document.getElementById("inputAnyone");
    if (opt.checked) {
        var element = document.getElementById("divAnyone");
        element.style.display = "block";
        var element = document.getElementById("divOther");
        element.style.display = "none";
        var element = document.getElementById("divEmployee");
        element.style.display = "none";
    }
}
