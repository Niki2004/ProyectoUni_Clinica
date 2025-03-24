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
            if (!User.Identity.IsAuthenticated)
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }

            if (User.IsInRole("Administrador"))
                return RedirectToAction("VistaAdmin", "Empleados");

            else if (User.IsInRole("Medico"))
                return RedirectToAction("IndDOC", "Cita");

           else if (User.IsInRole("Auditor"))
                return RedirectToAction("VistaAUD", "Contabilidad");

            else if (User.IsInRole("Contador"))
                return RedirectToAction("VistaCONTA", "Contabilidad");

            else if (User.IsInRole("Secretaria"))
                return RedirectToAction("VistaSEC", "Factura");

            else  if (User.IsInRole("Usuario"))
                return RedirectToAction("VistaCita", "Cita");

          
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            ModelState.AddModelError("", "El usuario no tiene un rol válido");
            return RedirectToAction("Login", "Account");
        }
    }
}