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
        public int Id_Estado { get; set; }

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

 
        [StringLength(50)]
        public string Jornada { get; set; }

        public DateTime Fecha_registro { get; set; } = DateTime.Now;

        [StringLength(255)]
        public string Departamento { get; set; }



        // Llave foranea  y relacion 
       

        [ForeignKey(nameof(Id_Estado))]
        public virtual Estado Estado { get; set; }

        // Relaciones
        public ICollection<RolAsignacion> Roles { get; set; }
        public ICollection<Documento> Documentos { get; set; }
        public ICollection<NotificacionEmpleado> Notificaciones { get; set; }
        public ICollection<Evaluacion> Evaluaciones { get; set; }


    }
}