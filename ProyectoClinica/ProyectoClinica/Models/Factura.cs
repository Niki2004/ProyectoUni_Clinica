using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Factura
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Factura { get; set; }

        public int Id_Descuento { get; set; }

        public DateTime FechaHora { get; set; }

        public string CedulaCliente { get; set; }

        public string NombreCliente { get; set; }

        public decimal Subtotal { get; set; }
      
        public decimal Descuento_Aplicado { get; set; } = 0;

        public decimal Impuesto { get; set; }

        public decimal TotalPagado { get; set; }

        //Llaves foraneas

        [ForeignKey(nameof(Id_Descuento))]
        public virtual Descuento Descuento { get; set; }

        //Relacion de las tablas 
        public virtual ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();

    }
}