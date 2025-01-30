using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Asientos_Contables
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Asiento { get; set; }

        public string Numero_Asiento { get; set; }

        public DateTime Fecha_Asiento { get; set; }

        public string Tipo_Asiento { get; set; }

        public string Cuenta_Contable { get; set; }

        public string Descripcion_Cuenta { get; set; }

        public string Tipo_Movimiento { get; set; }

        public decimal Monto { get; set; }

        public string Concepto { get; set; }

        public string Referencia { get; set; }

        public string Centros_Costos { get; set; }

        public string Periodo_Contable { get; set; }

        public string Estado { get; set; }

        public string Observaciones { get; set; }

        public string Usuario_Registro { get; set; }

        public DateTime Fecha_Registro { get; set; }

        public string Usuario_Modificacion { get; set; }

        public DateTime Fecha_Modificacion { get; set; }

    }
}