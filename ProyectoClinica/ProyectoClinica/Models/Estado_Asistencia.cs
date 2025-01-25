using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Estado_Asistencia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Estado_Asistencia { get; set; }

        [Required]
        [StringLength(255)]
        public string Descripcion { get; set; }

        //Relación de la tabla Medico
        public virtual ICollection<Medico> Medico { get; set; } = new List<Medico>();
    }
}