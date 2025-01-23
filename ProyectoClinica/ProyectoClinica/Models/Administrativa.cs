using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Administrativa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Admin { get; set; }

        public int Id_Estado { get; set; }

        public bool Notif_Intentos_Fallidos { get; set; } = true;

        public bool Notif_Acceso_Datos_Sensibles { get; set; } = true;

        public string Ultimos_Intentos_Login { get; set; }

        public string Log_Eliminacion { get; set; }

        // Llaves foraneas de las clases 
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; } // El campo Id en AspNetUsers es de tipo string.
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey(nameof(Id_Estado))]
        public virtual Estado Estado { get; set; }
    }
}