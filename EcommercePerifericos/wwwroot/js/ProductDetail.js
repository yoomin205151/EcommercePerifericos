$(function ($) {

    var cantidadInput = $('#quantityInput');

    cantidadInput.val(1);

    // Botón de resta
    $('#button-addon1').on('click', function () {
        restarCantidad();
    });

    // Botón de suma
    $('#button-addon2').on('click', function () {
        sumarCantidad();
    });

    var urlParams = new URLSearchParams(window.location.search);
    var productoID = urlParams.get('id');

    obtenerDetallesProducto(productoID);

    function obtenerDetallesProducto(productoID) {
        $.ajax({
            url: '/Product/ProductDetailProduct',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(productoID),
            success: function (response) {
                console.log('Detalles del producto obtenidos exitosamente:', response);

                // Verificar si la respuesta contiene datos y si la propiedad success es verdadera
                if (response.success && response.data) {
                    // Actualizar los elementos HTML con la información del producto
                    actualizarInterfazUsuario(response.data);
                } else {
                    console.error('No se recibieron datos válidos del servidor.');
                }
            },
            error: function (error) {
                console.error('Error al obtener detalles del producto:', error);
            }
        });
    }

    function actualizarInterfazUsuario(producto) {
        // Actualizar elementos HTML con la información del producto
        $('.title').html(producto.nombre);
        $('.text-muted').html('<i class="bi bi-bag-check"></i>' + producto.stock);
        $('.text-success').html('In stock');

        // Mostrar precio normal o de oferta
        var precioHTML = '<span class="h5">$' + producto.precio + '</span>';
        if (producto.precioOferta !== null) {
            precioHTML += '<span class="text-danger"><s>$' + producto.precioOferta + '</s></span>';
        }
        $('.detailPrecio').html(precioHTML);

        $('.detailDetalle').html(producto.detalle);
        $('.detailCategoria').html(producto.categoria);
        $('.detailIdProducto').html(producto.id);

        var base64Image = 'data:image/png;base64,' + producto.img;
        $('.detailImage img').attr('src', base64Image);

        // Puedes agregar más actualizaciones según la estructura de tu vista
    }

    function restarCantidad() {
        var cantidadActual = parseInt(cantidadInput.val(), 10);

        // Verificar que la cantidad no sea 1 antes de restar
        if (cantidadActual > 1) {
            cantidadInput.val(cantidadActual - 1);
        }
    }

    function sumarCantidad() {
        var cantidadActual = parseInt(cantidadInput.val(), 10);
        var stockDisponible = parseInt($('.text-muted').text().replace(/\D/g, ''), 10);

        // Verificar que la cantidad no exceda el stock disponible
        if (cantidadActual < stockDisponible) {
            cantidadInput.val(cantidadActual + 1);
        }
    }
})