using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoClinica.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ProyectoClinica.Controllers
{
    public class AsignacionRolesTemporalesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AsignacionRolesTemporalesController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
        }

        [Authorize(Roles = "Administrador")]
        // GET: AsignacionRolesTemporales
        public ActionResult Index()
        {
            // Obtener el usuario actual
            string userId = User.Identity.GetUserId();
            
            // Si el usuario no es administrador, solo mostrar sus asignaciones
            if (!User.IsInRole("Administrador"))
            {
                var asignacionRolesTemporales = _context.AsignacionRolesTemporales
                    .Include(a => a.ApplicationUser)
                    .Include(a => a.Departamentos)
                    .Where(a => a.Id_Usuario == userId && a.Estado == "Activo")
                    .ToList();
                return View(asignacionRolesTemporales);
            }
            else
            {
                // Si es administrador, mostrar todas las asignaciones
                var asignacionRolesTemporales = _context.AsignacionRolesTemporales
                    .Include(a => a.ApplicationUser)
                    .Include(a => a.Departamentos);
                return View(asignacionRolesTemporales.ToList());
            }
        }

        [Authorize(Roles = "Administrador")]
        // GET: AsignacionRolesTemporales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            string userId = User.Identity.GetUserId();
            AsignacionRolesTemporales asignacionRolesTemporales = _context.AsignacionRolesTemporales
                .Include(a => a.ApplicationUser)
                .Include(a => a.Departamentos)
                .FirstOrDefault(a => a.Id_AsignacionRoles == id);

            if (asignacionRolesTemporales == null)
            {
                return HttpNotFound();
            }

            // Verificar acceso al departamento (solo administradores o usuarios asignados al departamento)
            if (!User.IsInRole("Administrador") && asignacionRolesTemporales.Id_Usuario != userId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "No tiene acceso a esta asignación de rol.");
            }

            return View(asignacionRolesTemporales);
        }

        [Authorize(Roles = "Administrador")]
        // GET: AsignacionRolesTemporales/Create
        public ActionResult Create()
        {
            // Cargamos la lista de usuarios para ambos campos
            ViewBag.Id = new SelectList(_context.Users, "Id", "UserName");
            ViewBag.Id_Usuario = new SelectList(_context.Users, "Id", "UserName");
            ViewBag.Id_Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento");
            return View();
        }

        // POST: AsignacionRolesTemporales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_AsignacionRoles,Id,Id_Usuario,Id_Departamento,Fecha_Inicio,Fecha_Fin,Estado,Motivo")] AsignacionRolesTemporales asignacionRolesTemporales)
        {
            // Validación de fechas
            DateTime hoy = DateTime.Today;
            if (asignacionRolesTemporales.Fecha_Inicio < hoy)
            {
                ModelState.AddModelError("Fecha_Inicio", "No se permiten fechas pasadas.");
                ViewBag.Id = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                ViewBag.Id_Usuario = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                ViewBag.Id_Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                return View(asignacionRolesTemporales);
            }
            
            if (asignacionRolesTemporales.Fecha_Inicio.DayOfWeek == DayOfWeek.Saturday || asignacionRolesTemporales.Fecha_Inicio.DayOfWeek == DayOfWeek.Sunday)
            {
                ModelState.AddModelError("Fecha_Inicio", "No se permiten fechas en sábado ni domingo.");
                ViewBag.Id = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                ViewBag.Id_Usuario = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                ViewBag.Id_Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                return View(asignacionRolesTemporales);
            }
            
            if (asignacionRolesTemporales.Fecha_Fin < hoy)
            {
                ModelState.AddModelError("Fecha_Fin", "No se permiten fechas pasadas.");
                ViewBag.Id = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                ViewBag.Id_Usuario = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                ViewBag.Id_Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                return View(asignacionRolesTemporales);
            }
            
            if (asignacionRolesTemporales.Fecha_Fin.DayOfWeek == DayOfWeek.Saturday || asignacionRolesTemporales.Fecha_Fin.DayOfWeek == DayOfWeek.Sunday)
            {
                ModelState.AddModelError("Fecha_Fin", "No se permiten fechas en sábado ni domingo.");
                ViewBag.Id = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                ViewBag.Id_Usuario = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                ViewBag.Id_Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                return View(asignacionRolesTemporales);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    // Verificar que los campos requeridos no sean nulos
                    if (asignacionRolesTemporales.Id == null || asignacionRolesTemporales.Id_Departamento == null)
                    {
                        ModelState.AddModelError("", "Debe seleccionar un usuario y un departamento");
                        throw new Exception("Campos requeridos faltantes");
                    }

                    // Verificar que no exista una asignación activa para el mismo usuario y departamento
                    var asignacionExistente = _context.AsignacionRolesTemporales
                        .FirstOrDefault(a => a.Id_Usuario == asignacionRolesTemporales.Id_Usuario && 
                                           a.Id_Departamento == asignacionRolesTemporales.Id_Departamento && 
                                           a.Estado == "Activo");

                    if (asignacionExistente != null)
                    {
                        ModelState.AddModelError("", "Ya existe una asignación activa para este usuario en este departamento.");
                        ViewBag.Id = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                        ViewBag.Id_Usuario = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                        ViewBag.Id_Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                        return View(asignacionRolesTemporales);
                    }

                    // Asignamos la fecha actual al crear el registro
                    asignacionRolesTemporales.Fecha_Inicio = DateTime.Now;
                    
                    // Asignamos el estado por defecto
                    asignacionRolesTemporales.Estado = "Activo";
                    
                    // Agregamos el registro al contexto
                    _context.AsignacionRolesTemporales.Add(asignacionRolesTemporales);
                    
                    // Intentamos guardar los cambios
                    _context.SaveChanges();
                    
                    // Asignar el rol correspondiente al usuario en AspNetUsers
                    AsignarRolDepartamento(asignacionRolesTemporales.Id_Usuario, asignacionRolesTemporales.Id_Departamento);
                    
                    // Crear notificación para el usuario asignado
                    CrearNotificacion(asignacionRolesTemporales.Id_Usuario, 
                                    "Se le ha asignado un rol temporal en el departamento " + 
                                    ObtenerNombreDepartamento(asignacionRolesTemporales.Id_Departamento));
                    
                    TempData["SuccessMessage"] = "Registro creado exitosamente y rol asignado al usuario";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Si hay errores de validación, los mostramos
                    foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        ModelState.AddModelError("", modelError.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear el registro: " + ex.Message);
                // Log del error
                System.Diagnostics.Debug.WriteLine("Error en Create: " + ex.Message);
            }

            // Si hay error, recargamos las listas desplegables
            ViewBag.Id = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id);
            ViewBag.Id_Usuario = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
            ViewBag.Id_Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
            return View(asignacionRolesTemporales);
        }

        [Authorize(Roles = "Administrador")]
        // GET: AsignacionRolesTemporales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            string userId = User.Identity.GetUserId();
            AsignacionRolesTemporales asignacionRolesTemporales = _context.AsignacionRolesTemporales
                .Include(a => a.ApplicationUser)
                .Include(a => a.Departamentos)
                .FirstOrDefault(a => a.Id_AsignacionRoles == id);

            if (asignacionRolesTemporales == null)
            {
                return HttpNotFound();
            }

            // Verificar acceso al departamento (solo administradores o usuarios asignados al departamento)
            if (!User.IsInRole("Administrador") && asignacionRolesTemporales.Id_Usuario != userId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "No tiene acceso a esta asignación de rol.");
            }

            ViewBag.Id = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id);
            ViewBag.Id_Usuario = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
            ViewBag.Id_Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
            return View(asignacionRolesTemporales);
        }

        // POST: AsignacionRolesTemporales/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_AsignacionRoles,Id,Id_Usuario,Id_Departamento,Fecha_Inicio,Fecha_Fin,Estado,Motivo")] AsignacionRolesTemporales asignacionRolesTemporales)
        {
            // Validación de fechas
            DateTime hoy = DateTime.Today;
            if (asignacionRolesTemporales.Fecha_Inicio < hoy)
            {
                ModelState.AddModelError("Fecha_Inicio", "No se permiten fechas pasadas.");
                ViewBag.Id = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                ViewBag.Id_Usuario = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                ViewBag.Id_Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                return View(asignacionRolesTemporales);
            }
            
            if (asignacionRolesTemporales.Fecha_Inicio.DayOfWeek == DayOfWeek.Saturday || asignacionRolesTemporales.Fecha_Inicio.DayOfWeek == DayOfWeek.Sunday)
            {
                ModelState.AddModelError("Fecha_Inicio", "No se permiten fechas en sábado ni domingo.");
                ViewBag.Id = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                ViewBag.Id_Usuario = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                ViewBag.Id_Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                return View(asignacionRolesTemporales);
            }
            
            if (asignacionRolesTemporales.Fecha_Fin < hoy)
            {
                ModelState.AddModelError("Fecha_Fin", "No se permiten fechas pasadas.");
                ViewBag.Id = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                ViewBag.Id_Usuario = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                ViewBag.Id_Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                return View(asignacionRolesTemporales);
            }
            
            if (asignacionRolesTemporales.Fecha_Fin.DayOfWeek == DayOfWeek.Saturday || asignacionRolesTemporales.Fecha_Fin.DayOfWeek == DayOfWeek.Sunday)
            {
                ModelState.AddModelError("Fecha_Fin", "No se permiten fechas en sábado ni domingo.");
                ViewBag.Id = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                ViewBag.Id_Usuario = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                ViewBag.Id_Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                return View(asignacionRolesTemporales);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    // Verificar que los campos requeridos no sean nulos
                    if (asignacionRolesTemporales.Id == null || asignacionRolesTemporales.Id_Departamento == null)
                    {
                        ModelState.AddModelError("", "Debe seleccionar un usuario y un departamento");
                        throw new Exception("Campos requeridos faltantes");
                    }

                    // Obtener el registro original para comparar cambios
                    var asignacionOriginal = _context.AsignacionRolesTemporales
                        .AsNoTracking()
                        .FirstOrDefault(a => a.Id_AsignacionRoles == asignacionRolesTemporales.Id_AsignacionRoles);

                    // Actualizamos el registro
                    _context.Entry(asignacionRolesTemporales).State = EntityState.Modified;
                    
                    // Intentamos guardar los cambios
                    _context.SaveChanges();
                    
                    // Si cambió el departamento, actualizar el rol del usuario
                    if (asignacionOriginal != null && asignacionOriginal.Id_Departamento != asignacionRolesTemporales.Id_Departamento)
                    {
                        // Remover rol anterior
                        RemoverRolDepartamento(asignacionRolesTemporales.Id_Usuario, asignacionOriginal.Id_Departamento);
                        // Asignar nuevo rol
                        AsignarRolDepartamento(asignacionRolesTemporales.Id_Usuario, asignacionRolesTemporales.Id_Departamento);
                    }
                    
                    // Crear notificación si cambió el departamento o el estado
                    if (asignacionOriginal != null && 
                        (asignacionOriginal.Id_Departamento != asignacionRolesTemporales.Id_Departamento ||
                         asignacionOriginal.Estado != asignacionRolesTemporales.Estado))
                    {
                        string mensaje = "Su asignación de rol temporal ha sido modificada. ";
                        if (asignacionOriginal.Id_Departamento != asignacionRolesTemporales.Id_Departamento)
                        {
                            mensaje += $"Nuevo departamento: {ObtenerNombreDepartamento(asignacionRolesTemporales.Id_Departamento)}. ";
                        }
                        if (asignacionOriginal.Estado != asignacionRolesTemporales.Estado)
                        {
                            mensaje += $"Nuevo estado: {asignacionRolesTemporales.Estado}.";
                        }
                        
                        CrearNotificacion(asignacionRolesTemporales.Id_Usuario, mensaje);
                    }
                    
                    TempData["SuccessMessage"] = "Registro actualizado exitosamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Si hay errores de validación, los mostramos
                    foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        ModelState.AddModelError("", modelError.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar el registro: " + ex.Message);
                // Log del error
                System.Diagnostics.Debug.WriteLine("Error en Edit: " + ex.Message);
            }

            // Si hay error, recargamos las listas desplegables
            ViewBag.Id = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id);
            ViewBag.Id_Usuario = new SelectList(_context.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
            ViewBag.Id_Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
            return View(asignacionRolesTemporales);
        }

        [Authorize(Roles = "Administrador")]
        // GET: AsignacionRolesTemporales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            string userId = User.Identity.GetUserId();
            AsignacionRolesTemporales asignacionRolesTemporales = _context.AsignacionRolesTemporales
                .Include(a => a.ApplicationUser)
                .Include(a => a.Departamentos)
                .FirstOrDefault(a => a.Id_AsignacionRoles == id);

            if (asignacionRolesTemporales == null)
            {
                return HttpNotFound();
            }

            // Verificar acceso al departamento (solo administradores o usuarios asignados al departamento)
            if (!User.IsInRole("Administrador") && asignacionRolesTemporales.Id_Usuario != userId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "No tiene acceso a esta asignación de rol.");
            }

            return View(asignacionRolesTemporales);
        }

        // POST: AsignacionRolesTemporales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                AsignacionRolesTemporales asignacionRolesTemporales = _context.AsignacionRolesTemporales
                    .Include(a => a.Departamentos)
                    .FirstOrDefault(a => a.Id_AsignacionRoles == id);
                
                if (asignacionRolesTemporales != null)
                {
                    // Remover el rol del usuario antes de eliminar la asignación
                    RemoverRolDepartamento(asignacionRolesTemporales.Id_Usuario, asignacionRolesTemporales.Id_Departamento);
                    
                    // Crear notificación antes de eliminar
                    CrearNotificacion(asignacionRolesTemporales.Id_Usuario, 
                                    "Su asignación de rol temporal en el departamento " + 
                                    ObtenerNombreDepartamento(asignacionRolesTemporales.Id_Departamento) + 
                                    " ha sido eliminada.");
                    
                    _context.AsignacionRolesTemporales.Remove(asignacionRolesTemporales);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Registro eliminado exitosamente y rol removido del usuario";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar el registro: " + ex.Message;
            }
            return RedirectToAction("Index");
        }

        // Método para asignar rol según el departamento
        private void AsignarRolDepartamento(string userId, int departamentoId)
        {
            try
            {
                var departamento = _context.Departamentos.FirstOrDefault(d => d.Id_Departamento == departamentoId);
                if (departamento == null) return;

                // Obtener el nombre del departamento y mapearlo al rol correspondiente
                string nombreDepartamento = departamento.Nombre_Departamento;
                string rolAsignar = MapearDepartamentoARol(nombreDepartamento);
                
                if (!string.IsNullOrEmpty(rolAsignar))
                {
                    // Verificar si el rol existe en AspNetRoles
                    if (_roleManager.RoleExists(rolAsignar))
                    {
                        // Obtener todos los roles actuales del usuario
                        var rolesActuales = _userManager.GetRoles(userId);
                        
                        // Remover todos los roles existentes (excepto Usuario si existe)
                        foreach (var rol in rolesActuales)
                        {
                            if (rol != "Usuario")
                            {
                                var resultadoRemover = _userManager.RemoveFromRole(userId, rol);
                                if (!resultadoRemover.Succeeded)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Error al remover rol {rol}: {string.Join(", ", resultadoRemover.Errors)}");
                                }
                            }
                        }
                        
                        // Asignar el nuevo rol
                        var resultadoAsignacion = _userManager.AddToRole(userId, rolAsignar);
                        if (resultadoAsignacion.Succeeded)
                        {
                            System.Diagnostics.Debug.WriteLine($"Rol {rolAsignar} asignado exitosamente al usuario {userId}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Error al asignar rol {rolAsignar} al usuario {userId}: {string.Join(", ", resultadoAsignacion.Errors)}");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"El rol {rolAsignar} no existe en AspNetRoles");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en AsignarRolDepartamento: {ex.Message}");
            }
        }

        // Método para remover rol según el departamento
        private void RemoverRolDepartamento(string userId, int departamentoId)
        {
            try
            {
                var departamento = _context.Departamentos.FirstOrDefault(d => d.Id_Departamento == departamentoId);
                if (departamento == null) return;

                string nombreDepartamento = departamento.Nombre_Departamento;
                string rolRemover = MapearDepartamentoARol(nombreDepartamento);
                
                if (!string.IsNullOrEmpty(rolRemover))
                {
                    var resultado = _userManager.RemoveFromRole(userId, rolRemover);
                    if (resultado.Succeeded)
                    {
                        System.Diagnostics.Debug.WriteLine($"Rol {rolRemover} removido exitosamente del usuario {userId}");
                        
                        // Asignar rol "Usuario" por defecto si no tiene ningún rol
                        var rolesActuales = _userManager.GetRoles(userId);
                        if (!rolesActuales.Any())
                        {
                            _userManager.AddToRole(userId, "Usuario");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Error al remover rol {rolRemover} del usuario {userId}: {string.Join(", ", resultado.Errors)}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en RemoverRolDepartamento: {ex.Message}");
            }
        }

        // Método para mapear departamento a rol
        private string MapearDepartamentoARol(string nombreDepartamento)
        {
            // Mapeo directo de departamentos a roles existentes en AspNetRoles
            switch (nombreDepartamento.ToLower())
            {
                case "contabilidad":
                    return "Contador";
                case "medicina":
                    return "Medico";
                case "secretaria":
                    return "Secretaria";
                case "auditor":
                    return "Auditor";
                case "administrador":
                    return "Administrador";
                case "usuario":
                    return "Usuario";
                default:
                    // Si no hay mapeo específico, retornar el nombre del departamento tal como está
                    // asumiendo que coincide con el nombre del rol en AspNetRoles
                    return nombreDepartamento;
            }
        }

        // Método para verificar acceso al departamento
        private bool TieneAccesoDepartamento(string userId, int departamentoId)
        {
            return _context.AsignacionRolesTemporales
                .Any(a => a.Id_Usuario == userId && 
                          a.Id_Departamento == departamentoId && 
                          a.Estado == "Activo" &&
                          a.Fecha_Fin >= DateTime.Now);
        }

        // Método para obtener el nombre del departamento
        private string ObtenerNombreDepartamento(int departamentoId)
        {
            var departamento = _context.Departamentos.FirstOrDefault(d => d.Id_Departamento == departamentoId);
            return departamento?.Nombre_Departamento ?? "Departamento Desconocido";
        }

        // Método para crear notificaciones
        private void CrearNotificacion(string userId, string mensaje)
        {
            try
            {
                var notificacion = new Notificacion
                {
                    Mensaje = mensaje,
                    Fecha = DateTime.Now
                };

                _context.Notificacion.Add(notificacion);
                _context.SaveChanges();

                // También almacenamos en sesión para notificaciones en tiempo real
                var notificaciones = Session["Notificaciones"] as List<string> ?? new List<string>();
                notificaciones.Add(mensaje);
                Session["Notificaciones"] = notificaciones;
                Session["ContadorNotificaciones"] = notificaciones.Count;
            }
            catch (Exception ex)
            {
                // Log del error de notificación
                System.Diagnostics.Debug.WriteLine("Error al crear notificación: " + ex.Message);
            }
        }

        // Método para obtener asignaciones activas del usuario
        public ActionResult MisAsignaciones()
        {
            string userId = User.Identity.GetUserId();
            
            var misAsignaciones = _context.AsignacionRolesTemporales
                .Include(a => a.Departamentos)
                .Where(a => a.Id_Usuario == userId && a.Estado == "Activo")
                .ToList();

            return View(misAsignaciones);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
                _userManager?.Dispose();
                _roleManager?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
