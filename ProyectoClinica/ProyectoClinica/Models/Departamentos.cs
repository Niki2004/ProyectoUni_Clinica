using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Departamentos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Departamento { get; set; }

        [Required]
        [StringLength(255)]
        public string Nombre_Departamento { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public DateTime Fecha_Creacion { get; set; }

        public DateTime? Fecha_Actualizacion { get; set; }

        public DateTime? Fecha_Eliminacion { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool Estatus { get; set; }

        [Required]
        public int Id_Usuario_Creador { get; set; }
    }
}