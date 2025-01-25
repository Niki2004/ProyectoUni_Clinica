using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Avisos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Aviso { get; set; }

        [Required]
        public int Id_Articulo { get; set; }
       
        public int DiasAnticipacion { get; set; }

        public int HorasAnticipacion { get; set; } = 0;

        public string Categorias { get; set; }

        public bool NotificacionPush { get; set; } = false;

        public bool NotificacionEmail { get; set; } = false;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        //Llave foranea 
        [ForeignKey(nameof(Id_Articulo))]
        public virtual Inventario Inventario { get; set; }

    }
}