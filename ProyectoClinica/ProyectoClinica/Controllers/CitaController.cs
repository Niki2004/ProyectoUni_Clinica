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
            var citas = BaseDatos.Cita
                .Include("Medico")
                .Include("Atencion_Cliente")
                .AsEnumerable()
                .Select(c => new
                {
                    id = c.Id_Cita,
                    title = $"{c.Tipo_Tratamiento} - {c.Modalidad}",
                    start = c.Fecha_Cita.ToString("yyyy-MM-dd") + "T" + c.Hora_cita.ToString(@"hh\:mm\:ss"),
                    end = c.Fecha_Cita.ToString("yyyy-MM-dd") + "T" + c.Hora_cita.Add(TimeSpan.Parse(c.Duracion_Tratamiento)).ToString(@"hh\:mm\:ss"),
                    color = c.Citas_Asistidas > 0 ? "green" : (c.Citas_No_Asistidas > 0 ? "red" : "orange")
                })
                .ToList();

            return Json(citas, JsonRequestBehavior.AllowGet);
        }



        //---------------------------------------------------- Crear ------------------------------------------------------------
        [HttpGet]
        public ActionResult Crear()
        {
            var medicos = BaseDatos.Medico.Include(m => m.ApplicationUser) 
                           .Select(m => new
                           { m.Id, Nombre = m.ApplicationUser.Nombre }).ToList();

            var userId = User.Identity.GetUserId();

            var paciente = BaseDatos.Paciente
                    .Include(p => p.ApplicationUser) 
                    .FirstOrDefault(p => p.Id == userId);

        if (paciente == null)
            {
                return HttpNotFound();
            }

            // Preparar la lista de pacientes para el DropDownList
            var pacientes = BaseDatos.Paciente
                              .Include(p => p.ApplicationUser) // Asegura que los datos del usuario estén disponibles
                              .Select(p => new
                              {
                                  p.Id_Paciente,
                                  NombrePaciente = p.ApplicationUser.Nombre // Suponiendo que "Nombre" es un campo en ApplicationUser
                              })
                              .ToList();

            ViewBag.Id_Medico = new SelectList(medicos, "Id", "Nombre");
            ViewBag.Id_Paciente = new SelectList(pacientes, "Id_Paciente", "NombrePaciente");
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Cita cita)
        {
            if (ModelState.IsValid)
            {
                // Validar si hay conflicto en el horario
                var citaExistente = BaseDatos.Cita
                    .FirstOrDefault(c =>
                        c.Id_Medico == cita.Id_Medico &&
                        c.Fecha_Cita == cita.Fecha_Cita &&
                        ((cita.Hora_cita >= c.Hora_cita && cita.Hora_cita < c.Hora_cita.Add(TimeSpan.Parse(c.Duracion_Tratamiento))) ||
                         (cita.Hora_cita.Add(TimeSpan.Parse(c.Duracion_Tratamiento)) > c.Hora_cita && cita.Hora_cita.Add(TimeSpan.Parse(c.Duracion_Tratamiento)) <= c.Hora_cita.Add(TimeSpan.Parse(c.Duracion_Tratamiento)))));

                if (citaExistente != null)
                {
                    cita.Citas_No_Asistidas = 1; 
                }
                else
                {
                    cita.Citas_Asistidas = 1; 
                }

                BaseDatos.Cita.Add(cita);
                BaseDatos.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Medico = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre", cita.Id_Medico);
            ViewBag.Id_Atencion_Cliente = new SelectList(BaseDatos.Atencion_Cliente, "Id_Atencion_Cliente", "Nombre_Cliente", cita.Id_Atencion_Cliente);
            return View(cita);
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