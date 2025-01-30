using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Receta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_receta { get; set; }

        public DateTime Fecha_Creacion { get; set; } = DateTime.Now;

        [StringLength(255)]
        [Display(Name = "Nombre de la receta")]
        public string Nombre_Receta { get; set; }

        [StringLength(255)]
        [Display(Name = "Observaciones de pacientes")]
        public string Observaciones_Pacientes { get; set; }

        [StringLength(255)]
        [Display(Name = "Duracion del tratamiento")]
        public string Duracion_Tratamiento { get; set; }
    }
}