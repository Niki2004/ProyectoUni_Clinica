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

        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [StringLength(255)]
        [Display(Name = "Paciente")]
        public string Nombre_Paciente { get; set; }

        [StringLength(255)]
        [Display(Name = "Estado")]
        public string Estado_Asistencia { get; set; } //Asistida o no asistida

        [Required]
        [Display(Name = "Hora")]
        public TimeSpan Hora_cita { get; set; }

        [StringLength(255)]
        [Display(Name = "Descripción")]
        public string Descripcion_Complicaciones { get; set; }

        [StringLength(255)]
        [Display(Name = "Sintomas")]
        public string Sintomas { get; set; }

        [Required]
        [Display(Name = "Fecha de la cita")]
        public DateTime Fecha_Cita { get; set; }

        [StringLength(50)]
        public string Modalidad { get; set; } //Presencial o virtual

        // Llaves foraneas de la clase medico 
        [ForeignKey(nameof(Id_Medico))]
        public virtual Medico Medico { get; set; }

    }
}