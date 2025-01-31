using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Busquedas_exportaciones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Busqueda { get; set; }

        [Required]
        public int Id_Paciente { get; set; }
       
        public string Criterios_Busqueda { get; set; }

        public string Estado_Busqueda { get; set; }

        public string Exportado_PDF { get; set; }

        public DateTime Fecha_Exportacion { get; set; } = DateTime.Now;

        //Llave foranea 
        // Llaves foraneas de las clases 
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; } // El campo Id en AspNetUsers es de tipo string.
        public ApplicationUser ApplicationUser { get; set; }

    }
}