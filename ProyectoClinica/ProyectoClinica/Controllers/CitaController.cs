using Microsoft.AspNet.Identity;
using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoClinica.Controllers
{
    public class CitaController : Controller
    {
        //Conexión BD
        private ApplicationDbContext BaseDatos = new ApplicationDbContext();
       
        [Authorize(Roles = "Medico")]
        public ActionResult IndDOC()
        {
            return View();

        }

        [Authorize(Roles = "Medico")]
        public ActionResult VistaDOC()
        {
            return View();

        }

        [Authorize(Roles = "Administrador")]
        public ActionResult VistaCAdmin()
        {
            return View();

        }

        // GET: Cita
        [Authorize(Roles = "Usuario")]
        public ActionResult Index()
        {
            var citas = BaseDatos.Cita.ToList();
            return View(citas);
        }

        //Información de la clinica
        [Authorize(Roles = "Usuario")]
        public ActionResult VistaCita()
        {
            return View();
        }

        //Información de la clinica
        [Authorize(Roles = "Usuario")]
        public ActionResult Especialidad()
        {
            return View();
        }

        //Información de la clinica
        [Authorize(Roles = "Usuario")]
        public ActionResult Contactanos()
        {
            return View();
        }

        //Información de la clinica
        [Authorize(Roles = "Usuario")]
        public ActionResult Ubicacion()
        {
            return View();
        }
        
        //Me trae directamente la especialidad del doct
        [HttpGet]
        public JsonResult GetEspecialidadPorMedico(int idMedico)
        {
            var especialidad = BaseDatos.Medico
                .Where(m => m.Id_Medico == idMedico)
                .Select(m => m.Especialidad)
                .FirstOrDefault();

            if (especialidad != null)
            {
                return Json(new { success = true, especialidad }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Especialidad no encontrada." }, JsonRequestBehavior.AllowGet);
            }
        }

        //---------------------------------------------------- Obtener ------------------------------------------------------------
        [HttpGet]
        public ActionResult GetEvents()
        {
            var citas = BaseDatos.Cita
                .Include("Medico")
                .AsEnumerable()
                .Select(pc => new
                {
                    id = pc.Id_Cita,
                    title = pc.Medico.Nombre,
                    start = pc.Fecha_Cita.ToString("yyyy-MM-dd") + "T" + pc.Hora_cita.ToString(@"hh\:mm\:ss"),
                    end = pc.Fecha_Cita.ToString("yyyy-MM-dd") + "T" + pc.Hora_cita.ToString(@"hh\:mm\:ss"),
                    color = pc.Estado_Asistencia == "No Asistida" ? "red" : (pc.Estado_Asistencia == "Asistida" ? "green" : "orange"),
                    doctor = pc.Medico.Nombre,  // Añadido
                   
                })
                .ToList();
            return Json(citas, JsonRequestBehavior.AllowGet);
        }

        //---------------------------------------------------- Crear ------------------------------------------------------------
        [HttpGet]
        public ActionResult Crear()
        {

            ViewBag.Id_Medico = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre");
            return View();

        }

        [HttpPost]
        public ActionResult Crear(Cita Cita)
        {
            if (ModelState.IsValid)
            {
                var CitaExistente = BaseDatos.Cita
                .FirstOrDefault(r =>
                    r.Fecha_Cita == Cita.Fecha_Cita &&
                    r.Id_Medico == Cita.Id_Medico &&
                    (
                        (Cita.Hora_cita >= r.Hora_cita && Cita.Hora_cita < DbFunctions.AddMinutes(r.Hora_cita, 30)) ||
                        (Cita.Hora_cita <= r.Hora_cita && DbFunctions.AddMinutes(Cita.Hora_cita, 30) > r.Hora_cita)
                    ));

                string mensajeNotificacion;

                if (CitaExistente != null)
                {
                    Cita.Estado_Asistencia = "No Asistida";
                    mensajeNotificacion = "La cita se ha creado, pero hay un conflicto de horario. Buscar otro horario";
                }
                else
                {
                    Cita.Estado_Asistencia = "Asistida";
                    mensajeNotificacion = "Tu cita ha sido creada exitosamente";
                }

                // Guardar la Cita
                BaseDatos.Cita.Add(Cita);
                BaseDatos.SaveChanges();

                // Manejo de la sesión de notificaciones
                if (Session["Notificaciones"] == null)
                {
                    Session["Notificaciones"] = new List<string>();
                }

                var listaNotificaciones = (List<string>)Session["Notificaciones"];
                listaNotificaciones.Add(mensajeNotificacion);

                // Guardar el número total de notificaciones en la sesión
                Session["ContadorNotificaciones"] = listaNotificaciones.Count;

                // Redirigir a la vista de índice
                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, regresar la vista con los errores
            return View(Cita);
        }


        //---------------------------------------------------- Crear nota ----------------------------------------------------------
        [HttpGet]
        public ActionResult CrearNota()
        {
            var notaPaciente = new Nota_Paciente();

            return View(notaPaciente);
        }

        [HttpPost]
        public ActionResult CrearNota(Nota_Paciente notaPaciente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    BaseDatos.Nota_Paciente.Add(notaPaciente);
                    BaseDatos.SaveChanges();

                    // Agregar un mensaje de éxito
                    TempData["SuccessMessage"] = "La nota se ha creado correctamente.";

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al crear la nota del paciente: " + ex.Message);
                }
            }
            return View(notaPaciente);
        }

        //---------------------------------------------------- Gestión horario ------------------------------------------------------
        [HttpGet]
        public ActionResult GHorarios()
        {
            var Medico = BaseDatos.Medico.ToList();
            return View(Medico);
        }

        [HttpGet]
        public ActionResult HCita()
        {
            var Cita = BaseDatos.Cita.ToList();
            return View(Cita);
        }


        //Editar
        [HttpGet]
        public ActionResult EditarMedico(int id)
        {
            var medico = BaseDatos.Medico.Find(id);
            if (medico == null)
            {
                return HttpNotFound();
            }

            ViewBag.Id_Cita = new SelectList(BaseDatos.Cita, "Id_Cita", "Hora_cita");
            ViewBag.Id_receta = new SelectList(BaseDatos.Receta, "Id_receta", "Nombre_Receta");
            ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "Nombre");

            return View(medico);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarMedico(Medico medico)
        {
            if (ModelState.IsValid)
            {
                BaseDatos.Entry(medico).State = EntityState.Modified;
                BaseDatos.SaveChanges();
                return RedirectToAction("GHorarios");
            }

            ViewBag.Id_Cita = new SelectList(BaseDatos.Cita, "Id_Cita", "Hora_cita");
            ViewBag.Id_receta = new SelectList(BaseDatos.Receta, "Id_receta", "Nombre_Receta");
            ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "Nombre");

            return View(medico);
        }

        //---------------------------------------------------- Reprogramar horario ------------------------------------------------------
        [HttpGet]
        public ActionResult GetCita()
        {
            var citas = BaseDatos.Cita
                .Include("Medico")
                .AsEnumerable()
                .Select(pc => new
                {
                    id = pc.Id_Cita,
                    title = pc.Medico.Nombre,
                    start = pc.Fecha_Cita.ToString("yyyy-MM-dd") + "T" + pc.Hora_cita.ToString(@"hh\:mm\:ss"),
                    end = pc.Fecha_Cita.ToString("yyyy-MM-dd") + "T" + pc.Hora_cita.ToString(@"hh\:mm\:ss"),
                    color = pc.Estado_Asistencia == "No Asistida" ? "red" : (pc.Estado_Asistencia == "Asistida" ? "green" : "orange"),
                    doctor = pc.Medico.Nombre,  // Añadido

                })
                .ToList();
            return Json(citas, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PCitas()
        {
            var Medico = BaseDatos.Cita.ToList();
            return View(Medico);
        }

        [HttpGet]
        public ActionResult CrearADM()
        {

            ViewBag.Id_Medico = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre");
            return View();

        }

        [HttpPost]
        public ActionResult CrearADM(Cita Cita)
        {
            if (ModelState.IsValid)
            {
                var CitaExistente = BaseDatos.Cita
                .FirstOrDefault(r =>
                    r.Fecha_Cita == Cita.Fecha_Cita &&
                    r.Id_Medico == Cita.Id_Medico &&
                    (
                        (Cita.Hora_cita >= r.Hora_cita && Cita.Hora_cita < DbFunctions.AddMinutes(r.Hora_cita, 30)) ||
                        (Cita.Hora_cita <= r.Hora_cita && DbFunctions.AddMinutes(Cita.Hora_cita, 30) > r.Hora_cita)
                    ));

                string meNotificacion;

                if (CitaExistente != null)
                {
                    Cita.Estado_Asistencia = "No Asistida";
                    meNotificacion = "La cita se ha creado, pero hay un conflicto de horario. Buscar otro horario";
                }
                else
                {
                    Cita.Estado_Asistencia = "Asistida";
                    meNotificacion = "Tu cita ha sido creada exitosamente";
                
                }

                // Guardar la Cita
                BaseDatos.Cita.Add(Cita);
                BaseDatos.SaveChanges();

                // Manejo de la sesión de notificaciones
                if (Session["NotificacionesADM"] == null)
                {
                    Session["NotificacionesADM"] = new List<string>();
                }

                var liNotificaciones = (List<string>)Session["NotificacionesADM"];
                liNotificaciones.Add(meNotificacion);

                // Guardar el número total de notificaciones en la sesión
                Session["ContadorNotificaciones"] = liNotificaciones.Count;

                // Redirigir a la vista de índice
                return RedirectToAction("PCitas");
            }

            // Si el modelo no es válido, regresar la vista con los errores
            return View(Cita);
        }

        [HttpGet]
        public ActionResult CCita()
        {
            var cita = BaseDatos.Cita.ToList();
            return View(cita);
        }

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

            return RedirectToAction("CCita");
        }

        //---------------------------------------------------- Doctor ------------------------------------------------------
        [HttpGet]
        public ActionResult GetDOctCitas()
        {
            var citas = BaseDatos.Cita
                .Include("Medico")
                .AsEnumerable()
                .Select(pc => new
                {
                    id = pc.Id_Cita,
                    title = pc.Medico.Nombre,
                    start = pc.Fecha_Cita.ToString("yyyy-MM-dd") + "T" + pc.Hora_cita.ToString(@"hh\:mm\:ss"),
                    end = pc.Fecha_Cita.ToString("yyyy-MM-dd") + "T" + pc.Hora_cita.ToString(@"hh\:mm\:ss"),
                    color = pc.Estado_Asistencia == "No Asistida" ? "red" : (pc.Estado_Asistencia == "Asistida" ? "green" : "orange"),
                    doctor = pc.Medico.Nombre,  // Añadido

                })
                .ToList();
            return Json(citas, JsonRequestBehavior.AllowGet);
        }

        //---------------------------------------------------- Atención cliente ------------------------------------------------------
        [HttpGet]
        public ActionResult AtencionCliente()
        {
            var cita = BaseDatos.Atencion_Cliente.ToList();
            return View(cita);
        }

        [HttpGet]
        public ActionResult CrearAtencionCliente()
        {
            var crearAtencionCliente = new Atencion_Cliente();

            return View(crearAtencionCliente);
        }

        [HttpPost]
        public ActionResult CrearAtencionCliente(Atencion_Cliente atencionCliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    atencionCliente.Fechas_Comentario = DateTime.Now;
                    BaseDatos.Atencion_Cliente.Add(atencionCliente);
                    BaseDatos.SaveChanges();

                    return RedirectToAction("AtencionCliente");
                }

                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al crear el comentario: " + ex.Message);
                }
            }
            return View(atencionCliente);
        }

        //---------------------------------------------------- Atención cliente AD------------------------------------------------------
        [HttpGet]
        public ActionResult SAdComentarios()
        {
            var comentario = BaseDatos.Comentario.ToList();
            return View(comentario);
        }

        [HttpGet]
        public ActionResult SComentarios()
        {
            ViewBag.Id_Atencion_Cliente = new SelectList(BaseDatos.Atencion_Cliente, "Id_Atencion_Cliente", "Comentarios_Paciente");
            ViewBag.Id_Estado_Comentario = new SelectList(BaseDatos.Estado_Comentario, "Id_Estado_Comentario", "Estado");
            ViewBag.Id_Destacado_Comentario = new SelectList(BaseDatos.Destacado_Comentario, "Id_Destacado_Comentario", "Destacado");
            ViewBag.Id_Sensible_Comentario = new SelectList(BaseDatos.Sensible_Comentario, "Id_Sensible_Comentario", "Sensible");
            return View();
        }

        public ActionResult SComentarios(Comentario comentario)
        {
            if (ModelState.IsValid)
            {
                var comentarioExistente = BaseDatos.Comentario.FirstOrDefault(r =>
                     r.Id_Atencion_Cliente == comentario.Id_Atencion_Cliente &&
                     r.Id_Estado_Comentario == comentario.Id_Estado_Comentario &&
                     r.Id_Destacado_Comentario == comentario.Id_Destacado_Comentario &&
                     r.Id_Sensible_Comentario == comentario.Id_Sensible_Comentario);

                string mensajeNotificacion = "";

                if (comentarioExistente != null)
                {
                    var estadoComentario = BaseDatos.Estado_Comentario.FirstOrDefault(e => e.Id_Estado_Comentario == comentario.Id_Estado_Comentario);
                    var destacadoComentario = BaseDatos.Destacado_Comentario.FirstOrDefault(d => d.Id_Destacado_Comentario == comentario.Id_Destacado_Comentario);
                    var sensibleComentario = BaseDatos.Sensible_Comentario.FirstOrDefault(s => s.Id_Sensible_Comentario == comentario.Id_Sensible_Comentario);

                    if (estadoComentario != null)
                    {
                        mensajeNotificacion += ObtenerMensajePorEstado(estadoComentario.Estado);
                    }

                    if (destacadoComentario != null)
                    {
                        mensajeNotificacion += ObtenerMensajePorEstado(destacadoComentario.Destacado);
                    }

                    if (sensibleComentario != null)
                    {
                        mensajeNotificacion += ObtenerMensajePorEstado(sensibleComentario.Sensible);
                    }

                    if (string.IsNullOrEmpty(mensajeNotificacion))
                    {
                        mensajeNotificacion = "El estado del comentario no se encontró";
                    }
                }
                else
                {
                    BaseDatos.Comentario.Add(comentario);
                    BaseDatos.SaveChanges();
                    mensajeNotificacion = "Tu comentario ha sido creado exitosamente";
                }

                TempData["Mensaje"] = mensajeNotificacion;
                return RedirectToAction("SAdComentarios");
            }

            return View(comentario);
        }

        private string ObtenerMensajePorEstado(string estado)
        {
            switch (estado)
            {
                case "Rechazado":
                    return "El comentario ha sido rechazado. ";
                case "Pendiente":
                    return "El comentario está pendiente de revisión. ";
                case "Destacado":
                    return "El comentario ha sido marcado como destacado. ";
                default:
                    return "El comentario contiene información sensible. ";
            }
        }






    }
}