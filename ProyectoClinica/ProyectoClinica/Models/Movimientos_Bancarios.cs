using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Movimientos_Bancarios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Movimiento { get; set; }

        [Required]
        public int Id_Diario { get; set; }

        [Required]
        public int Id_Conciliacion { get; set; }

        [Required]
        public int Id_Pagos_Diarios { get; set; }

        [Required]
        public int Ingresos { get; set; }

        [Required]
        public int Egresos { get; set; }

        [Required]
        public decimal Saldo { get; set; }

        [Required]
        public DateTime Fecha_Movimiento { get; set; }

        [Required]
        [StringLength(255)]
        public string Descripcion { get; set; }

        // Relaciones con otras entidades
        [ForeignKey("Id_Diario")]
        public virtual Diarios_Contables Diarios_Contables { get; set; }

        [ForeignKey("Id_Conciliacion")]
        public virtual Conciliaciones_Bancarias Conciliaciones_Bancarias { get; set; }

        [ForeignKey("Id_Pagos_Diarios")]
        public virtual Pagos_Diarios Pagos_Diarios { get; set; }
    }
}