using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoClinica.Models;

namespace ProyectoClinica.Controllers
{
    public class AsignacionRolesTemporalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AsignacionRolesTemporalesController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize(Roles = "Administrador")]
        // GET: AsignacionRolesTemporales
        public ActionResult Index()
        {
            var asignacionRolesTemporales = _context.AsignacionRolesTemporales.Include(a => a.ApplicationUser);
            return View(asignacionRolesTemporales.ToList());
        }

        [Authorize(Roles = "Administrador")]
        // GET: AsignacionRolesTemporales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AsignacionRolesTemporales asignacionRolesTemporales = _context.AsignacionRolesTemporales.Find(id);
            if (asignacionRolesTemporales == null)
            {
                return HttpNotFound();
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

                    // Asignamos la fecha actual al crear el registro
                    asignacionRolesTemporales.Fecha_Inicio = DateTime.Now;
                    
                    // Asignamos el estado por defecto
                    asignacionRolesTemporales.Estado = "Activo";
                    
                    // Agregamos el registro al contexto
                    _context.AsignacionRolesTemporales.Add(asignacionRolesTemporales);
                    
                    // Intentamos guardar los cambios
                    _context.SaveChanges();
                    
                    TempData["SuccessMessage"] = "Registro creado exitosamente";
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
            AsignacionRolesTemporales asignacionRolesTemporales = _context.AsignacionRolesTemporales.Find(id);
            if (asignacionRolesTemporales == null)
            {
                return HttpNotFound();
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

                    // Actualizamos el registro
                    _context.Entry(asignacionRolesTemporales).State = EntityState.Modified;
                    
                    // Intentamos guardar los cambios
                    _context.SaveChanges();
                    
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
            AsignacionRolesTemporales asignacionRolesTemporales = _context.AsignacionRolesTemporales.Find(id);
            if (asignacionRolesTemporales == null)
            {
                return HttpNotFound();
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
                AsignacionRolesTemporales asignacionRolesTemporales = _context.AsignacionRolesTemporales.Find(id);
                if (asignacionRolesTemporales != null)
                {
                    _context.AsignacionRolesTemporales.Remove(asignacionRolesTemporales);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Registro eliminado exitosamente";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar el registro: " + ex.Message;
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
