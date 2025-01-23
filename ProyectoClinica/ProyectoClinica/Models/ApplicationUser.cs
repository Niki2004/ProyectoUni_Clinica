using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ProyectoClinica.Models
{
    public class ApplicationUser : IdentityUser
    {
        //Variables extra del login 
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public int Edad_Paciente { get; set; }

        public string Direccion { get; set; }

        public string Cedula { get; set; }

        public string Imagen { get; set; }

        // Relación con tablas 
        public ICollection<Administrativa> Administrativa { get; set; } = new List<Administrativa>();
        public ICollection<Medico> Medico { get; set; } = new List<Medico>();
        public ICollection<Paciente> Paciente { get; set; } = new List<Paciente>();
        public ICollection<Rol_Permiso> Rol_Permiso { get; set; } = new List<Rol_Permiso>();

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
           
            return userIdentity;
        }
    }
}