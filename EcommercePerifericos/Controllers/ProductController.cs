using EcommercePerifericos.Models;
using EcommercePerifericos.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EcommercePerifericos.Controllers
{
    public class ProductController : Controller
    {
        private Models.DB.AleshopBdContext dbContext;
        public ProductController(Models.DB.AleshopBdContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductDetail()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ProductList(int categoriaID)
        {
            List<Producto> productos;

            try
            {
                if(categoriaID > 0)
                {
                    productos = await dbContext.Productos.Where(p => p.IdCategoria == categoriaID && p.Activo == "activo").ToListAsync();
                }
                else
                {
                    productos = await dbContext.Productos
                    .Include(p => p.IdCategoriaNavigation)
                    .Where(p => p.Activo == "activo")
                    .ToListAsync();
                }
                
                var productosDTO = productos.Select(p => new
                {
                    p.Id,
                    p.Img,
                    p.Nombre,
                    p.Stock,
                    categoria = p.IdCategoriaNavigation?.Nombre,
                    p.IdCategoria,
                    p.Precio,
                    p.Activo,
                    p.Detalle,
                    p.PrecioOferta
                });

                return Json(new { data = productosDTO });

            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, data = new List<object>() });
            }
        }

        [HttpGet]
        public async Task<IActionResult> CategoriesList()
        {
            List<Categorium> categorias = await dbContext.Categoria.ToListAsync();

            try
            {
                var categoriasDTO = categorias.Select(c => new
                {

                    c.Id,
                    c.Nombre

                });

                return Json(new { data = categoriasDTO });

            }
            catch(Exception ex)
            {
                return Json(new { data = new List<object>() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProductDetailProduct([FromBody] int productoID)
        {
            var producto = await dbContext.Productos
                .Include(p => p.IdCategoriaNavigation)
                .Where(p => p.Id == productoID)
                .FirstOrDefaultAsync();

            if (producto == null)
            {
                // Manejar el caso en que no se encuentre el producto
                return Json(new { success = false, message = "Producto no encontrado" });
            }

            // Mapear el producto a un objeto anónimo con los datos necesarios
            var model = new ProductoViewModel
            (
                producto.Id,
                producto.Img,
                producto.Nombre,
                producto.Stock,
                producto.IdCategoriaNavigation?.Nombre,
                producto.IdCategoria,
                producto.Precio,
                producto.Activo,
                producto.Detalle,
                producto.PrecioOferta
            );

            // Devolver un JSON indicando el éxito y, opcionalmente, los datos del producto
            return Json(new { success = true, data = model });
        }


    }
}
