using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoClinica.Models
{
	public class Historial_Aplicaciones
	{
		[Key]
		public int Id_Historial { get; set; }

		[Required]
		[StringLength(50)]
		public string Tipo_Evento { get; set; } 

		[Required]
		[StringLength(500)]
		public string Descripcion { get; set; }

		[Required]
		public DateTime Fecha_Hora { get; set; }

		[StringLength(50)]
		public string Rol_Anterior { get; set; }

		[StringLength(50)]
		public string Rol_Nuevo { get; set; }

		[StringLength(100)]
		public string Area_Accedida { get; set; }

		[StringLength(500)]
		public string Motivo_Bloqueo { get; set; }

		[Required]
		public bool Exitoso { get; set; } 

        [ForeignKey("ApplicationUser")]
        public string Id { get; set; } 
        public ApplicationUser ApplicationUser { get; set; }
    }
}