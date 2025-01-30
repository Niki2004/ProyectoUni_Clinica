using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Bancos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Banco { get; set; }

        public int Id_Diario { get; set; }

        public string Nombre_Banco { get; set; }

        public string Codigo_Banco { get; set; }

        //Llave foranea
        [ForeignKey(nameof(Id_Diario))]
        public virtual Diarios_Contables Diarios_Contables { get; set; }

    }
}