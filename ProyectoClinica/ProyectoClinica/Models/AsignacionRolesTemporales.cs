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

        public DateTime Fecha_Inicio { get; set; }

        public DateTime Fecha_Fin { get; set; }

        [StringLength(50)]
        public string Estado { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}