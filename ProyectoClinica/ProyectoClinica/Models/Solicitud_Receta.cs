using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Solicitud_Receta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_SReceta { get; set; }

        [StringLength(255)]
        [Display(Name = "Receta solicitada")]
        public string receta_solicitada { get; set; }
    }
}