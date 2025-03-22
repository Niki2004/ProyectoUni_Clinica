using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
	public class UsuariosAdmin
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Usuario { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        [StringLength(100)]
        public string Departamento { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime FechaAlta { get; set; } = DateTime.Now;

        public int? NivelAcceso { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}