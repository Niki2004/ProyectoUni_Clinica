using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Paciente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Paciente { get; set; }

        [StringLength(255)]
        public string Contacto_emergencia { get; set; }

        public string Nota_paciente { get; set; }

        [StringLength(255)]
        public string Sintomas { get; set; }

        [StringLength(255)]
        public string Nombre_Complicacion { get; set; }

        [StringLength(50)]
        public string Formato_imagen { get; set; }

        // Llaves foraneas de las clases 
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; } // El campo Id en AspNetUsers es de tipo string.
        public ApplicationUser ApplicationUser { get; set; }

        //Relacion con la tabla Paciente_Cita
        public virtual ICollection<Paciente_Cita> Paciente_Citas { get; set; } = new List<Paciente_Cita>();
    }
}


