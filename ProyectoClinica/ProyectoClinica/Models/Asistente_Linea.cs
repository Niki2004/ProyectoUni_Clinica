using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Asistente_Linea
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_asistente { get; set; }

        [Required]
        public int Id_Cita { get; set; }
      
        public string Resumen_Asistencia { get; set; }

        public string Detalle { get; set; }

        //Llaves foraneas 
        [ForeignKey(nameof(Id_Cita))]
        public virtual Cita Cita { get; set; }
    }
}