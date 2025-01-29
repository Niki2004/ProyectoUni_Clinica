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
        private ApplicationDbContext Context = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var usuario = await Context.Users.FirstAsync(u => u.Id == userId);

            var modelo = new IndexViewModel
            {
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Imagen = usuario.Imagen
            };

            return View(modelo);
        }
    }
}