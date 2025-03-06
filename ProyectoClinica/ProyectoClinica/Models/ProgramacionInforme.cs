using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class ProgramacionInforme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_ProgramacionInforme { get; set; }
        public string Periodo { get; set; } // Puede ser "Diario", "Semanal", "Mensual"
        public string Destinatario { get; set; } // Correo electrónico
        public DateTime FechaHora { get; set; } // Fecha y hora de la próxima ejecución

        [ForeignKey("ApplicationUser")]
        public string Id { get; set; } 
        public ApplicationUser ApplicationUser { get; set; }

    }
}