using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ProyectoClinica.Controllers
{
    public class ReporteController : Controller
    {
        private ApplicationDbContext BaseDatos = new ApplicationDbContext();

        public ActionResult VistaAdmRep()
        {
            return View();
        }

        public ActionResult VistaReporteConta()
        {
            return View();
        }

        //Historia de usuario 01
        public ActionResult AsistenciaConsulta(DateTime? fechaCita, string especialidadMedico)
        {
            // Cargar especialidades para el dropdown
            ViewBag.Especialidades = BaseDatos.Medico
                .Select(m => m.Especialidad)
                .Distinct()
                .OrderBy(e => e)
                .ToList();

            // Consulta base con carga explícita de relaciones
            var citas = BaseDatos.Reporte
                .Include(r => r.Cita)
                .Include(r => r.Medico)
                .Where(r => r.Cita != null) // Asegurar que solo se seleccionen reportes con citas
                .AsQueryable();

            // Filtrar por fecha de cita si se proporciona
            if (fechaCita.HasValue)
            {
                citas = citas.Where(r =>
                    r.Cita.Fecha_Cita.Year == fechaCita.Value.Year &&
                    r.Cita.Fecha_Cita.Month == fechaCita.Value.Month &&
                    r.Cita.Fecha_Cita.Day == fechaCita.Value.Day
                );
            }

            // Filtrar por especialidad médica si se proporciona
            if (!string.IsNullOrEmpty(especialidadMedico))
            {
                citas = citas.Where(r =>
                    r.Medico != null &&
                    r.Medico.Especialidad != null &&
                    r.Medico.Especialidad.ToLower().Contains(especialidadMedico.ToLower())
                );
            }

            // Obtener la lista de citas
            var resultadoCitas = citas
                .Select(r => new
                {
                    Reporte = r,
                    MedicoNombre = r.Medico.Nombre,
                    CitaNombrePaciente = r.Cita.Nombre_Paciente
                })
                .ToList()
                .Select(x => x.Reporte)
                .ToList();

            // Agregar información de depuración
            ViewBag.TotalResultados = resultadoCitas.Count;
            ViewBag.DetallesResultados = resultadoCitas.Select(r => new
            {
                MedicoNombre = r.Medico?.Nombre,
                CitaNombrePaciente = r.Cita?.Nombre_Paciente
            }).ToList();

            return View(resultadoCitas);
        }

    }
    }