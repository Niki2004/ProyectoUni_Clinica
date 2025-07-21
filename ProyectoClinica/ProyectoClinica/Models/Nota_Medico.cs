using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Nota_Medico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Nota_Medico { get; set; }

        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [StringLength(255)]
        [Display(Name = "Observación")]
        public string Observacion { get; set; }

        [StringLength(255)]
        [Display(Name = "Recomendación")]
        public string Recomendacion { get; set; }
    }
}