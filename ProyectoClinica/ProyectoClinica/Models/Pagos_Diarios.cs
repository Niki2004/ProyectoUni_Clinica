using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Pagos_Diarios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Pago_Diario { get; set; }

        [Required]
        public int Id_Contabilidad { get; set; }

        [Required]
        public int Id_Empleado { get; set; }

        [Required]
        public DateTime Fecha_Pago { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [Required]
        [StringLength(50)]
        public string Metodo_Pago { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado_Pago { get; set; }

        [StringLength(255)]
        public string Observaciones { get; set; }

        [Required]
        public DateTime Fecha_Registro { get; set; }

        // Relaciones con otras entidades
        [ForeignKey("Id_Contabilidad")]
        public virtual Contabilidad Contabilidad { get; set; }

        [ForeignKey("Id_Empleado")]
        public virtual Empleado Empleado { get; set; }
    }
}