using Microsoft.AspNet.Identity;
using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProyectoClinica.Controllers
{
    public class PerfilController : Controller
    {
        private ApplicationDbContext BaseDatos = new ApplicationDbContext();

        //---------------------------------------------------- Vista que jala el nombre... ------------------------------------------------------------
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var usuario = await BaseDatos.Users.FirstAsync(u => u.Id == userId);

            var modelo = new IndexViewModel
            {
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Imagen = usuario.Imagen
            };

            return View(modelo);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> PerfilADM()
        {
            var userId = User.Identity.GetUserId();
            var usuario = await BaseDatos.Users.FirstAsync(u => u.Id == userId);

            var modelo = new IndexViewModel
            {
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Imagen = usuario.Imagen
            };

            return View(modelo);
        }

        [Authorize(Roles = "Medico")]
        public async Task<ActionResult> PerfilMED()
        {
            var userId = User.Identity.GetUserId();
            var usuario = await BaseDatos.Users.FirstAsync(u => u.Id == userId);

            var modelo = new IndexViewModel
            {
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Imagen = usuario.Imagen
            };

            return View(modelo);
        }

        [Authorize(Roles = "Auditor")]
        public async Task<ActionResult> PerfilAUD()
        {
            var userId = User.Identity.GetUserId();
            var usuario = await BaseDatos.Users.FirstAsync(u => u.Id == userId);

            var modelo = new IndexViewModel
            {
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Imagen = usuario.Imagen
            };

            return View(modelo);
        }

        [Authorize(Roles = "Contador")]
        public async Task<ActionResult> PerfilCON()
        {
            var userId = User.Identity.GetUserId();
            var usuario = await BaseDatos.Users.FirstAsync(u => u.Id == userId);

            var modelo = new IndexViewModel
            {
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Imagen = usuario.Imagen
            };

            return View(modelo);
        }

        [Authorize(Roles = "Secretaria")]
        public async Task<ActionResult> PerfilSEC()
        {
            var userId = User.Identity.GetUserId();
            var usuario = await BaseDatos.Users.FirstAsync(u => u.Id == userId);

            var modelo = new IndexViewModel
            {
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Imagen = usuario.Imagen
            };

            return View(modelo);
        }

        //---------------------------------------------------- Vista de citas ------------------------------------------------------------
        [Authorize(Roles = "Usuario")]
        public ActionResult VistaCita()
        {
            var userId = User.Identity.GetUserId(); // Obtener ID del usuario logueado

            var citasDelUsuario = BaseDatos.Cita
                .Include("Medico")
                .Include("ApplicationUser") // Opcional: solo si necesitas mostrar nombre de usuario, etc.
                .Where(c => c.ApplicationUser.Id == userId)
                .ToList();

            return View(citasDelUsuario);
        }

        //---------------------------------------------------- Vista de citas ------------------------------------------------------------
        [Authorize(Roles = "Usuario")]
        public ActionResult EliminarCita()
        {
            var userId = User.Identity.GetUserId(); // Obtener ID del usuario logueado

            var citasDelUsuario = BaseDatos.Cita
                .Include("Medico")
                .Include("ApplicationUser") // Opcional: solo si necesitas mostrar nombre de usuario, etc.
                .Where(c => c.ApplicationUser.Id == userId)
                .ToList();

            return View(citasDelUsuario);
        }

        //---------------------------------------------------- Vista de citas ------------------------------------------------------------
        [Authorize(Roles = "Usuario")]
        public ActionResult HistorialCita()
        {
            var userId = User.Identity.GetUserId(); // Obtener ID del usuario logueado

            var citasDelUsuario = BaseDatos.Cita
                .Include("Medico")
                .Include("ApplicationUser") // Opcional: solo si necesitas mostrar nombre de usuario, etc.
                .Where(c => c.ApplicationUser.Id == userId)
                .ToList();

            return View(citasDelUsuario);
        }

        //---------------------------------------------------- Vista de citas ------------------------------------------------------------
        [Authorize(Roles = "Usuario")]
        public ActionResult NotasPersonale()
        {
            var userId = User.Identity.GetUserId();

            var notasPersonales = BaseDatos.Nota_Paciente
                .Where(n => n.ApplicationUser.Id == userId)
                .ToList();

            return View(notasPersonales);
        }

        //---------------------------------------------------- Editar cita ------------------------------------------------------------
        [Authorize(Roles = "Usuario")]
        public ActionResult Editar(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var cita = BaseDatos.Cita.Find(id);
            if (cita == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdMedico = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre", cita.Id_Medico);
            return View(cita);
        }

        [Authorize(Roles = "Usuario")]
        [HttpPost]
        public ActionResult Editar(Cita cita)
        {
            if (cita.Fecha_Cita < DateTime.Today)
            {
                ModelState.AddModelError("Fecha_Cita", "No se puede agendar una cita en una fecha pasada.");
            }

            var diaSemana = (int)cita.Fecha_Cita.DayOfWeek; // 0 = Domingo, 6 = Sábado
            if (diaSemana == 0 || diaSemana == 6)
            {
                ModelState.AddModelError("Fecha_Cita", "No se pueden agendar citas los sábados ni domingos.");
            }

            if (cita.Fecha_Cita == DateTime.Today && cita.Hora_cita < DateTime.Now.TimeOfDay)
            {
                ModelState.AddModelError("Hora_cita", "No se puede agendar una cita en una hora pasada.");
            }

            if (cita.Hora_cita < TimeSpan.FromHours(7) || cita.Hora_cita > TimeSpan.FromHours(20))
            {
                ModelState.AddModelError("Hora_cita", "La hora debe estar entre las 07:00 y las 20:00.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.IdMedico = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre", cita.Id_Medico);
                return View(cita);
            }

            var citaExistente = BaseDatos.Cita.Find(cita.Id_Cita);

            if (citaExistente == null)
            {
                return HttpNotFound();
            }

            // Verificar si la cita se está intentando modificar el mismo día
            if (citaExistente.Fecha_Cita.Date == DateTime.Now.Date)
            {
                TempData["ErrorMessage"] = "No es posible modificar una cita el mismo día. Por favor, cancele y agende una nueva.";
                return RedirectToAction("VistaCita");
            }

            // Actualizar manualmente los valores en la entidad existente
            citaExistente.Id_Medico = cita.Id_Medico;
            citaExistente.Fecha_Cita = cita.Fecha_Cita;
            citaExistente.Hora_cita = cita.Hora_cita;
            citaExistente.Modalidad = cita.Modalidad;
            citaExistente.Sintomas = cita.Sintomas;
            citaExistente.Descripcion_Complicaciones = cita.Descripcion_Complicaciones;
            citaExistente.Estado_Asistencia = cita.Estado_Asistencia;
            //El nombre no se modifica ya que es el nombre del paciente

            // Marcar la entidad como modificada
            BaseDatos.Entry(citaExistente).State = EntityState.Modified;

            // Guardar cambios
            BaseDatos.SaveChanges();

            TempData["SuccessMessage"] = "La cita se ha actualizado correctamente.";
            return RedirectToAction("VistaCita");
        }

        //---------------------------------------------------- Eliminar cita ------------------------------------------------------------
        [Authorize(Roles = "Usuario")]
        [HttpGet]
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var cita = BaseDatos.Cita.SingleOrDefault(l => l.Id_Cita == id);
            if (cita == null)
                return HttpNotFound();

            return View(cita);
        }

        [Authorize(Roles = "Usuario")]
        [HttpPost, ActionName("Eliminar")]
        public ActionResult EliminarCitaUsuario(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var cita = BaseDatos.Cita.Find(id);
            if (cita == null)
                return HttpNotFound();

            BaseDatos.Cita.Remove(cita);
            BaseDatos.SaveChanges();

            // Agregar mensaje de éxito
            TempData["SuccessMessage"] = "La cita se ha eliminado correctamente.";

            return RedirectToAction("EliminarCita");
        }

        //---------------------------------------------------- Editar Nota ------------------------------------------------------------
        [Authorize(Roles = "Usuario")]
        [HttpGet]
        public ActionResult EditarNota(int id)
        {
            var notaPaciente = BaseDatos.Nota_Paciente.Find(id);
            if (notaPaciente == null)
            {
                return HttpNotFound();
            }

            return View(notaPaciente);
        }

        [Authorize(Roles = "Usuario")]
        [HttpPost]
        public ActionResult EditarNota(Nota_Paciente notaPaciente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    BaseDatos.Entry(notaPaciente).State = EntityState.Modified;
                    BaseDatos.SaveChanges();

                    // Agregar mensaje de éxito
                    TempData["SuccessMessage"] = "La nota se ha actualizado correctamente.";

                    return RedirectToAction("NotasPersonale");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al actualizar la nota: " + ex.Message);
                }
            }

            return View(notaPaciente);
        }

        //---------------------------------------------------- Eliminar Nota ------------------------------------------------------------
        [Authorize(Roles = "Usuario")]
        [HttpGet]
        public ActionResult EliminarNota(int id)
        {
            var notaPaciente = BaseDatos.Nota_Paciente.Find(id);
            if (notaPaciente == null)
            {
                return HttpNotFound();
            }

            return View(notaPaciente);
        }

        [Authorize(Roles = "Usuario")]
        [HttpPost, ActionName("EliminarNota")]
        public ActionResult ConfirmarEliminarNota(int id)
        {
            try
            {
                var notaPaciente = BaseDatos.Nota_Paciente.Find(id);
                if (notaPaciente == null)
                {
                    return HttpNotFound();
                }

                BaseDatos.Nota_Paciente.Remove(notaPaciente);
                BaseDatos.SaveChanges();

                // Agregar mensaje de éxito
                TempData["SuccessMessage"] = "La nota se ha eliminado correctamente.";

                return RedirectToAction("NotasPersonale");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar la nota: " + ex.Message);
                return RedirectToAction("EliminarNota", new { id });
            }
        }

        //---------------------------------------------------- Notificacion ------------------------------------------------------------
        [Authorize(Roles = "Usuario")]
        public ActionResult Notificaciones()
        {
            var notificaciones = Session["Notificaciones"] as List<string> ?? new List<string>();

            // Limpiar el contador después de que el usuario vea las notificaciones
            Session["ContadorNotificaciones"] = 0;

            return View(notificaciones);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult NotificacionesADM()
        {
            var notificaciones = Session["NotificacionesADM"] as List<string> ?? new List<string>();

            // Limpiar el contador después de que el usuario vea las notificaciones
            Session["ContadorNotificaciones"] = 0;

            return View(notificaciones);
        }

        //---------------------------------------------------- Medico ------------------------------------------------------------
        [Authorize(Roles = "Medico")]
        [HttpGet]
        public ActionResult DOCHCita()
        {
            var Medico = BaseDatos.Cita.ToList();
            return View(Medico);
        }

        public ActionResult FiltrarCitas(string tipoConsulta)
        {
            try
            {
                var citas = BaseDatos.Cita.Include(c => c.Medico).AsQueryable();

                if (!string.IsNullOrEmpty(tipoConsulta))
                {
                    citas = citas.Where(c => c.Modalidad.ToLower() == tipoConsulta.ToLower());
                }

                var filteredCitas = citas.ToList().Select(c => new
                {
                    NombreMedico = c.Medico != null ? c.Medico.Nombre : "",
                    c.Nombre_Paciente,
                    c.Estado_Asistencia,
                    Hora_cita = c.Hora_cita.ToString(@"hh\:mm"),
                    c.Descripcion_Complicaciones,
                    c.Sintomas,
                    Fecha_Cita = c.Fecha_Cita.ToString("dd/MM/yyyy"),
                    c.Modalidad
                });

                return Json(filteredCitas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Medico")]
        [HttpGet]
        public ActionResult NotasAdicionales()
        {
            var Medico = BaseDatos.Nota_Paciente.ToList();
            return View(Medico);
        }

        [Authorize(Roles = "Medico")]
        public ActionResult ConcenReceta()
        {
            var Receta = BaseDatos.Receta.ToList();
            return View(Receta);
        }

        [Authorize(Roles = "Medico")]
        public ActionResult RecetaDOC()
        {
            ViewBag.Id_Receta = new SelectList(BaseDatos.Receta, "Id_receta", "Nombre_Receta");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecetaDOC(Modificacion_Receta modificacion_receta)
        {

            if (modificacion_receta.Fecha_Modificacion < DateTime.Today)
            {
                ModelState.AddModelError("Fecha_Cita", "No se puede agendar una cita en una fecha pasada.");
            }

            var diaSemana = (int)modificacion_receta.Fecha_Modificacion.DayOfWeek; // 0 = Domingo, 6 = Sábado
            if (diaSemana == 0 || diaSemana == 6)
            {
                ModelState.AddModelError("Fecha_Cita", "No se pueden agendar citas los sábados ni domingos.");
            }

            if (ModelState.IsValid)
            {
                // Crear una nueva modificación de receta basada en la receta seleccionada
                BaseDatos.Modificacion_Receta.Add(modificacion_receta);

                // Guardar los cambios en la base de datos
                BaseDatos.SaveChanges();

                // Redirigir a la vista de la lista de recetas
                return RedirectToAction("ConcenReceta");
            }
            ViewBag.Id_Receta = new SelectList(BaseDatos.Receta, "Id_receta", "Nombre_Receta");
            // Si hay errores, regresar con el modelo para mostrar los errores de validación
            return View(modificacion_receta);
        }

        //---------------------------------------------------- Receta ------------------------------------------------------
        [Authorize(Roles = "Usuario")]
        public ActionResult CrearReceta()
        {
            return View();
        }

        [Authorize(Roles = "Usuario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearReceta(Receta receta)
        {
            if (ModelState.IsValid)
            {
                BaseDatos.Receta.Add(receta);
                BaseDatos.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(receta);
        }

        [Authorize(Roles = "Usuario")]
        public ActionResult IndexReceta()
        {
            var viewModel = new RecetaViewModel
            {
                Recetas = BaseDatos.Receta.ToList(),
                Modificaciones = BaseDatos.Modificacion_Receta.ToList()
            };

            return View(viewModel);
        }

        //---------------------------------------------------- Nota ------------------------------------------------------
        [Authorize(Roles = "Medico")]
        public ActionResult NotasMedicas()
        {
            var citas = BaseDatos.Nota_Medico.ToList();
            return View(citas);
        }

        [Authorize(Roles = "Medico")]
        [HttpGet]
        public ActionResult Nota_Medico()
        {
            return View();
        }

        [Authorize(Roles = "Medico")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Nota_Medico(Nota_Medico nota_medico)
        {
            if (ModelState.IsValid)
            {
                BaseDatos.Nota_Medico.Add(nota_medico);
                BaseDatos.SaveChangesAsync();

                TempData["SuccessMessage"] = "La nota se ha creado correctamente.";

                return RedirectToAction("DOCHCita");
            }

            return View(nota_medico);
        }

        [Authorize(Roles = "Medico")]
        [HttpGet]
        public ActionResult EditarNotaMedico(int id)
        {
            var NotaMedico = BaseDatos.Nota_Medico.Find(id);
            if (NotaMedico == null)
            {
                return HttpNotFound();
            }

            return View(NotaMedico);
        }

        [Authorize(Roles = "Medico")]
        [HttpPost]
        public ActionResult EditarNotaMedico(Nota_Medico nota_medico)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    BaseDatos.Entry(nota_medico).State = EntityState.Modified;
                    BaseDatos.SaveChanges();

                    // Agregar mensaje de éxito
                    TempData["SuccessMessage"] = "La nota se ha actualizado correctamente.";

                    return RedirectToAction("NotasMedicas");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al actualizar la nota: " + ex.Message);
                }
            }

            return View(nota_medico);
        }

        [Authorize(Roles = "Medico")]
        [HttpGet]
        public ActionResult EliminarNotaMedico(int id)
        {
            var notaMedico = BaseDatos.Nota_Medico.Find(id);
            if (notaMedico == null)
            {
                return HttpNotFound();
            }

            return View(notaMedico);
        }

        [HttpPost, ActionName("EliminarNotaMedico")]
        public ActionResult ConfirmarEliminarNotaMedico(int id)
        {
            try
            {
                var notaMedico = BaseDatos.Nota_Medico.Find(id);
                if (notaMedico == null)
                {
                    return HttpNotFound();
                }

                BaseDatos.Nota_Medico.Remove(notaMedico);
                BaseDatos.SaveChanges();

                // Agregar mensaje de éxito
                TempData["SuccessMessage"] = "La nota se ha eliminado correctamente.";

                return RedirectToAction("NotasMedicas");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar la nota: " + ex.Message);
                return RedirectToAction("EliminarNotaMedico", new { id });
            }
        }

        //---------------------------------------------------- Vista de citas F ------------------------------------------------------------
        [Authorize(Roles = "Usuario")]
        public ActionResult VistaCitaFutura()
        {
            var userId = User.Identity.GetUserId(); // Obtener ID del usuario autenticado

            var citasFuturas = BaseDatos.Cita
                .Where(c => c.Fecha_Cita > DateTime.Now && c.ApplicationUser.Id == userId)
                .ToList();

            return View(citasFuturas);
        }



    }
}