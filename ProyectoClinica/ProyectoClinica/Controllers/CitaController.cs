using Microsoft.AspNet.Identity;
using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoClinica.Controllers
{
    public class CitaController : Controller
    {
        //Conexión BD
        private ApplicationDbContext BaseDatos = new ApplicationDbContext();


        // GET: Cita
        [Authorize(Roles = "Usuario")]
        public ActionResult Index()
        {
            var citas = BaseDatos.Cita.ToList();
            return View(citas);
        }


        public ActionResult Especialidad()
        {
            return View();
        }

        public ActionResult Contactanos()
        {
            return View();
        }

        //---------------------------------------------------- Obtener ------------------------------------------------------------
        [HttpGet]
        public ActionResult GetEvents()
        {
            // Obtén las citas con los pacientes y médicos a través de la tabla intersección Paciente_Cita
            var citas = BaseDatos.Paciente_Cita
                .Include("Cita") // Incluye las citas relacionadas
                .Include("Paciente") // Incluye los pacientes relacionados
                .Include("Cita.Medico") // Incluye los médicos relacionados con las citas
                .AsEnumerable()
                .Select(pc => new
                {
                    id = pc.Cita.Id_Cita, // ID de la cita
                    title = $"{pc.Cita.Tipo_Consulta} - {pc.Cita.Modalidad}",
                    start = pc.Cita.Fecha_Cita.ToString("yyyy-MM-dd") + "T" + pc.Cita.Hora_cita.ToString(@"hh\:mm\:ss"),
                    end = pc.Cita.Fecha_Cita.ToString("yyyy-MM-dd") + "T" + pc.Cita.Hora_cita.ToString(@"hh\:mm\:ss"), // Corregido aquí, agregué la conversión correcta
                    color = pc.Cita.Citas_Asistidas > 0
                             ? "green"
                             : (pc.Cita.Citas_No_Asistidas > 0 ? "red" : "orange"),
                    pacienteNombre = pc.Paciente.Nombre, // Nombre del paciente
                    medicoNombre = pc.Cita.Medico.Nombre, // Nombre del médico
                })
                .ToList();

            return Json(citas, JsonRequestBehavior.AllowGet);
        }




        //---------------------------------------------------- Crear ------------------------------------------------------------
        [HttpGet]
        public ActionResult Crear()
        {
            // Obtener médicos y sus datos
            var medicos = BaseDatos.Medico
                .Select(m => new
                {



                    Id_Medico = m.Id_Medico,
                    Nombre = m.Nombre
                })
                .ToList();

            // Obtener pacientes y sus datos
            var pacientes = BaseDatos.Paciente
                .Select(p => new
                {
                    Id_Paciente = p.Id_Paciente,
                    Nombre = p.Nombre
                })
                .ToList();

            // Pasar los datos a la vista
            ViewBag.Id_Medico = new SelectList(medicos, "Id_Medico", "Nombre");
            ViewBag.Id_Paciente = new SelectList(pacientes, "Id_Paciente", "Nombre");

            return View();
        }


        [HttpPost]
        public ActionResult Crear(Cita nuevaCita)
        {
            int idPaciente = Convert.ToInt32(Session["Id_Paciente"]);

            if (ModelState.IsValid)
            {
                // Calcular el final de la nueva cita
                TimeSpan duracion = TimeSpan.FromMinutes(30); // Suponiendo que cada cita dura 30 minutos
                var nuevaCitaHoraFin = nuevaCita.Hora_cita + duracion;

                // Validar conflictos en el horario de la cita
                var conflictoHorario = BaseDatos.Cita
                    .Any(c => c.Id_Medico == nuevaCita.Id_Medico &&
                              c.Fecha_Cita == nuevaCita.Fecha_Cita &&
                              (
                                  (nuevaCita.Hora_cita >= c.Hora_cita && nuevaCita.Hora_cita < DbFunctions.AddMinutes(c.Hora_cita, 30)) ||
                                  (nuevaCitaHoraFin > c.Hora_cita && nuevaCitaHoraFin <= DbFunctions.AddMinutes(c.Hora_cita, 30))
                              ));

                if (conflictoHorario)
                {
                    ModelState.AddModelError("", "El horario seleccionado tiene un conflicto con otra cita.");
                }
                else
                {
                    // Asignar estado inicial a la nueva cita
                    nuevaCita.Citas_Asistidas = 0;
                    nuevaCita.Citas_No_Asistidas = 0;

                    // Guardar la nueva cita
                    BaseDatos.Cita.Add(nuevaCita);
                    BaseDatos.SaveChanges(); // Guarda para obtener el Id_Cita generado

                    // Crear la relación en la tabla intersección Paciente_Cita
                    var pacienteCita = new Paciente_Cita
                    {
                        Id_Paciente = idPaciente,
                        Id_Cita = nuevaCita.Id_Cita // Usamos el ID generado automáticamente
                    };

                    BaseDatos.Paciente_Cita.Add(pacienteCita);
                    BaseDatos.SaveChanges(); // Guardar relación

                    return RedirectToAction("Index"); // Redirigir a la lista de citas
                }
            }

            // Si hay errores, recargar datos para la vista
            ViewBag.Id_Medico = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre", nuevaCita.Id_Medico);
            ViewBag.Id_Paciente = new SelectList(BaseDatos.Paciente, "Id_Paciente", "Nombre", idPaciente); // Aseguramos que el paciente esté preseleccionado

            return View(nuevaCita);
        }




        //---------------------------------------------------- Editar ------------------------------------------------------------
        [HttpGet]
        public ActionResult Editar(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var cita = BaseDatos.Cita.Find(id);
            if (cita == null)
                return HttpNotFound();

            ViewBag.Id_Medico = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre", cita.Id_Medico);
            ViewBag.Id_Atencion_Cliente = new SelectList(BaseDatos.Atencion_Cliente, "Id_Atencion_Cliente", "Nombre_Cliente", cita.Id_Atencion_Cliente);
            return View(cita);
        }

        [HttpPost]
        public ActionResult Editar(Cita cita)
        {
            if (ModelState.IsValid)
            {
                BaseDatos.Entry(cita).State = EntityState.Modified;
                BaseDatos.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Medico = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre", cita.Id_Medico);
            ViewBag.Id_Atencion_Cliente = new SelectList(BaseDatos.Atencion_Cliente, "Id_Atencion_Cliente", "Nombre_Cliente", cita.Id_Atencion_Cliente);
            return View(cita);
        }

        //---------------------------------------------------- Eliminar ------------------------------------------------------------
        [HttpGet]
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var cita = BaseDatos.Cita.Find(id);
            if (cita == null)
                return HttpNotFound();

            return View(cita);
        }

        [HttpPost, ActionName("Eliminar")]
        public ActionResult EliminarConfirmacion(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var cita = BaseDatos.Cita.Find(id);
            if (cita == null)
                return HttpNotFound();

            BaseDatos.Cita.Remove(cita);
            BaseDatos.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}