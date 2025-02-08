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
                return RedirectToAction("Empleados", "Empleados");

            if (User.IsInRole("Usuario"))
                return RedirectToAction("VistaCita", "Cita");

            if (User.IsInRole("Medico"))
                return RedirectToAction("Index", "Medico");

            if (User.IsInRole("Auditor"))
                return RedirectToAction("Index", "Auditor");

            if (User.IsInRole("Contador"))
                return RedirectToAction("Index", "Contador");

            if (User.IsInRole("Secretaria"))
                return RedirectToAction("Index", "Secretaria");

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            ModelState.AddModelError("", "El usuario no tiene ningun Rol");

            return RedirectToAction("Login", "Account"); ;
        }
    }
}