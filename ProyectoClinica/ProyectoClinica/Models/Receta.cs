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

        [Display(Name = "Fecha de creación")]
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

        [StringLength(60)]
        [Display(Name = "Cantidad requerida")] 
        public string Cantidad_Requerida { get; set; }

        //Esta es para las detalles de la receta 
        [StringLength(255)]
        [Display(Name = "Motivo de la solicitud")]
        public string Motivo_Solicitud { get; set; }

        [Display(Name = "Imagen")]
        public string Imagen { get; set; }
    }
}