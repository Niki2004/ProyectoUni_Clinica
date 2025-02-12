using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Productos_Conta
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Producto { get; set; }

        [Required]
        [StringLength(255)]
        public string Nombre_producto { get; set; }

        [Required]
        public DateTime Creacion_producto { get; set; }

        public DateTime? Actualizacion_producto { get; set; }

        public DateTime? Eliminacion_producto { get; set; }

        [Required]
        public bool Estatus_producto { get; set; }

        [StringLength(255)]
        public string Tipo_producto { get; set; }

        [Required]
        public int Id_Usuario { get; set; }
    }
}