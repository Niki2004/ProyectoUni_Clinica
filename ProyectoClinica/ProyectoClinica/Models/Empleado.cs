using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Empleado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Empleado { get; set; }

        [Required]
        public int Id_Medico { get; set; }

        [Required]
        public int Id_Estado { get; set; }

        [Required]
        [StringLength(450)]
        public string Id_Usuario { get; set; }

        [StringLength(255)]
        public string Comentarios { get; set; }

        [StringLength(255)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string Apellido { get; set; }

        [StringLength(255)]
        public string Cedula { get; set; }
        [StringLength(255)]
        public string Correo { get; set; }

        [StringLength(255)]
        public string Historial_cambios { get; set; }

        [StringLength(50)]
        public string Jornada { get; set; }

        [StringLength(255)]
        public string Notificaciones { get; set; }//

        [StringLength(255)]
        public string Evaluaciones { get; set; }

        public DateTime Fecha_vencimiento_contrato { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        [StringLength(255)]
        public string Administrador_modificacion { get; set; }//

        public DateTime Fecha_registro { get; set; } = DateTime.Now;

        [StringLength(255)]
        public string documentos { get; set; }

        public DateTime Fecha_actualizacion { get; set; }

        public DateTime Fecha_proxima_evaluacion { get; set; }

        [StringLength(255)]
        public string Historial_capacitaciones { get; set; }

        [StringLength(255)]
        public string Departamento { get; set; }

        // Llave foranea  y relacion 
        [ForeignKey(nameof(Id_Medico))]
        public virtual Medico Medico { get; set; }

        [ForeignKey(nameof(Id_Estado))]
        public virtual Estado Estado { get; set; }

        [ForeignKey(nameof(Id_Usuario))]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}