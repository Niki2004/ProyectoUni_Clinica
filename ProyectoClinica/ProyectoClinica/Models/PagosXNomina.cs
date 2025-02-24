using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class PagosXNomina
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_PagosXNomina { get; set; }

        [Required]
        [Display(Name = "Id de Contabilidad")]
        public int Id_Contabilidad { get; set; }

        [Required]
        [Display(Name = "Id del Pago")]
        public int Id_Pago { get; set; }

        [Required]
        [Display(Name = "Id del Empleado")]
        public int Id_Empleado { get; set; }

        [Required]
        [Display(Name = "Fecha Pago")]
        public DateTime Fecha_Pago { get; set; }

        [Required]
        [Display(Name = "Monto Pago")]
        public decimal Monto_Pago { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Metodo Pago")]
        public string Metodo_Pago { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Estado del Pago")]
        public string Estado_Pago { get; set; }

        [StringLength(255)]
        public string Observaciones { get; set; }

        [Required]
        [Display(Name = "Fecha Registro")]
        public DateTime Fecha_Registro { get; set; }

        // Relaciones con otras entidades
        [ForeignKey("Id_Contabilidad")]
        public virtual Contabilidad Contabilidad { get; set; }

        [ForeignKey("Id_Pago")]
        public virtual Pagos Pagos { get; set; }

        [ForeignKey("Id_Empleado")]
        public virtual Empleado Empleado { get; set; }
    }
}
