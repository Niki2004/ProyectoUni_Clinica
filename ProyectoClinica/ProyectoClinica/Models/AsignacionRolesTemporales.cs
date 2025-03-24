using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class AsignacionRolesTemporales
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_AsignacionRoles { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        [Required]
        [ForeignKey("UsuarioAsignado")]
        public string Id_Usuario { get; set; }

        [Required]
        [ForeignKey("Departamentos")]
        public int Id_Departamento { get; set; }

        public DateTime Fecha_Inicio { get; set; }

        public DateTime Fecha_Fin { get; set; }

        [StringLength(50)]
        public string Estado { get; set; }

        public string Motivo { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ApplicationUser UsuarioAsignado { get; set; }

        public virtual Departamentos Departamentos { get; set; }
    }
}