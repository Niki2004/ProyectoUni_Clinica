using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Modificacion_Receta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Modificacion_receta { get; set; }

        [Required]
        public int Id_receta { get; set; }

        [Display(Name = "Fecha de modificacion")]
        public DateTime Fecha_Modificacion { get; set; } = DateTime.Now;

        [StringLength(255)]
        [Display(Name = "Consentimiento")]
        public string Consentimiento { get; set; }

        [StringLength(255)]
        [Display(Name = "Motivo de modificacion")]
        public string motivo_modificacion { get; set; }

        // Llaves foraneas de la clase medico 
        [ForeignKey(nameof(Id_receta))]
        public virtual Receta Receta { get; set; }
    }
}