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

        [Display(Name = "Articulo")]
        public string NombreArticulo { get; set; }

        public string Marca { get; set; }

        public int Cantidad { get; set; }

        [Display(Name = "Precio")]
        public decimal PrecioUnitario { get; set; }

        [Display(Name = "Caducidad")]
        public DateTime FechaCaducidad { get; set; }

        [Display(Name = "Tipo de articulo")]
        public string TipoArticulo { get; set; }

        [Display(Name = "Ingreso")]
        public DateTime FechaIngreso { get; set; } = DateTime.Now;

        //Llave foranea
        [ForeignKey(nameof(Id_Estado))]
        public virtual Estado Estado { get; set; }

        //Relación de la tabla 
        public virtual ICollection<Avisos> Avisos { get; set; } = new List<Avisos>();

    }
}