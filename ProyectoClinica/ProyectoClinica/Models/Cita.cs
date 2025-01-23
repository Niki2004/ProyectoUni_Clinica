using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Cita
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Cita { get; set; }

        [Required]
        public int Id_Medico { get; set; }

        public int Id_Atencion_Cliente { get; set; }

        [Required]
        public TimeSpan Hora_cita { get; set; }

        [StringLength(255)]
        public string Duracion_Tratamiento { get; set; }

        public int Total_Citas { get; set; }

        public int Cantidad_Pacientes { get; set; }

        [Required]
        public DateTime Fecha_Cita { get; set; }

        [StringLength(50)]
        public string Modalidad { get; set; }

        public DateTime Fecha_solicitud { get; set; } = DateTime.Now;

        public DateTime Fecha_notificacion { get; set; }

        public TimeSpan Hora_notificacion { get; set; }

        [StringLength(255)]
        public string Tipo_Tratamiento { get; set; }

        [StringLength(255)]
        public string Tipo_Consulta { get; set; }

        [StringLength(255)]
        public string Receta_solicitada { get; set; }

        public int Citas_Asistidas { get; set; } = 0;

        public int Citas_No_Asistidas { get; set; } = 0;

        // Llaves foraneas de las clases 
        [ForeignKey(nameof(Id_Medico))]
        public virtual Medico Medico { get; set; }

        [ForeignKey(nameof(Id_Atencion_Cliente))]
        public virtual Atencion_Cliente Atencion_Cliente { get; set; }

        //Relacion con la tabla Paciente Cita 
        public virtual ICollection<Paciente_Cita> Paciente_Citas { get; set; } = new List<Paciente_Cita>();

    }
}