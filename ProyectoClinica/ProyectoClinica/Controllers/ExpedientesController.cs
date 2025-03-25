using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProyectoClinica.Controllers
{
    public class ExpedientesController : Controller
    {
        //Conexión BD
        private ApplicationDbContext BaseDatos = new ApplicationDbContext();
        // GET: Expedientes

        //[Authorize(Roles = "Administrador")]
        public ActionResult Expedientes()
        {
            return View();
        }

        //[Authorize(Roles = "Medico")]
        public ActionResult MdExpedientes()
        {
            return View();
        }

        //[Authorize(Roles = "Medico")]
        public ActionResult VistaHistorial()
        {
            var citas = BaseDatos.Cita.ToList();
            return View(citas);
        }


        //[Authorize(Roles = "Administrador")]
        public ActionResult VistaHistorialMd()
        {
            var citas = BaseDatos.Cita.ToList();
            return View(citas);
        }

        //[Authorize(Roles = "Administrador")]
        public ActionResult RolExpe()
        {
            ViewBag.Id_Empleado = new SelectList(BaseDatos.Empleado, "Id_Empleado", "Nombre");
            return View();
        }


        [HttpPost]
        //[Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public ActionResult RolExpe([Bind(Include = "Id_Empleado,Nombre")] RolAsignacion rolAsignacion)
        {
            if (ModelState.IsValid)
            {
                BaseDatos.RolAsignacion.Add(rolAsignacion);
                BaseDatos.SaveChanges();
                TempData["SuccessMessage"] = "Se Asigno el Rol Correctamente.";
                return RedirectToAction("Empleados/Empleados");
            }

            ViewBag.Id_Empleado = new SelectList(BaseDatos.Empleado, "Id_Empleado", "Nombre", rolAsignacion.Id_Empleado);
            return View(rolAsignacion);
        }




        public ActionResult drive()
        {
            return View();
        }





    }
}