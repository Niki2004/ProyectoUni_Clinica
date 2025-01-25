using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Servicio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Servicio { get; set; }

        public string Nombre_Servicio { get; set; }

        public decimal Precio_Servicio { get; set; }

        public string Especialidad { get; set; }

        public virtual ICollection<Servicios_Brindados> Servicios_Brindados { get; set; } = new List<Servicios_Brindados>();

    }
}