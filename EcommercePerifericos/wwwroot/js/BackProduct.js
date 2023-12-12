
$(function ($) {

    /*funcion que abre el modal con producto cargado o vacio dependienco si recibe un json*/
    function abrirModal(json) {
        $("#txtid").val(0);
        $("#txtnombre").val("");
        $("#txtprecio").val("");
        $("#txtstock").val("");
        $("#txtimg").val("");
        $("#txtcategoria").val("");

        if (json !== null) {
            $("#txtid").val(json.id);
            $("#txtnombre").val(json.nombre);
            $("#txtprecio").val(json.precio);
            $("#txtstock").val(json.stock);
            $("#txtcategoria").val(json.idCategoria);

            if (json.img !== null && json.img !== '') {
                var imgSrc = 'data:image/png;base64,' + json.img;
                $("#previewImg").attr("src", imgSrc);
            } else {
                $("#previewImg").attr("src", ""); 
            }

        }

        $("#FormModal").modal("show");
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
            url: '/BackProduct/ProductList',
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data'
        },
        columns: [
            { data: 'id' },
            {
                data: 'img',
                render: function (data) {
                    if (data !== null && data !== '') {
                        return '<img src="data:image/png;base64,' + data + '" width="135" height="100" />';
                    } else {
                        return '';
                    }
                },
                defaultContent: ''
            },
            { data: 'nombre' },
            { data: 'stock' },
            { data: 'categoria' },
            { data: 'precio' },
            { data: 'activo' },
            {
                defaultContent: '<div style="display: flex;">' +
                    '<button type="button" class="btn btn-primary btn-sm btn-editar"><i class="bi bi-pencil"></i></button>' +
                    '<button type="button" class="btn btn-danger btn-sm ms-2 btn-eliminar"><i class="bi bi-trash3"></i></button>' +
                    '</div>',
                orderable: false,
                searchable: false,
                width: "60px"
            },
            { data: 'idCategoria', visible: false }
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

    // Evento de clic para cambiar el estado "activo" de un producto
    $("#tabla tbody").on("click", "td:nth-child(7)", function () {
        var filaSeleccionada = $(this).closest("tr");
        var data = tabla.row(filaSeleccionada).data();
        var idProducto = data.id;
        var estadoActual = data.activo;

        var nuevoEstado = estadoActual === "activo" ? "inactivo" : "activo";

        $.post("/BackProduct/CambiarEstadoProducto", { id: idProducto, nuevoEstado: nuevoEstado })
            .done(function (response) {
                showToast("Estado del producto cambiado exitosamente.", "success");
                data.activo = nuevoEstado;
                tabla.row(filaSeleccionada).data(data).draw(); // Actualizar el estado en la tabla
            })
            .fail(function (error) {
                showToast("Ha ocurrido un error al cambiar el estado del producto.", "error");
                console.error(error);
            });
    });

    /*Evento Crear y Modificar Producto*/
    $(document).on("click", "#btnGuardarCambios", function () {
        var formData = new FormData($("#usuarioForm")[0]);

        if ($("#txtid").val() == 0) {
            $.ajax({
                url: "/BackProduct/CrearProducto",
                type: "POST",
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    showToast("Producto creado exitosamente.", "success");
                    tabla.ajax.reload();
                    $("#FormModal").modal("hide");
                },
                error: function (error) {
                    showToast("Ha ocurrido un error al crear el producto.", "error");
                    console.error(error);
                }
            });
        } else {
            formData.append("id", $("#txtid").val());
            $.ajax({
                url: "/BackProduct/ModificarProducto",
                type: "POST",
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    showToast("Producto modificado exitosamente.", "success");
                    tabla.ajax.reload();
                    $("#FormModal").modal("hide");
                    console.log(response);
                },
                error: function (error) {
                    showToast("Ha ocurrido un error al modificar el producto.", "error");
                    console.error(error);
                }
            });
        }
    });

    // Evento de envío del formulario de creación/edición
    $(document).on("submit", "#usuarioForm", function (event) {
        event.preventDefault();
    });

    // Reiniciar campos al cerrar el modal
    $("#FormModal").on("hidden.bs.modal", function () {
        $("#usuarioForm")[0].reset();
        $("#previewImg").attr("src", ""); // Restablecer el atributo src de la imagen a vacío
    });

    // Evento de clic para el botón de eliminar
    $("#tabla tbody").on("click", ".btn-eliminar", function () {
        var filaSeleccionada = $(this).closest("tr");
        var data = tabla.row(filaSeleccionada).data();
        var idProducto = data.id;

        // Al hacer clic en el botón de eliminar, abrimos el modal de confirmación
        $("#confirmarEliminarModal").modal("show");

        // Configuramos el evento de clic para el botón "Confirmar" del modal
        $("#btnConfirmarEliminar").on("click", function () {
            // Hacer la solicitud de eliminación al servidor
            $.post("/BackProduct/EliminarProducto", { id: idProducto })
                .done(function (response) {
                    showToast("Producto eliminado exitosamente.", "success");
                    tabla.row(filaSeleccionada).remove().draw(); // Eliminar la fila de la tabla
                })
                .fail(function (error) {
                    showToast("Ha ocurrido un error al eliminar el producto.", "error");
                    console.error(error);
                });

            // Cerrar el modal después de hacer clic en "Confirmar"
            $("#confirmarEliminarModal").modal("hide");

            // Eliminar el evento de clic para evitar ejecuciones duplicadas
            $("#btnConfirmarEliminar").off("click");
        });
    });
});

