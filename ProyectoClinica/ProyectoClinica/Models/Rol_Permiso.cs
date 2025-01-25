using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Rol_Permiso
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Rol { get; set; }

        [Required]
        [StringLength(100)]
        public string Rol_Usuario { get; set; }

        [StringLength(255)]
        public string Permisos { get; set; }

        // Llaves foraneas de las clases 
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; } // El campo Id en AspNetUsers es de tipo string.
        public ApplicationUser ApplicationUser { get; set; }
    }
}