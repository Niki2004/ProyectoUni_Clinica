using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ProyectoClinica.Controllers
{
    public class AdministrativosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdministrativosController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Administrativo
      
        public ActionResult Administrativo()
        {
            // Obtener el ID del usuario actual
            string userId = User.Identity.GetUserId();

            // Verificar si el usuario es administrador
            var userManager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(_context));
            bool esAdmin = userManager.IsInRole(userId, "Administrador");

            if (esAdmin)
            {
                // Si es administrador, tiene acceso total
                ViewBag.TienePermisos = true;
                ViewBag.EsAdministrador = true;
                ViewBag.Estado = "Activo";
                ViewBag.Departamento = "Administración";
                return View();
            }

            // Si no es administrador, verificar asignaciones temporales
            var asignacionActiva = _context.AsignacionRolesTemporales
                .Include(a => a.Departamentos)
                .FirstOrDefault(a => a.Id == userId && a.Estado == "Activo");

            // Pasar la información a la vista
            ViewBag.TienePermisos = asignacionActiva != null;
            ViewBag.EsAdministrador = false;
            ViewBag.Departamento = asignacionActiva?.Departamentos?.Nombre_Departamento;
            ViewBag.Estado = asignacionActiva?.Estado;

            return View();
        }
        [HttpGet]
        public ActionResult VistaADM()
        {
            var listaRegistros = _context.Empleado.ToList();
            return View(listaRegistros);

        }

        [HttpGet]
        public ActionResult VistaEliminar()
        {
            var empleados = _context.Empleado.ToList();
            return View(empleados);
        }

        public ActionResult Create()
        {
            ViewBag.Id_Estado = new SelectList(_context.Estado, "Id_Estado", "Descripcion");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Estado,Comentarios,Nombre,Apellido,Cedula,Correo,Jornada,Fecha_registro,Departamento")] Empleado administrativo)
        {
            if (ModelState.IsValid)
            {
                _context.Empleado.Add(administrativo);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "El empleado se ha creado correctamente.";//para que muestre el comentario en el index 
                return RedirectToAction("Administrativo/Administrativos");
            }
            ViewBag.Id_Estado = new SelectList(_context.Estado, "Id_Estado", "Descripcion", administrativo.Id_Estado);
            return View(administrativo);
        }



        public ActionResult Rol()
        {
            ViewBag.Id_Empleado = new SelectList(_context.Empleado, "Id_Empleado", "Nombre");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rol([Bind(Include = "Id_Empleado,Nombre")] RolAsignacion rolAsignacion)
        {
            if (ModelState.IsValid)
            {
                _context.RolAsignacion.Add(rolAsignacion);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Se Asigno el Rol Correctamente.";
                return RedirectToAction("Administrativo/Administrativos");
            }

            ViewBag.Id_Empleado = new SelectList(_context.Empleado, "Id_Empleado", "Nombre", rolAsignacion.Id_Empleado);
            return View(rolAsignacion);
        }



        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado administrativo = _context.Empleado.Find(id);
            if (administrativo == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Estado = new SelectList(_context.Estado, "Id_Estado", "Descripcion", administrativo.Id_Estado);
            return View(administrativo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Empleado,Id_Estado,Comentarios,Nombre,Apellido,Cedula,Correo,Jornada,Fecha_registro,Departamento")] Empleado administrativo)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(administrativo).State = EntityState.Modified;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "El empleado se ha actualizado correctamente.";
                return RedirectToAction("VistaADM");
            }
            ViewBag.Id_Estado = new SelectList(_context.Estado, "Id_Estado", "Descripcion", administrativo.Id_Estado);
            return View(administrativo);
        }



        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado administrativa = _context.Empleado.Find(id);
            if (administrativa == null)
            {
                return HttpNotFound();
            }
            return View(administrativa);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empleado administrativa = _context.Empleado.Find(id);
            if (administrativa != null)
            {
                _context.Empleado.Remove(administrativa);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Se Elimino Correctamente";
            }
            return RedirectToAction("VistaEliminar");
        }

        public ActionResult VistaListar()
        {
            var RolAsignacion = _context.RolAsignacion.Include("Empleado").ToList();
            return View(RolAsignacion);
        }
        public ActionResult DetallesHistorial(int id)
        {
            var rolAsignacion = _context.RolAsignacion
                .Include("Empleado")
                .FirstOrDefault(r => r.Id_Rol == id);

            if (rolAsignacion == null)
            {
                return HttpNotFound();
            }

            return View(rolAsignacion);
        }


        public ActionResult VistaAuditoría()
        {
            var auditoria = _context.Cita.ToList();
            return View(auditoria);
        }


        public ActionResult Buscar(string nombre, string cedula)
        {
            var administrativo = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
            {
                administrativo = administrativo.Where(e => e.Nombre.Contains(nombre));
            }

            if (!string.IsNullOrEmpty(cedula))
            {
                administrativo = administrativo.Where(e => e.Cedula.Contains(cedula));
            }

            return View(administrativo.ToList());
        }

        public ActionResult ListarHistorial()
        {
            var historial = _context.Historial_Aplicaciones
                .Include(h => h.ApplicationUser)
                .OrderByDescending(h => h.Fecha_Hora)
                .ToList();
            return View(historial);
        }

        public ActionResult CrearHistorial()
        {
            var usuarios = _context.Users.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.UserName
            }).ToList();

            ViewBag.Usuarios = new SelectList(usuarios, "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearHistorial(Historial_Aplicaciones historial)
        {
            if (ModelState.IsValid)
            {
                historial.Fecha_Hora = DateTime.Now;
                _context.Historial_Aplicaciones.Add(historial);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "El registro de historial se ha creado correctamente.";
                return RedirectToAction("ListarHistorial");
            }

            var usuarios = _context.Users.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.UserName
            }).ToList();

            ViewBag.Usuarios = new SelectList(usuarios, "Value", "Text", historial.Id);
            return View(historial);
        }

        public ActionResult EditarHistorial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var historial = _context.Historial_Aplicaciones.Find(id);
            if (historial == null)
            {
                return HttpNotFound();
            }

            ViewBag.Usuarios = new SelectList(_context.Users, "Id", "UserName", historial.Id);
            return View(historial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarHistorial(Historial_Aplicaciones historial)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(historial).State = EntityState.Modified;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "El registro de historial se ha actualizado correctamente.";
                return RedirectToAction("ListarHistorial");
            }

            ViewBag.Usuarios = new SelectList(_context.Users, "Id", "UserName", historial.Id);
            return View(historial);
        }

        public ActionResult EliminarHistorial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var historial = _context.Historial_Aplicaciones
                .Include(h => h.ApplicationUser)
                .FirstOrDefault(h => h.Id_Historial == id);

            if (historial == null)
            {
                return HttpNotFound();
            }

            return View(historial);
        }

        [HttpPost, ActionName("EliminarHistorial")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarHistorialConfirmado(int id)
        {
            var historial = _context.Historial_Aplicaciones.Find(id);
            if (historial != null)
            {
                _context.Historial_Aplicaciones.Remove(historial);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "El registro de historial se ha eliminado correctamente.";
            }
            return RedirectToAction("ListarHistorial");
        }

    }
}





