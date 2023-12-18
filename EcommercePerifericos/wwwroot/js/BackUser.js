
$(function ($) {

    /*funcion que abre el modal con producto cargado o vacio dependienco si recibe un json*/
    function abrirModal(json) {
        $("#txtid").val(0);
        $("#txtnombre").val("");
        $("#txtapellido").val("");
        $("#txtemail").val("");
        $("#txtcontrasenia").val("");
        $("#txtrol").val("");

        if (json !== null) {
            $("#txtid").val(json.id);
            $("#txtnombre").val(json.nombre);
            $("#txtapellido").val(json.apellido);
            $("#txtemail").val(json.email);
            $("#txtcontrasenia").val(json.contrasenia);
            $("#txtrol").val(json.idRol);
        }

        $("#FormModal").modal("show");
        console.log("Valor de #txtid:", $("#txtid").val());
    }

    /*evento cuando se le da al boton crear*/
    $("#btnCrearNuevo").on("click", function () {
        abrirModal(null);
    });

    /*Setea datos de la fila de datatable*/
    var tabla = $('#tabla').DataTable({
        responsive: true,
        ordering: false,
        ajax: {
            url: '/BackUser/UserList',
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data'
        },
        columns: [
            { data: 'id' },           
            { data: 'nombre' },
            { data: 'apellido' },
            { data: 'email' },
            { data: 'contrasenia' },
            { data: 'rol' },
            {
                defaultContent: '<div style="display: flex;">' +
                    '<button type="button" class="btn btn-primary btn-sm btn-editar"><i class="bi bi-pencil"></i></button>' +
                    '<button type="button" class="btn btn-danger btn-sm ms-2 btn-eliminar"><i class="bi bi-trash3"></i></button>' +
                    '</div>',
                orderable: false,
                searchable: false,
                width: "60px"
            },
            { data: 'idRol', visible: false }
        ],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json'
        }
    });
   
    // Evento de clic para el botón de editar
    $("#tabla tbody").on("click", ".btn-editar", function () {
        var filaSeleccionada = $(this).closest("tr");
        var data = tabla.row(filaSeleccionada).data();
        abrirModal(data);
    });

    // Mostrar mensaje toast
    function showToast(message, type) {
        var toastContainer = $("#toastContainer");
        var toastClass = "";

        switch (type) {
            case "success":
                toastClass = "bg-success text-white";
                break;
            case "error":
                toastClass = "bg-danger text-white";
                break;
        }

        var toast = `
                    <div class="toast show ${toastClass}" role="alert" aria-live="assertive" aria-atomic="true">
                        <div class="toast-body">
                            ${message}                           
                        </div>
                    </div>
                `;

        toastContainer.append(toast);

        $(".toast").toast("show");
        $(".toast").on("hidden.bs.toast", function () {
            $(this).remove();
        });
    }

    /*Evento Crear y Modificar Producto*/
    $(document).on("click", "#btnGuardarCambios", function () {
        var formData = new FormData($("#usuarioForm")[0]);

        if ($("#txtid").val() == 0) {
            $.ajax({
                url: "/BackUser/CrearUsuario",
                type: "POST",
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (validar()) {
                        showToast("Usuario creado exitosamente.", "success");
                        tabla.ajax.reload();
                        $("#FormModal").modal("hide");
                    } else {
                        // Mostrar mensajes de alerta
                        showToast(response.message, "error");
                    }
                },
                error: function (error) {
                    showToast("Ha ocurrido un error al crear usuario.", "error");
                    console.error(error);
                }
            });
        } else {
            formData.append("id", $("#txtid").val());
            $.ajax({
                url: "/BackUser/ModificarUsuario",
                type: "POST",
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (validar()) {
                        showToast("Usuario modificado exitosamente.", "success");
                        tabla.ajax.reload();
                        $("#FormModal").modal("hide");
                    } else {
                        // Mostrar mensajes de alerta
                        showToast(response.message, "error");
                    }
                },
                error: function (error) {
                    showToast("Ha ocurrido un error al modificar el usuario.", "error");
                    console.error(error);
                }
            });
        }
    });

    // Evento de envío del formulario de creación/edición
    $(document).on("submit", "#usuarioForm", function (event) {
        event.preventDefault();      
    });

    function validar() {
        var nombre = $("#txtnombre").val();
        var isValid = nombre.length >= 4 && nombre.length <= 20;

        // Actualiza el mensaje de error y el estilo
        if (isValid) {
            $("#nombreError").text("").hide();
        } else {
            $("#nombreError").text("El nombre debe tener entre 4 y 20 caracteres.").show();
        }

        return isValid;
    }

    // Reiniciar campos al cerrar el modal
    $("#FormModal").on("hidden.bs.modal", function () {
        $("#usuarioForm")[0].reset();
        $("#previewImg").attr("src", ""); // Restablecer el atributo src de la imagen a vacío
    });

    // Evento de clic para el botón de eliminar
    $("#tabla tbody").on("click", ".btn-eliminar", function () {
        var filaSeleccionada = $(this).closest("tr");
        var data = tabla.row(filaSeleccionada).data();
        var idUsuario = data.id;

        // Al hacer clic en el botón de eliminar, abrimos el modal de confirmación
        $("#confirmarEliminarModal").modal("show");

        // Configuramos el evento de clic para el botón "Confirmar" del modal
        $("#btnConfirmarEliminar").on("click", function () {
            // Hacer la solicitud de eliminación al servidor
            $.post("/BackUser/EliminarUsuario", { id: idUsuario })
                .done(function (response) {
                    showToast("Usuario eliminado exitosamente.", "success");
                    tabla.row(filaSeleccionada).remove().draw(); // Eliminar la fila de la tabla
                })
                .fail(function (error) {
                    showToast("Ha ocurrido un error al eliminar el usuario.", "error");
                    console.error(error);
                });

            // Cerrar el modal después de hacer clic en "Confirmar"
            $("#confirmarEliminarModal").modal("hide");

            // Eliminar el evento de clic para evitar ejecuciones duplicadas
            $("#btnConfirmarEliminar").off("click");
        });
    });
});

