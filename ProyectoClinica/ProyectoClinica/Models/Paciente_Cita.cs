using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Paciente_Cita
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Paciente_Cita { get; set; }

        public int Id_Paciente { get; set; }

        public int Id_Cita { get; set; }

        [ForeignKey(nameof(Id_Paciente))]
        public virtual Paciente Paciente { get; set; }

        [ForeignKey(nameof(Id_Cita))]
        public virtual Cita Cita { get; set; }
    }
}