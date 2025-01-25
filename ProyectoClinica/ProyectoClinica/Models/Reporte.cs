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
        public int Id_Cita { get; set; }

        [Required]
        public int Id_Paciente { get; set; }

        [Required]
        public int Id_Factura { get; set; }

        [Required]
        public int Id_Contabilidad { get; set; }

        [Required]
        public int Id_Medico { get; set; }
      
        public string Tipo_Reporte { get; set; }

        public int Promedio_Complicaciones { get; set; }

        public decimal Costo_Promedio { get; set; }

        public DateTime Fecha_Reporte { get; set; } = DateTime.Now;

        public int Total_Complicaciones { get; set; }

        public string Rango_Edad { get; set; }

        public string Intervalo_Datos { get; set; }

        public string Dia_Semana { get; set; }

        public string Horario { get; set; }

        public decimal Porcentaje_Exito { get; set; }

        //Llaves foraneas de las tablas 
        [ForeignKey(nameof(Id_Cita))]

        public virtual Cita Cita { get; set; }

        [ForeignKey(nameof(Id_Paciente))]
        public virtual Paciente Paciente { get; set; }

        [ForeignKey(nameof(Id_Factura))]
        public virtual Factura Factura { get; set; }

        [ForeignKey(nameof(Id_Contabilidad))]
        public virtual Contabilidad Contabilidad { get; set; }

        [ForeignKey(nameof(Id_Medico))]
        public virtual Medico Medico { get; set; }

        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }


    }
}