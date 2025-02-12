using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Inventario_Encabezado_Conta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Inventario_Encabezado { get; set; }

        [Required]
        public int Id_Usuario { get; set; }

        [Required]
        public DateTime Fecha_Inventario { get; set; }

        [Required]
        public int Inventario_Estatus { get; set; }

        [Required]
        public DateTime Inventario_Creacion { get; set; }

        public DateTime? Inventario_Actualizacion { get; set; }

        public DateTime? Inventario_Eliminacion { get; set; }
    }
}