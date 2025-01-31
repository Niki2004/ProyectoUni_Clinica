using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Reporte
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Reporte { get; set; }

        [Required]
        public int Id_Empleado { get; set; }

        [Required]
        public int Id_Cita { get; set; }

        [Required]
        public int Id_Contabilidad { get; set; }

        [Required]
        public int Id_SReceta { get; set; }

        [Required]
        public int Id_Atencion_Cliente { get; set; }

        [Required]
        public int Id_Medico { get; set; }

        [Required]
        public int Id_Factura { get; set; }

        public DateTime Fecha_Reporte { get; set; } = DateTime.Now;



        //Llaves foraneas de las tablas 
        [ForeignKey(nameof(Id_Empleado))]
        public virtual Empleado Empleado { get; set; }

        [ForeignKey(nameof(Id_Cita))]
        public virtual Cita Cita { get; set; }

        [ForeignKey(nameof(Id_Contabilidad))]
        public virtual Contabilidad Contabilidad { get; set; }

        [ForeignKey(nameof(Id_SReceta))]
        public virtual Solicitud_Receta Solicitud_Receta { get; set; }

        [ForeignKey(nameof(Id_Atencion_Cliente))]
        public virtual Atencion_Cliente Atencion_Cliente { get; set; }

        [ForeignKey(nameof(Id_Medico))]
        public virtual Medico Medico { get; set; }

        [ForeignKey(nameof(Id_Factura))]
        public virtual Factura Factura { get; set; }

        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}