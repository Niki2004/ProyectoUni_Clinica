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

        public string Genero_Paciente { get; set; }

        public string Direccion { get; set; }

        public string Cedula { get; set; }

        public string Imagen { get; set; }

        public string PhoneNumber { get; set; }

        // Relación con tablas 
        public ICollection<Administrativa> Administrativa { get; set; } = new List<Administrativa>();
        public ICollection<Medico> Medico { get; set; } = new List<Medico>();
        public ICollection<Rol_Permiso> Rol_Permiso { get; set; } = new List<Rol_Permiso>();
        public ICollection<Copago> Copago { get; set; } = new List<Copago>();
        public ICollection<Reporte> Reporte { get; set; } = new List<Reporte>();
        public ICollection<AsignacionRolesTemporales> AsignacionRolesTemporales { get; set; } = new List<AsignacionRolesTemporales>();

        public ICollection<UsuariosAdmin> UsuariosAdmin { get; set; } = new List<UsuariosAdmin>();


        public ICollection<Historial_Aplicaciones> Historial_Aplicaciones { get; set; } = new List<Historial_Aplicaciones>();

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
           
            return userIdentity;
        }
    }
}