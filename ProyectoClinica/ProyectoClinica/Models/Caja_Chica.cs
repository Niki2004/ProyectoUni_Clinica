using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Caja_Chica
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Caja_Chica { get; set; }

        public int Id_Contabilidad { get; set; }

        public string Numero_Comprobante { get; set; }

        public DateTime Fecha_Movimiento { get; set; }

        public string Tipo_Movimiento { get; set; }

        public string Concepto { get; set; }

        public decimal Monto { get; set; }

        public decimal Saldo_Anterior { get; set; }

        public decimal Saldo_Actual { get; set; }

        public string Beneficiario { get; set; }

        public string Categoria_Gasto { get; set; }

        public string Numero_Factura { get; set; }

        public string Estado { get; set; }

        public string Observaciones { get; set; }

        public string Usuario_Registro { get; set; }

        public DateTime Fecha_Registro { get; set; }

        public string Usuario_Modificacion { get; set; }

        public DateTime Fecha_Modificacion { get; set; }

        // Llaves foraneas de las tablas 
        [ForeignKey(nameof(Id_Contabilidad))]
        public virtual Contabilidad Contabilidad { get; set; }
    }
}