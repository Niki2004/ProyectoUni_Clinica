using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Sensible_Comentario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Sensible_Comentario { get; set; }

        [StringLength(50)]
        public string Sensible { get; set; }     // 'Sensible', 'No sensible'

    }
}