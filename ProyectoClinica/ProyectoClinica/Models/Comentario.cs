using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Comentario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Comentario { get; set; }

        [ForeignKey("Atencion_Cliente")]
        public int Id_Atencion_Cliente { get; set; }

        [ForeignKey("Estado_Comentario")]
        public int Id_Estado_Comentario { get; set; }

        [ForeignKey("Destacado_Comentario")]
        public int Id_Destacado_Comentario { get; set; }

        [ForeignKey("Sensible_Comentario")]
        public int Id_Sensible_Comentario { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Comentario")]
        public string Comentario_Texto { get; set; }

        [Required]
        public int Calificacion { get; set; } // Calificación del 1 al 10

        public DateTime Fecha { get; set; } = DateTime.Now;

        public virtual Atencion_Cliente Atencion_Cliente { get; set; }
        public virtual Estado_Comentario Estado_Comentario { get; set; }
        public virtual Destacado_Comentario Destacado_Comentario { get; set; }
        public virtual Sensible_Comentario Sensible_Comentario { get; set; }


    }
}