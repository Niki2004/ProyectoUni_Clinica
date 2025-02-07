using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Evaluacion
    {
        [Key]
        public int Id_Evaluacion { get; set; }

        [Required]
        public DateTime Fecha_Evaluacion { get; set; }

        
        public decimal Calificacion { get; set; }

        public string Comentarios { get; set; }

        [Required]
        public int Id_Empleado { get; set; }

        [ForeignKey("Id_Empleado")]
        public Empleado Empleado { get; set; }
    }
}