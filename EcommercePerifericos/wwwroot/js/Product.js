$(function ($) {

    listarCategorias();
    listarProductos();
    /* Función para listar productos */
    function listarProductos(categoriaID) {
        $.ajax({
            url: '/product/ProductList',
            type: 'GET',
            dataType: 'json',
            data: { categoriaID: categoriaID },
            success: function (response) {
                // Verificar si la respuesta contiene datos
                if (response.data && response.data.length > 0) {
                    // Limpiar productos existentes
                    $('#productosContainer').empty();

                    // Recorrer la lista de productos en la respuesta
                    response.data.forEach(function (producto) {
                        // Construir el HTML del producto
                        var card = `
                        <div class="col-lg-4 col-md-6 col-sm-6 d-flex">
                            <div class="card w-100 my-2 shadow-2-strong">
                                <a href="#" class="product-detail-link" data-id="${producto.id}">
                                    <img src="data:image/png;base64,${producto.img}" class="card-img-top" style="aspect-ratio: 1 / 1" />
                                </a>
                                <div class="card-body d-flex flex-column">
                                    <h5 class="card-title">${producto.nombre}</h5>
                                    <p class="mb-1 me-1">Stock: ${producto.stock}</p>
                                    <div class="d-flex flex-row">
                                        <h5 class="mb-1 me-1">$${producto.precio}</h5>
                                        ${producto.precioOferta ? `<span class="text-danger"><s>$${producto.precioOferta}</s></span>` : ''}
                                    </div>
                                    <div class="card-footer d-flex align-items-end pt-3 px-0 pb-0 mt-auto">
                                        <a href="#!" class="btn btn-primary shadow-0 me-1">Add to cart</a>
                                        <a href="#!" class="btn btn-light border px-2 pt-2 icon-hover"><i class="bi bi-heart-fill"></i></a>
                                    </div>
                                </div>
                            </div>
                        </div>`;

                        // Agregar el producto al contenedor
                        $('#productosContainer').append(card);
                    });

                    // Asociar el evento de clic a la clase 'product-detail-link'
                    $('.product-detail-link').on('click', function (event) {
                        // Obtener el ID del producto desde el atributo data-id
                        var productoID = $(this).data('id');
                        // Llamar a la función obtenerDetallesProducto con el ID del producto
                        obtenerDetallesProducto(productoID);
                    });
                } else {
                    // Mostrar un mensaje si no hay productos
                    $('#productosContainer').html('<p>No hay productos disponibles.</p>');
                }
            },
            error: function (error) {
                console.error('Error al cargar productos:', error);
                $('#productosContainer').html('<p>Error al cargar productos.</p>');
            }
        });
    }


    /* Función para listar categorías */
    function listarCategorias() {
        $.ajax({
            url: '/Product/CategoriesList',
            type: 'GET',
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.data && response.data.length > 0) {
                    var categoriesContainer = $("#categoriesCollapse ul");

                    categoriesContainer.empty();

                    response.data.forEach(function (category) {
                        var listItem = $('<li><a href="#" class="text-dark categoria-btn" data-id="' + category.id + '">' + category.nombre + '</a></li>');
                        categoriesContainer.append(listItem);
                    });

                    $('.categoria-btn').on('click', function () {
                        var categoriaID = $(this).data('id');
                        listarProductos(categoriaID);
                    });
                }
            },
            error: function (error) {
                $("#categoriesCollapse").html('<p>Error al cargar categorías.</p>');
                console.error(error);
            }
        });
    }

    function obtenerDetallesProducto(productoID) {
        $.ajax({
            url: '/Product/ProductDetailProduct',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(productoID),  // Solo envía el ID directamente
            success: function (response) {
                console.log('Detalles del producto obtenidos exitosamente:', response);

                // Verificar si la respuesta contiene datos y si la propiedad success es verdadera
                if (response.success && response.data) {
                    // Redirigir a la página de detalles del producto y pasar los datos
                    window.location.href = '/Product/ProductDetail?id=' + response.data.id;
                } else {
                    console.error('No se recibieron datos válidos del servidor.');
                }
            },
            error: function (error) {
                console.error('Error al obtener detalles del producto:', error);
            }
        });
    }



});
