using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Nota_Paciente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Nota_Paciente { get; set; }

        [StringLength(255)]
        [Display(Name = "Nota del paciente")]
        public string Nota_Del_Paciente { get; set; }
    }
}