using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Inventario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Articulo { get; set; }

        [Required]
        public int Id_Estado { get; set; }
       
        public string NombreArticulo { get; set; }

        public string Marca { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public DateTime FechaCaducidad { get; set; }

        public string TipoArticulo { get; set; }

        public DateTime FechaIngreso { get; set; } = DateTime.Now;

        //Llave foranea
        [ForeignKey(nameof(Id_Estado))]
        public virtual Estado Estado { get; set; }

        //Relación de la tabla 
        public virtual ICollection<Avisos> Avisos { get; set; } = new List<Avisos>();

    }
}