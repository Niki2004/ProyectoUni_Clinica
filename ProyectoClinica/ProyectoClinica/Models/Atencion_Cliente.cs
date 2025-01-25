using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Atencion_Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Atencion_Cliente { get; set; }

        [Required]
        public int Id_Clasificacion { get; set; }

        [Required]
        public int Id_Prioridad { get; set; }

        [Required]
        public int Id_Tipo_Servicio { get; set; }

        [Range(1, 10)]
        public int Promedio_servicio { get; set; }

        [StringLength(225)]
        public string Salud_Evaluada { get; set; }

        [StringLength(225)]
        public string Comentarios_Paciente { get; set; }

        public decimal Porcentaje_Pacientes_Satisfechos { get; set; }

        public DateTime Fechas_Comentario { get; set; }

        [StringLength(50)]
        public string Frecuencia_Comentarios { get; set; }

        // LLaves foraneas de las clases 
        [ForeignKey(nameof(Id_Clasificacion))]
        public virtual Clasificacion_Problema Clasificacion_Problema { get; set; }

        [ForeignKey(nameof(Id_Prioridad))]
        public virtual Prioridad_Mejora Prioridad_Mejora { get; set; }

        [ForeignKey(nameof(Id_Tipo_Servicio))]
        public virtual Tipo_Servicio Tipo_Servicio { get; set; }

        //Relación de la tabla Cita
        public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();
    }
}