using EcommercePerifericos.Models.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EcommercePerifericos.Controllers
{
    [Authorize]
    public class BackUserController : Controller
    {
        private Models.DB.AleshopBdContext dbContext;
        public BackUserController(Models.DB.AleshopBdContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var roles = dbContext.Rols.ToList();
            ViewBag.Roles = roles;
            return View();
        }

        public async Task<IActionResult> UserList()
        {
            List<Usuario>User = await dbContext.Usuarios
                .Include(u => u.IdRolNavigation)
                .ToListAsync();

            try
            {
                var userDTO = User.Select(u => new
                {
                    u.Id,
                    u.Nombre,
                    u.Apellido,
                    u.Email,
                    u.Contrasenia,
                    u.IdRol,
                    Rol = u.IdRolNavigation?.Tipo ?? "Sin Rol"
                });

                return Json(new { data = userDTO });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al cargar usuarios: {ex.Message}");
                return Json(new { data = new List<object>() });
            }
            
        }

        [HttpPost]
        public IActionResult CrearUsuario(Usuario nuevoUsuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Realizar lógica de creación de producto, por ejemplo, guardar en la base de datos
                    dbContext.Usuarios.Add(nuevoUsuario);
                    dbContext.SaveChanges();

                    // Opcional: Redirigir a una página de éxito o mostrar un mensaje de éxito en la vista
                    TempData["SuccessMessage"] = "Usuario creado exitosamente.";

                    // Redirigir a una página de éxito o regresar a la vista original
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Datos de usuario no válidos.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al Crear Usuario: {ex.Message}");

                TempData["ErrorMessage"] = "Ha ocurrido un error al crear usuario.";
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        public IActionResult EliminarUsuario(int id)
        {
            try
            {
                var usuarioExistente = dbContext.Usuarios.Find(id);
                if (usuarioExistente != null)
                {
                    dbContext.Usuarios.Remove(usuarioExistente);
                    dbContext.SaveChanges();
                    TempData["SuccessMessage"] = "Usuario eliminado exitosamente.";
                    return Json(new { success = true });
                }
                else
                {
                    TempData["ErrorMessage"] = "El usuario no existe.";
                    return Json(new { success = false });
                }
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                string? errorMessage = innerException?.Message;


                Console.WriteLine(errorMessage);

                TempData["ErrorMessage"] = "Ha ocurrido un error al eliminar el usuario.";
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public IActionResult ModificarUsuario(Usuario usuarioModificado)
        {
            try
            {
                if (usuarioModificado != null)
                {
                    var usuarioExistente = dbContext.Usuarios.Find(usuarioModificado.Id);
                    if (usuarioExistente != null)
                    {
                        // Actualizar las propiedades del producto existente con los valores modificados
                        usuarioExistente.Nombre = usuarioModificado.Nombre;
                        usuarioExistente.Apellido = usuarioModificado.Apellido;
                        usuarioExistente.Email = usuarioModificado.Email;
                        usuarioExistente.IdRol = usuarioModificado.IdRol;
                        usuarioExistente.Contrasenia = usuarioModificado.Contrasenia;

                        Console.WriteLine("Usuario Modificado:");
                        Console.WriteLine(usuarioModificado);

                        // Guardar los cambios en la base de datos
                        dbContext.SaveChanges();

                        TempData["SuccessMessage"] = "Usuario modificado exitosamente.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "El usuario no existe.";
                    }

                    return RedirectToAction("Index");
                }

                return View("Index", usuarioModificado);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ModificarProducto: {ex.Message}");

                TempData["ErrorMessage"] = "Ha ocurrido un error al modificar el producto.";
                return RedirectToAction("Index");
            }
        }
    }
}
