using Microsoft.AspNet.Identity;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using ProyectoClinica.Models;

namespace ProyectoClinica.Controllers
{
    public class RedirectController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _context;

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public RedirectController()
        {
            _context = new ApplicationDbContext();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: Redirect

        public async Task<ActionResult> Redirect(String email)
        {
            if (!User.Identity.IsAuthenticated)
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }

            var user = await UserManager.FindByEmailAsync(email);

            if (_context.Recuperacion_Contra.Any(r => r.Id == user.Id)) {
                return RedirectToAction("ResetPassword", "Account", new { email = email });
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