using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class RolAsignacion
    {
        [Key]
        public int Id_Rol { get; set; }

        [Required]
        public int Id_Empleado { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [ForeignKey("Id_Empleado")]
        public Empleado Empleado { get; set; }
    }
}