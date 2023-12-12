using EcommercePerifericos.Models.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EcommercePerifericos.Controllers
{
    public class BackProductController : Controller
    {
        private Models.DB.AleshopBdContext dbContext;
        public BackProductController(Models.DB.AleshopBdContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var model = new Producto();
            var categorias = dbContext.Categoria.ToList();
            ViewBag.Categorias = categorias;
            return View(model);
        }

        public JsonResult ProductList(int categoriaId = 0)
        {
            List<Producto> productos;

            try
            {
                if (categoriaId > 0)
                {
                    // Filtrar los productos por categoría con carga ansiosa
                    productos = dbContext.Productos
                        .Include(p => p.IdCategoriaNavigation)
                        .Where(p => p.IdCategoria == categoriaId)
                        .ToList();
                }
                else
                {
                    // Obtener todos los productos con carga ansiosa
                    productos = dbContext.Productos
                        .Include(p => p.IdCategoriaNavigation)
                        .ToList();
                }

                var productosDTO = productos.Select(p => new
                {
                    p.Id,
                    p.Img,
                    p.Nombre,
                    p.Stock,
                    categoria = p.IdCategoriaNavigation?.Nombre ?? "Sin categoría",
                    p.IdCategoria,
                    p.Precio,
                    p.Activo
                });

                return Json(new { data = productosDTO });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al cargar productos: {ex.Message}");
                return Json(new { data = new List<object>() }); 
            }
        }

        [HttpPost]
        public IActionResult CambiarEstadoProducto(int id, string nuevoEstado)
        {
           var producto = dbContext.Productos.Find(id);          
            try
            {
                if (producto != null)
                {
                    producto.Activo = nuevoEstado;
                    dbContext.SaveChanges();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "El producto no existe." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al cambiar el estado: {ex.Message}" });
            }

        }

        [HttpPost]
        public IActionResult CrearProducto(Producto nuevoProducto, IFormFile ImagenFile)
        {
            if (ModelState.IsValid)
            {
                // Establecer el valor predeterminado del campo "activo"
                nuevoProducto.Activo = "activo";

                // Guardar la imagen como base64 en la propiedad img
                if (ImagenFile != null && ImagenFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        ImagenFile.CopyTo(memoryStream);
                        nuevoProducto.Img = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }

                // Realizar lógica de creación de producto, por ejemplo, guardar en la base de datos
                dbContext.Productos.Add(nuevoProducto);
                dbContext.SaveChanges();

                // Opcional: Redirigir a una página de éxito o mostrar un mensaje de éxito en la vista
                TempData["SuccessMessage"] = "Producto creado exitosamente.";

                // Redirigir a una página de éxito o regresar a la vista original
                return RedirectToAction("Index");
            }

            // Si llegamos a este punto, significa que ha ocurrido un error en la validación
            // Vuelve a cargar la vista con los mensajes de error
            var categorias = dbContext.Categoria.ToList();
            ViewBag.Categorias = categorias;
            return View("Index");
        }

        [HttpPost]
        public IActionResult EliminarProducto(int id)
        {
            try
            {
                var productoExistente = dbContext.Productos.Find(id);
                if (productoExistente != null)
                {
                    dbContext.Productos.Remove(productoExistente);
                    dbContext.SaveChanges();
                    TempData["SuccessMessage"] = "Producto eliminado exitosamente.";
                    return Json(new { success = true });
                }
                else
                {
                    TempData["ErrorMessage"] = "El producto no existe.";
                    return Json(new { success = false });
                }
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                string? errorMessage = innerException?.Message;


                Console.WriteLine(errorMessage);

                TempData["ErrorMessage"] = "Ha ocurrido un error al eliminar el producto.";
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public IActionResult ModificarProducto(Producto productoModificado, IFormFile ImagenFile)
        {
            try
            {
                if (productoModificado != null)
                {
                    var productoExistente = dbContext.Productos.Find(productoModificado.Id);
                    if (productoExistente != null)
                    {
                        // Actualizar las propiedades del producto existente con los valores modificados
                        productoExistente.Nombre = productoModificado.Nombre;
                        productoExistente.Stock = productoModificado.Stock;
                        productoExistente.Precio = productoModificado.Precio;
                        productoExistente.IdCategoria = productoModificado.IdCategoria;

                        Console.WriteLine("Producto Modificado:");
                        Console.WriteLine(productoModificado);

                        // Guardar la nueva imagen en base64 si se proporciona
                        if (ImagenFile != null && ImagenFile.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                ImagenFile.CopyTo(memoryStream);
                                productoExistente.Img = Convert.ToBase64String(memoryStream.ToArray());
                            }
                        }

                        Console.WriteLine("Producto Modificado:");
                        Console.WriteLine(productoExistente);

                        // Guardar los cambios en la base de datos
                        dbContext.SaveChanges();

                        // Opcional: Redirigir a una página de éxito o mostrar un mensaje de éxito en la vista
                        TempData["SuccessMessage"] = "Producto modificado exitosamente.";
                    }
                    else
                    {
                        // Si el producto no existe, puedes mostrar un mensaje de error o redirigir a una página de error
                        TempData["ErrorMessage"] = "El producto no existe.";
                    }

                    // Redirigir a una página de éxito o regresar a la vista original
                    return RedirectToAction("Index");
                }

                // Si llegamos a este punto, significa que ha ocurrido un error en la validación
                // Vuelve a cargar la vista con los mensajes de error
                var categorias = dbContext.Categoria.ToList();
                ViewBag.Categorias = categorias;
                return View("Index", productoModificado);
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí (puedes imprimir mensajes de error, registrarlos, etc.)
                Console.WriteLine($"Error en ModificarProducto: {ex.Message}");

                // Puedes redirigir a una página de error específica o realizar alguna otra acción según tus necesidades
                TempData["ErrorMessage"] = "Ha ocurrido un error al modificar el producto.";
                return RedirectToAction("Index");
            }
        }



    }
}
