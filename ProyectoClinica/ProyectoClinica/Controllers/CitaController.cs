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

        //Información de la clinica
        public ActionResult Especialidad()
        {
            return View();
        }

        //Información de la clinica
        public ActionResult Contactanos()
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
                // Verificar si la CITA ya está ocupada en el mismo horario
                var CitaExistente = BaseDatos.Cita
                    .FirstOrDefault(r =>
                        r.Id_Cita == Cita.Id_Cita &&
                        r.Fecha_Cita == Cita.Fecha_Cita &&
                        ((Cita.Hora_cita >= r.Hora_cita && Cita.Hora_cita < r.Hora_cita) ||
                         (Cita.Hora_cita > r.Hora_cita && Cita.Hora_cita <= r.Hora_cita) ||
                         (Cita.Hora_cita <= r.Hora_cita && Cita.Hora_cita >= r.Hora_cita)));

                if (CitaExistente != null)
                {
                    // Si la cita está ocupada, cambiar el estatus a "Pendiente"
                     Cita.Estado_Asistencia = "No Asistida";
                }
                else
                {
                    // Si la cita está disponible, marcar la reserva como "Aprobada"
                    Cita.Estado_Asistencia = "Asistida";
                }

                // Guardar la Cita
                BaseDatos.Cita.Add(Cita);
                BaseDatos.SaveChanges();

                // Redirigir a la vista de índice
                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, regresar la vista con los errores
            return View(Cita);
        }

        ////---------------------------------------------------- Editar ------------------------------------------------------------
        //[HttpGet]
        //public ActionResult Editar(int? id)
        //{
        //    if (id == null)
        //        return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

        //    var cita = BaseDatos.Cita.Find(id);
        //    if (cita == null)
        //        return HttpNotFound();

        //    ViewBag.Id_Medico = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre", cita.Id_Medico);
        //    ViewBag.Id_Atencion_Cliente = new SelectList(BaseDatos.Atencion_Cliente, "Id_Atencion_Cliente", "Nombre_Cliente", cita.Id_Atencion_Cliente);
        //    return View(cita);
        //}

        //[HttpPost]
        //public ActionResult Editar(Cita cita)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        BaseDatos.Entry(cita).State = EntityState.Modified;
        //        BaseDatos.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.Id_Medico = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre", cita.Id_Medico);
        //    ViewBag.Id_Atencion_Cliente = new SelectList(BaseDatos.Atencion_Cliente, "Id_Atencion_Cliente", "Nombre_Cliente", cita.Id_Atencion_Cliente);
        //    return View(cita);
        //}

        ////---------------------------------------------------- Eliminar ------------------------------------------------------------
        //[HttpGet]
        //public ActionResult Eliminar(int? id)
        //{
        //    if (id == null)
        //        return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

        //    var cita = BaseDatos.Cita.Find(id);
        //    if (cita == null)
        //        return HttpNotFound();

        //    return View(cita);
        //}

        //[HttpPost, ActionName("Eliminar")]
        //public ActionResult EliminarConfirmacion(int? id)
        //{
        //    if (id == null)
        //        return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

        //    var cita = BaseDatos.Cita.Find(id);
        //    if (cita == null)
        //        return HttpNotFound();

        //    BaseDatos.Cita.Remove(cita);
        //    BaseDatos.SaveChanges();
        //    return RedirectToAction("Index");
        //}
    }
}