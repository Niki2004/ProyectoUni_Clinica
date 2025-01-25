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
        public int Id_Especialidad { get; set; }

        [Required]
        public int Id_Estado_Asistencia { get; set; }

        [StringLength(255)]
        public string Nota_medico { get; set; }

        public TimeSpan Horario_inicio { get; set; }

        public TimeSpan Horario_fin { get; set; }

        [StringLength(50)]
        public string Filtro_historial { get; set; }

        [StringLength(255)]
        public string Notificacion_enviada { get; set; }

        public int Total_Procedimiento { get; set; }

        [StringLength(255)]
        public string Motivo_cancelacion { get; set; }

        [StringLength(225)]
        public string Observaciones_Pacientes { get; set; }

        [StringLength(255)]
        public string Receta_aprobada { get; set; }

        [StringLength(255)]
        public string Detalle_receta { get; set; }

        [StringLength(255)]
        public string Motivo_modificacion { get; set; }

        public DateTime Fecha_modificacion { get; set; }

        public DateTime Fecha_creacion { get; set; } = DateTime.Now;

        // Llaves foraneas de las clases 
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; } // El campo Id en AspNetUsers es de tipo string.
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey(nameof(Id_Especialidad))]
        public virtual Especialidad Especialidad { get; set; }

        [ForeignKey(nameof(Id_Estado_Asistencia))]
        public virtual Estado_Asistencia EstadoAsistencia { get; set; }

        //Relacion con la tabla Cita
        public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

        public ICollection<Reporte> Reportes { get; set; } // Colección de Reportes relacionados


    }
}