using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class Notificacion
    {
        public int Id { get; set; } 
        public string Mensaje { get; set; } 
        public DateTime Fecha { get; set; } 
    }
}