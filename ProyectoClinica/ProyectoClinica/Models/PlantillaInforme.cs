using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class PlantillaInforme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_PlantillaInforme { get; set; }
        public string NombrePlantilla { get; set; }
        public string CamposSeleccionados { get; set; } // Campos que el administrador seleccionó para el informe
        public DateTime FechaCreacion { get; set; }
    }
}