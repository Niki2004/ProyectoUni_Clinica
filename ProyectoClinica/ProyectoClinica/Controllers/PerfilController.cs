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

        //---------------------------------------------------- Vista de citas ------------------------------------------------------------
        public ActionResult VistaCita()
        {
            var citas = BaseDatos.Cita.ToList();
            return View(citas);
        }

        //---------------------------------------------------- Vista de citas ------------------------------------------------------------
        public ActionResult EliminarCita()
        {
            var citas = BaseDatos.Cita.ToList();
            return View(citas);
        }

        //---------------------------------------------------- Vista de citas ------------------------------------------------------------
        public ActionResult HistorialCita()
        {
            var citas = BaseDatos.Cita.ToList();
            return View(citas);
        }

        //---------------------------------------------------- Editar ------------------------------------------------------------
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

        [HttpPost]
        public ActionResult Editar(Cita cita)
        {
            if (ModelState.IsValid)
            {
                BaseDatos.Entry(cita).State = EntityState.Modified;
                BaseDatos.SaveChanges();
                return RedirectToAction("VistaCita");
            }

            ViewBag.IdMedico = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre", cita.Id_Medico);

            return View(cita);
        }

        //---------------------------------------------------- Eliminar ------------------------------------------------------------
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
            return RedirectToAction("EliminarCita");
        }

    }
}