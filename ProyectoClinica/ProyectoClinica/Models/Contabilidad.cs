using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Contabilidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Contabilidad { get; set; }

        [Required]
        public int Id_Tipo_Registro { get; set; }

        public int Id_Estado_Contabilidad { get; set; }

        public int Id_Tipo_Transaccion { get; set; }

        public string ClienteProveedor { get; set; }

        public string Conta_pago { get; set; }

        public string Estatus_pago { get; set; }

        public DateTime Fecha_Registro { get; set; }

        public DateTime Fecha_Vencimiento { get; set; }

        public decimal Monto { get; set; }

        public decimal Monto_Anticipo { get; set; } = 0;

        public decimal Impuesto_Aplicado { get; set; }

        public decimal Descuento_Aplicado { get; set; } = 0;

        public string Comentarios { get; set; }

        public DateTime Fecha_Cierre { get; set; }

        public decimal Ingresos_Totales { get; set; }

        public decimal Total_Pagos_Pendientes { get; set; }

        public int Total_Sueldos { get; set; }

        public string Observaciones_Ingresos { get; set; }

        // Llaves foraneas de las tablas 
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey(nameof(Id_Tipo_Registro))]
        public virtual Tipo_Registro Tipo_Registro { get; set; }

        [ForeignKey(nameof(Id_Estado_Contabilidad))]
        public virtual Estado_Contabilidad Estado_Contabilidad { get; set; }

        [ForeignKey(nameof(Id_Tipo_Transaccion))]
        public virtual Tipo_Transaccion Tipo_Transaccion { get; set; }


        //Relación de las tablas 
        public virtual ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();


    }
}