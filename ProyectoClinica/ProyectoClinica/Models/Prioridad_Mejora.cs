using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Prioridad_Mejora
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Prioridad { get; set; }

        [Required]
        [StringLength(50)]
        public string Descripcion { get; set; }

        //Relación de la tabla Atencion_Cliente
        public virtual ICollection<Atencion_Cliente> Atencion_Cliente { get; set; } = new List<Atencion_Cliente>();
    }
}