using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Diarios_Contables
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Diario { get; set; }

        public int Id_Tipo_Registro { get; set; }

        public string Codigo_Diario { get; set; }

        public string Descripcion { get; set; }

        public bool Activo { get; set; }

        public DateTime Fecha_Creacion { get; set; }

        // Llaves foraneas de las tablas 
        [ForeignKey(nameof(Id_Tipo_Registro))]
        public virtual Tipo_Registro Tipo_Registro { get; set; }

    }
}