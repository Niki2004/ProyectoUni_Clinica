using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Facturacion_Productos_Conta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Factura_Producto { get; set; }

        [Required]
        public int Id_Factura { get; set; }

        [Required]
        public int Id_Inventario_Encabezado { get; set; }

        [Required]
        public int Id_Producto { get; set; }

        [Required]
        public int Cantidad_Vendida { get; set; }

        [Required]
        public int Costo_Por_Unidad { get; set; }

        public float? IVA { get; set; }

        public float? Descuento_Por_Producto { get; set; }

        [Required]
        public float Subtotal { get; set; }

        // Relaciones con otras tablas
        [ForeignKey("Id_Factura")]
        public virtual Factura Factura { get; set; }

        [ForeignKey("Id_Inventario_Encabezado")]
        public virtual Inventario_Encabezado_Conta InventarioEncabezado { get; set; }

        [ForeignKey("Id_Producto")]
        public virtual Productos_Conta Producto { get; set; }
    }
}