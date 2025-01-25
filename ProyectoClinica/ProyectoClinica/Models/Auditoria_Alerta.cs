using ProyectoClinica.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ProyectoClinica.Models
{
    public class Auditoria_Alerta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Auditoria { get; set; }

        [Required]
        public int Id_Paciente { get; set; }

        [StringLength(255)]
        public string Alerta_Respaldo_Fallido { get; set; }

        [StringLength(100)]
        public string Usuario_Modificacion { get; set; }

        public DateTime Fecha_Modificacion { get; set; } = DateTime.Now;

        // Llave foranea y relacion 
        [ForeignKey(nameof(Id_Paciente))]
        public virtual Paciente Paciente { get; set; }

    }
}
