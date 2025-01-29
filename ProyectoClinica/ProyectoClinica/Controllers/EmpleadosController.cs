using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoClinica.Controllers
{
    public class EmpleadosController : Controller
    {
        // GET: Empleados
        public ActionResult Empleados()
        {
            return View();
        }


        public ActionResult CrearEmpleado()
        {
            return View();
        }

        public ActionResult AsignacionRoles()
        {
            return View();
        }

        public ActionResult EditarEmpleado()
        {
            return View();
        }

        public ActionResult DesactivarPerfil()
        {
            return View();
        }

    }
}