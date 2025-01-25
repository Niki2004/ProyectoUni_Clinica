using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Tipo_Servicio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Tipo_Servicio { get; set; }

        [Required]
        [StringLength(100)]
        public string Descripcion { get; set; }

        //Relación de la tabla Atencion_Cliente
        public virtual ICollection<Atencion_Cliente> Atencion_Cliente { get; set; } = new List<Atencion_Cliente>();
    }
}