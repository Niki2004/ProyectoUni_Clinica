using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Inventario_Detalle_Conta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Inventario_Detalle { get; set; }

        [Required]
        public int Id_Inventario_Encabezado { get; set; }

        public int? Id_Departamento { get; set; }

        [Required]
        public int Id_Producto { get; set; }

        [Required]
        public DateTime Fecha_Entrada { get; set; }

        public int? Cantidad_Stock { get; set; }

        public DateTime? Fecha_Salida { get; set; }

        public int? Cantidad_Salida { get; set; }

        public float? Precio { get; set; }

        // Relaciones con otras tablas
        [ForeignKey("Id_Inventario_Encabezado")]
        public virtual Inventario_Encabezado_Conta InventarioEncabezado { get; set; }

        [ForeignKey("Id_Departamento")]
        public virtual Departamentos Departamento { get; set; }

        [ForeignKey("Id_Producto")]
        public virtual Productos_Conta Producto { get; set; }
    }
}