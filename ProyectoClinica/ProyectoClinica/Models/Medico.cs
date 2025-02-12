using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Medico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Medico { get; set; }

        [Required]
        public int Id_Cita { get; set; }

        [Required]
        public int Id_receta { get; set; }

        [Required]
        public string Especialidad { get; set; }

        public TimeSpan Horario_fin { get; set; }

        [StringLength(255)]
        public string Nombre { get; set; }

        public TimeSpan Horario_inicio { get; set; }

        public DateTime Fecha_creacion { get; set; } = DateTime.Now;

        [Display(Name = "Foto")]
        [StringLength(255)]
        public string Imagen { get; set; }

        // Llaves foraneas de las clases 
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; } // El campo Id en AspNetUsers es de tipo string.
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey(nameof(Id_receta))]
        public virtual Receta Receta { get; set; }

        //Relacion con la tabla Cita
        public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

        public ICollection<Reporte> Reportes { get; set; } // Colección de Reportes relacionados


    }
}
