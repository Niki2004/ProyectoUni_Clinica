using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Servicios_Brindados
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_ServicioBrindado { get; set; }

        [Required]
        public int Id_Factura {  get; set; }

        public int Id_Servicio { get; set; }
        [ForeignKey(nameof(Id_Servicio))]
        public virtual Servicio Servicio { get; set; }

        [ForeignKey(nameof(Id_Factura))]
        public virtual Factura Factura { get; set; }

        public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    }
}