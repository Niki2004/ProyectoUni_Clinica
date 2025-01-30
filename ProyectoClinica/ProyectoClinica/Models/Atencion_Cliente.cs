using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace ProyectoClinica.Models
{
    public class Atencion_Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Atencion_Cliente { get; set; }

        [StringLength(225)]
        [Display(Name = "Área de salud")]
        public string Salud_Evaluada { get; set; }

        [StringLength(225)]
        [Display(Name = "Comentario del paciente")]
        public string Comentarios_Paciente { get; set; }

        [StringLength(225)]
        [Display(Name = "Prioridad de mejora")]
        public string Prioridad_Mejora { get; set; }

        public DateTime Fechas_Comentario { get; set; }

        [StringLength(225)]
        [Display(Name = "Tipo de servicio")]
        public string Tipo_Servicio { get; set; }

        [Display(Name = "Clasificacion del problema")]
        public int Clasificacion_Problema { get; set; } //Del 1 al 10 

    }
}