using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Pagos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Pago { get; set; }

        public int Id_Banco { get; set; }

        public string Numero_Referencia { get; set; }

        public DateTime Fecha_Pago { get; set; }

        public string Tipo_Pago { get; set; }

        public decimal Monto { get; set; }

        public string Beneficiario { get; set; }

        public string Cuenta_Beneficiario { get; set; }

        public string Estado_Pago { get; set; }

        public string Descripcion { get; set; }

        public string Observaciones { get; set; }

        public string Usuario_Registro  { get; set; }

        public DateTime Fecha_Registro { get; set; }

        public string Usuario_Modificacion { get; set; }

        public DateTime Fecha_Modificacion { get; set; }

        // Llaves foraneas de las tablas 
        [ForeignKey(nameof(Id_Banco))]
        public virtual Bancos Bancos { get; set; }

    }
}