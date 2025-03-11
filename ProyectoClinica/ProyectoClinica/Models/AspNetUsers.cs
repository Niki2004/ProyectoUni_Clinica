using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class AspNetUsers
    {
        [Key]
        [Column("Id")]
        [StringLength(128)]
        public string Id { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("Apellido")]
        public string Apellido { get; set; }

        [Column("Edad_Paciente")]
        public int EdadPaciente { get; set; }

        [Column("Genero_Paciente")]
        public string GeneroPaciente { get; set; }

        [Column("Direccion")]
        public string Direccion { get; set; }

        [Column("Cedula")]
        public string Cedula { get; set; }

        [Column("Imagen")]
        public string Imagen { get; set; }

        [Column("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Column("Email")]
        [StringLength(256)]
        public string Email { get; set; }

        [Column("EmailConfirmed")]
        [Required]
        public bool EmailConfirmed { get; set; }

        [Column("PasswordHash")]
        public string PasswordHash { get; set; }

        [Column("SecurityStamp")]
        public string SecurityStamp { get; set; }

        [Column("PhoneNumberConfirmed")]
        [Required]
        public bool PhoneNumberConfirmed { get; set; }

        [Column("TwoFactorEnabled")]
        [Required]
        public bool TwoFactorEnabled { get; set; }

        [Column("LockoutEndDateUtc")]
        public DateTime? LockoutEndDateUtc { get; set; }

        [Column("LockoutEnabled")]
        [Required]
        public bool LockoutEnabled { get; set; }

        [Column("AccessFailedCount")]
        [Required]
        public int AccessFailedCount { get; set; }

        [Column("UserName")]
        [StringLength(256)]
        [Required]
        public string UserName { get; set; }
    }
}
