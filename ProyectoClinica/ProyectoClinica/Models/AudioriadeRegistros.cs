using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class AudioriadeRegistros
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Auditoría { get; set; }

        [ForeignKey("Cita")]
        public int Id_Cita { get; set; }

        public virtual Cita Cita { get; set; }
    }
}