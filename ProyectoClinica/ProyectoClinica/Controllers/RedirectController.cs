using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProyectoClinica.Controllers
{
    public class RedirectController : Controller
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // GET: Redirect
        public ActionResult Redirect()
        {

            if(User.IsInRole("Administrador"))
                return RedirectToAction("VistaAdmin", "Empleados");  //Lista la vista de empleados

            if (User.IsInRole("Usuario"))
                return RedirectToAction("VistaCita", "Cita"); //Lista la vista de usuarios

            if (User.IsInRole("Medico"))
                return RedirectToAction("IndDOC", "Cita"); //Lista la vista de medicos

            if (User.IsInRole("Auditor"))
                return RedirectToAction("VistaAUD", "Auditor"); 

            if (User.IsInRole("Contador"))
                return RedirectToAction("VistaCONTA", "Contador");

            if (User.IsInRole("Secretaria"))
                return RedirectToAction("VistaSEC", "Factura");  //Lista la vista de secretarias

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            ModelState.AddModelError("", "El usuario no tiene ningun Rol");

            return RedirectToAction("Login", "Account"); ;
        }
    }
}