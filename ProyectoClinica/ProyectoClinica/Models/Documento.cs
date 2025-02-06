using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Documento
    {
        [Key]
        public int Id_Documento { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        public string Informacion { get; set; }

        [Required]
        public int Id_Empleado { get; set; }

        [ForeignKey("Id_Empleado")]
        public Empleado Empleado { get; set; }
    }
}