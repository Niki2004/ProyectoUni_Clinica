using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;

namespace ProyectoClinica.Controllers
{
    public class ReporteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReporteController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult VistaAdmRep()
        {
            return View();
        }

        public ActionResult VistaReporteConta()
        {
            return View();
        }

        //Historia de usuario 01 
        public ActionResult AsistenciaConsulta(DateTime? fechaCita, string especialidad)
        {
            var citas = _context.Cita.Include("Medico").AsQueryable();
            if (fechaCita.HasValue)
            {
                DateTime inicioDelDia = fechaCita.Value.Date;
                DateTime finDelDia = inicioDelDia.AddDays(1).AddTicks(-1);
                citas = citas.Where(c => c.Fecha_Cita >= inicioDelDia && c.Fecha_Cita <= finDelDia);
            }
            if (!string.IsNullOrEmpty(especialidad))
            {
                citas = citas.Where(c => c.Medico.Especialidad.Contains(especialidad));
            }
            ViewBag.FechaCita = fechaCita;
            ViewBag.Especialidad = especialidad;
            return View(citas.ToList());
        }

        public ActionResult ExportarExcel(DateTime? fechaCita, string especialidad)
        {
            // Establecer el contexto de la licencia de EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var citas = _context.Cita.Include("Medico").AsQueryable();

            if (fechaCita.HasValue)
            {
                DateTime inicioDelDia = fechaCita.Value.Date;
                DateTime finDelDia = inicioDelDia.AddDays(1).AddTicks(-1);
                citas = citas.Where(c => c.Fecha_Cita >= inicioDelDia && c.Fecha_Cita <= finDelDia);
            }

            if (!string.IsNullOrEmpty(especialidad))
            {
                citas = citas.Where(c => c.Medico.Especialidad.Contains(especialidad));
            }

            var listaCitas = citas.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Informe de asistencia");

                // Encabezados de tabla (en la fila 1 como en la imagen)
                worksheet.Cells["A1"].Value = "Fecha";
                worksheet.Cells["B1"].Value = "Nombre del paciente";
                worksheet.Cells["C1"].Value = "Motivo de la consulta";
                worksheet.Cells["D1"].Value = "Estado";
                worksheet.Cells["E1"].Value = "Especialidad";

                // Estilo para la cabecera de la tabla - color azul-verde como en la imagen
                using (var range = worksheet.Cells["A1:E1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#26a69a")); // Color teal como en la imagen
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                // Agregar datos desde la fila 2 (como en la imagen)
                int row = 2;
                foreach (var cita in listaCitas)
                {
                    worksheet.Cells[row, 1].Value = cita.Fecha_Cita.ToString("dd/MM/yy");
                    worksheet.Cells[row, 2].Value = cita.Nombre_Paciente;
                    worksheet.Cells[row, 3].Value = cita.Sintomas;
                    worksheet.Cells[row, 4].Value = cita.Estado_Asistencia;
                    worksheet.Cells[row, 5].Value = cita.Medico.Especialidad;

                    // Alinear el texto al centro
                    using (var range = worksheet.Cells[row, 1, row, 5])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    row++;
                }

                // Añadir nota de pie de página en la fila 5 (como en la imagen)
                worksheet.Cells["A5"].Value = "El informe de asistencia proporciona un desglose detallado de las citas médicas programadas en el centro médico.";
                worksheet.Cells["A5:F5"].Merge = true;
                worksheet.Cells["A5"].Style.Font.Italic = true;
                worksheet.Cells["A5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                // Configuración de ancho de columnas similar a la imagen
                worksheet.Column(1).Width = 15; // Fecha
                worksheet.Column(2).Width = 25; // Nombre del paciente
                worksheet.Column(3).Width = 30; // Motivo
                worksheet.Column(4).Width = 15; // Estado
                worksheet.Column(5).Width = 15; // Especialidad

                // Agregar un borde verde en las celdas C9:C10 (como se ve en la imagen)
                using (var range = worksheet.Cells["C9:C10"])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#26a69a"));
                    range.Style.Border.Bottom.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#26a69a"));
                    range.Style.Border.Left.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#26a69a"));
                    range.Style.Border.Right.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#26a69a"));
                }

                var stream = new MemoryStream(package.GetAsByteArray());
                string fileName = "Informe_Asistencia";

                if (fechaCita.HasValue)
                {
                    fileName += "_" + fechaCita.Value.ToString("yyyyMMdd");
                }

                if (!string.IsNullOrEmpty(especialidad))
                {
                    fileName += "_" + especialidad.Replace(" ", "_");
                }

                fileName += ".xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        //Historia de usuario 02 
        public ActionResult ResultadoTratamiento(string nombreReceta)
        {
            var recetas = _context.Receta.AsQueryable();
            if (!string.IsNullOrEmpty(nombreReceta))
            {
                recetas = recetas.Where(r => r.Nombre_Receta.Contains(nombreReceta));
            }
            ViewBag.NombreReceta = nombreReceta;
            return View(recetas.ToList());
        }

        public ActionResult ExportarExcelRecetas(string nombreReceta)
        {
            // Establecer el contexto de la licencia de EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var recetas = _context.Receta.AsQueryable();

            if (!string.IsNullOrEmpty(nombreReceta))
            {
                recetas = recetas.Where(r => r.Nombre_Receta.Contains(nombreReceta));
            }

            var listaRecetas = recetas.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Informe de recetas");

                // Encabezados de tabla
                worksheet.Cells["A1"].Value = "Fecha de creación";
                worksheet.Cells["B1"].Value = "Nombre de la receta";
                worksheet.Cells["C1"].Value = "Observaciones de pacientes";
                worksheet.Cells["D1"].Value = "Duración del tratamiento";
                worksheet.Cells["E1"].Value = "Cantidad requerida";
                worksheet.Cells["F1"].Value = "Motivo de la solicitud";

                // Estilo de encabezado
                using (var range = worksheet.Cells["A1:F1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#26a69a"));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                // Agregar datos
                int row = 2;
                foreach (var receta in listaRecetas)
                {
                    worksheet.Cells[row, 1].Value = receta.Fecha_Creacion.ToString("dd/MM/yy");
                    worksheet.Cells[row, 2].Value = receta.Nombre_Receta;
                    worksheet.Cells[row, 3].Value = receta.Observaciones_Pacientes;
                    worksheet.Cells[row, 4].Value = receta.Duracion_Tratamiento;
                    worksheet.Cells[row, 5].Value = receta.Cantidad_Requerida;
                    worksheet.Cells[row, 6].Value = receta.Motivo_Solicitud;

                    using (var range = worksheet.Cells[row, 1, row, 6])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    row++;
                }

                // Configuración de ancho de columnas
                worksheet.Column(1).Width = 20; // Fecha de creación
                worksheet.Column(2).Width = 25; // Nombre de la receta
                worksheet.Column(3).Width = 30; // Observaciones
                worksheet.Column(4).Width = 25; // Duración del tratamiento
                worksheet.Column(5).Width = 20; // Cantidad requerida
                worksheet.Column(6).Width = 25; // Motivo de la solicitud

                var stream = new MemoryStream(package.GetAsByteArray());
                string fileName = "Informe_Recetas";

                if (!string.IsNullOrEmpty(nombreReceta))
                {
                    fileName += "_" + nombreReceta.Replace(" ", "_");
                }

                fileName += ".xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        //Historia de usuario 03 
        public ActionResult AreaMejora(string nombreReceta)
        {
            var recetas = _context.Receta.AsQueryable();
            if (!string.IsNullOrEmpty(nombreReceta))
            {
                recetas = recetas.Where(r => r.Nombre_Receta.Contains(nombreReceta));
            }
            ViewBag.NombreReceta = nombreReceta;
            return View(recetas.ToList());
        }

        public ActionResult ExportarExcelAreaMejora(string nombreReceta)
        {
            // Establecer el contexto de la licencia de EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var recetas = _context.Receta.AsQueryable();

            if (!string.IsNullOrEmpty(nombreReceta))
            {
                recetas = recetas.Where(r => r.Nombre_Receta.Contains(nombreReceta));
            }

            var listaRecetas = recetas.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Informe de recetas");

                // Encabezados de tabla
                worksheet.Cells["A1"].Value = "Fecha de creación";
                worksheet.Cells["B1"].Value = "Nombre de la receta";
                worksheet.Cells["C1"].Value = "Observaciones de pacientes";
                worksheet.Cells["D1"].Value = "Duración del tratamiento";
                worksheet.Cells["E1"].Value = "Cantidad requerida";
                worksheet.Cells["F1"].Value = "Motivo de la solicitud";

                // Estilo de encabezado
                using (var range = worksheet.Cells["A1:F1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#26a69a"));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                // Agregar datos
                int row = 2;
                foreach (var receta in listaRecetas)
                {
                    worksheet.Cells[row, 1].Value = receta.Fecha_Creacion.ToString("dd/MM/yy");
                    worksheet.Cells[row, 2].Value = receta.Nombre_Receta;
                    worksheet.Cells[row, 3].Value = receta.Observaciones_Pacientes;
                    worksheet.Cells[row, 4].Value = receta.Duracion_Tratamiento;
                    worksheet.Cells[row, 5].Value = receta.Cantidad_Requerida;
                    worksheet.Cells[row, 6].Value = receta.Motivo_Solicitud;

                    using (var range = worksheet.Cells[row, 1, row, 6])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    row++;
                }

                // Configuración de ancho de columnas
                worksheet.Column(1).Width = 20; // Fecha de creación
                worksheet.Column(2).Width = 25; // Nombre de la receta
                worksheet.Column(3).Width = 30; // Observaciones
                worksheet.Column(4).Width = 25; // Duración del tratamiento
                worksheet.Column(5).Width = 20; // Cantidad requerida
                worksheet.Column(6).Width = 25; // Motivo de la solicitud

                var stream = new MemoryStream(package.GetAsByteArray());
                string fileName = "Informe_Recetas";

                if (!string.IsNullOrEmpty(nombreReceta))
                {
                    fileName += "_" + nombreReceta.Replace(" ", "_");
                }

                fileName += ".xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        //Historia de usuario 04
    }
}
   