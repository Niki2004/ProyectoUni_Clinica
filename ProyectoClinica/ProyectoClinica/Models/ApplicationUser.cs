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
        public ICollection<Rol_Permiso> Rol_Permiso { get; set; } = new List<Rol_Permiso>();
        public ICollection<Copago> Copago { get; set; } = new List<Copago>();
        public ICollection<Empleado> Empleado { get; set; } = new List<Empleado>();
        public ICollection<Reporte> Reporte { get; set; } = new List<Reporte>();



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
           
            return userIdentity;
        }
    }
}