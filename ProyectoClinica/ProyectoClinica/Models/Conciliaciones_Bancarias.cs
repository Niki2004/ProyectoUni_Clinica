using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Conciliaciones_Bancarias
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Conciliacion { get; set; }

        public int Id_Banco { get; set; }

        public int Id_Diario { get; set; }

        public int Id_Tipo_Registro { get; set; }

        public DateTime Fecha_Conciliacion { get; set; }

        public decimal Saldo_Contable { get; set; }

        public decimal Saldo_Banco { get; set; }

        // Llaves foraneas de las tablas 
        [ForeignKey(nameof(Id_Banco))]
        public virtual Bancos Bancos { get; set; }

        [ForeignKey(nameof(Id_Diario))]
        public virtual Diarios_Contables Diarios_Contables { get; set; }

        [ForeignKey(nameof(Id_Tipo_Registro))]
        public virtual Tipo_Registro Tipo_Registro { get; set; }

    }
}