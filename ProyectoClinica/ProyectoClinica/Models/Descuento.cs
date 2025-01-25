using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Descuento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Descuento { get; set; }
        public string Nombre_Descuento { get; set; }
        public string Codigo_Descuento { get; set; }
        public decimal Porcentaje_Descuento { get; set; }
        public int Limite_Usos { get; set; }
        public string Compania_Afiliada { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime Fecha_Creacion { get; set; } = DateTime.Now;

        public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    }
}