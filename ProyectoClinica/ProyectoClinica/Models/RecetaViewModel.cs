using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class RecetaViewModel
    {
        public List<Receta> Recetas { get; set; }
        public List<Modificacion_Receta> Modificaciones { get; set; }
    }
}