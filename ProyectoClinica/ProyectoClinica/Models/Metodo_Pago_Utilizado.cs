using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Metodo_Pago_Utilizado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_MetodoPagoUtilizado { get; set; }

        [Required]
        public int Id_Factura { get; set; }

        [Required]
        public decimal Monto { get; set; }

        public int Id_MetodoPago { get; set; }
        [ForeignKey(nameof(Id_MetodoPago))]
        public virtual Metodo_Pago MetodoPago { get; set; }

        [ForeignKey(nameof(Id_Factura))]
        public virtual Factura Factura { get; set; }

        public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();


    }
}