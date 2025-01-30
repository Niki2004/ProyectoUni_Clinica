using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Respaldo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Respaldo { get; set; }

        [Required]
        public int Id_Paciente { get; set; }
       
        public string Respaldo_configurado { get; set; }

        public DateTime Fecha_respaldo { get; set; } = DateTime.Now;

        public string Respaldo_exitoso { get; set; }

        //Llaves foraneas 
        // Llaves foraneas de las clases 
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; } // El campo Id en AspNetUsers es de tipo string.
        public ApplicationUser ApplicationUser { get; set; }
    }
}