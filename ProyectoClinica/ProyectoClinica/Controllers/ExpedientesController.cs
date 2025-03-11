using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoClinica.Controllers
{
    public class ExpedientesController : Controller
    {
        //Conexión BD
        private ApplicationDbContext BaseDatos = new ApplicationDbContext();
        // GET: Expedientes

        public ActionResult Expedientes()
        {
            return View();
        }

        public ActionResult VistaHistorial()
        {
            var citas = BaseDatos.Cita.ToList();
            return View(citas);
        }

    }
}