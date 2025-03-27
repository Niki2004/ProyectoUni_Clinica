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
using Microsoft.AspNet.Identity;
using System.Net.Mail;

namespace ProyectoClinica.Controllers
{
    public class ReporteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReporteController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult VistaAdmRep()
        {
            return View();
        }
       
        [Authorize(Roles = "Contador")]
        public ActionResult VistaReporteConta()
        {
            return View();
        }

        //Historia de usuario 01 
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
        public ActionResult AreaMejora(string comentarioNegativo, string comentarioSensible, string comentarioDestacado, string estadoComentario)
        {
            var costos = _context.Comentario.AsQueryable();

            // Filtro para comentarioNegativo
            if (!string.IsNullOrEmpty(comentarioNegativo))
            {
                if (comentarioNegativo == "1")
                {
                    costos = costos.Where(r => r.Id_Atencion_Cliente == 1);
                }
                else if (comentarioNegativo == "0")
                {
                    // Intenta filtrar todos los que NO son 1
                    costos = costos.Where(r => r.Id_Atencion_Cliente != 1);
                }
            }

            // Filtro para comentarioSensible
            if (!string.IsNullOrEmpty(comentarioSensible))
            {
                if (comentarioSensible == "1")
                {
                    costos = costos.Where(r => r.Id_Sensible_Comentario == 1);
                }
                else if (comentarioSensible == "0")
                {
                    // Intenta filtrar todos los que NO son 1
                    costos = costos.Where(r => r.Id_Sensible_Comentario != 1);
                }
            }

            // Filtro para comentarioDestacado
            if (!string.IsNullOrEmpty(comentarioDestacado))
            {
                if (comentarioDestacado == "1")
                {
                    costos = costos.Where(r => r.Id_Destacado_Comentario == 1);
                }
                else if (comentarioDestacado == "0")
                {
                    // Intenta filtrar todos los que NO son 1
                    costos = costos.Where(r => r.Id_Destacado_Comentario != 1);
                }
            }

            // Filtro para estadoComentario
            if (!string.IsNullOrEmpty(estadoComentario))
            {
                costos = costos.Where(r => r.Estado_Comentario.Estado == estadoComentario);
            }

            // Asignamos los valores a ViewBag para mantener el estado de los filtros
            ViewBag.Negativo = comentarioNegativo;
            ViewBag.Sensible = comentarioSensible;
            ViewBag.Destacado = comentarioDestacado;
            ViewBag.Estado = estadoComentario;

            // Retornamos la vista con los resultados filtrados
            return View(costos.ToList());
        }
        
        public ActionResult ExportarExcelAreaMejora(string areaMejora, bool? esNegativo, bool? esSensible, bool? esDestacado, int? estadoComentario)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var comentarios = _context.Comentario.AsQueryable();

            if (!string.IsNullOrEmpty(areaMejora))
            {
                comentarios = comentarios.Where(r => r.Atencion_Cliente.Comentarios_Paciente.Contains(areaMejora)
                                                   || r.Sensible_Comentario.Sensible.Contains(areaMejora));
            }

            if (esNegativo.HasValue && esNegativo.Value)
            {
                comentarios = comentarios.Where(r => r.Calificacion <= 4);
            }

            if (esSensible.HasValue && esSensible.Value)
            {
                comentarios = comentarios.Where(r => r.Id_Sensible_Comentario == 1);
            }

            if (esDestacado.HasValue && esDestacado.Value)
            {
                comentarios = comentarios.Where(r => r.Id_Destacado_Comentario == 1);
            }

            if (estadoComentario.HasValue)
            {
                comentarios = comentarios.Where(r => r.Id_Estado_Comentario == estadoComentario);
            }

            var listaComentarios = comentarios.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Informe de Comentarios");

                worksheet.Cells["A1"].Value = "Estado Sensible";
                worksheet.Cells["B1"].Value = "Comentario";
                worksheet.Cells["C1"].Value = "Calificación";
                worksheet.Cells["D1"].Value = "Fecha";
                worksheet.Cells["E1"].Value = "Estado";
                worksheet.Cells["F1"].Value = "Estado Destacado";
                worksheet.Cells["G1"].Value = "Comentario del cliente";

                using (var range = worksheet.Cells["A1:G1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#26a69a"));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                int row = 2;
                foreach (var comentario in listaComentarios)
                {
                    worksheet.Cells[row, 1].Value = comentario.Sensible_Comentario.Sensible;
                    worksheet.Cells[row, 2].Value = comentario.Comentario_Texto;
                    worksheet.Cells[row, 3].Value = comentario.Calificacion;
                    worksheet.Cells[row, 4].Value = comentario.Fecha.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 5].Value = comentario.Estado_Comentario.Estado;
                    worksheet.Cells[row, 6].Value = comentario.Destacado_Comentario.Destacado;
                    worksheet.Cells[row, 7].Value = comentario.Atencion_Cliente.Comentarios_Paciente;


                    using (var range = worksheet.Cells[row, 1, row, 7])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    row++;
                }

                worksheet.Column(1).Width = 15;
                worksheet.Column(2).Width = 80;
                worksheet.Column(3).Width = 15;
                worksheet.Column(4).Width = 20;
                worksheet.Column(5).Width = 25;
                worksheet.Column(6).Width = 25;
                worksheet.Column(7).Width = 80;

                string fileName = "Informe_Comentarios";

                if (!string.IsNullOrEmpty(areaMejora))
                {
                    fileName += "_" + areaMejora.Replace(" ", "_");
                }

                fileName += ".xlsx";

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        //Historia de usuario 04
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public ActionResult CostosTratamiento(string Procedimientocostos, string servicio2)
        {
            var costos = _context.Servicio.AsQueryable();

            // Comprobar si ambos filtros están presentes y hacer la comparación de ambos
            if (!string.IsNullOrEmpty(Procedimientocostos) && !string.IsNullOrEmpty(servicio2))
            {
                // Filtra los servicios que contengan al menos uno de los dos
                costos = costos.Where(r => r.Nombre_Servicio.Contains(Procedimientocostos)
                                         || r.Nombre_Servicio.Contains(servicio2));
            }
            // Si solo hay un filtro
            else if (!string.IsNullOrEmpty(Procedimientocostos))
            {
                costos = costos.Where(r => r.Nombre_Servicio.Contains(Procedimientocostos));
            }
            else if (!string.IsNullOrEmpty(servicio2))
            {
                costos = costos.Where(r => r.Nombre_Servicio.Contains(servicio2));
            }

            // Asignar los valores de los filtros al ViewBag para mantener la selección después del postback
            ViewBag.ProcedimientoCostos = Procedimientocostos;
            ViewBag.Servicio2 = servicio2;

            return View(costos.ToList());
        }

        public ActionResult ExportarExcelCostosTratamiento(string Procedimientocostos, string servicio2)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var Procedimiento = _context.Servicio.AsQueryable();

            if (!string.IsNullOrEmpty(Procedimientocostos) && !string.IsNullOrEmpty(servicio2))
            {
                Procedimiento = Procedimiento.Where(r => r.Nombre_Servicio.Contains(Procedimientocostos)
                                                       || r.Nombre_Servicio.Contains(servicio2));
            }

            else if (!string.IsNullOrEmpty(Procedimientocostos))
            {
                Procedimiento = Procedimiento.Where(r => r.Nombre_Servicio.Contains(Procedimientocostos));
            }
            else if (!string.IsNullOrEmpty(servicio2))
            {
                Procedimiento = Procedimiento.Where(r => r.Nombre_Servicio.Contains(servicio2));
            }

            var listaProcedimiento = Procedimiento.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Informe de procedimientos");

                worksheet.Cells["A1"].Value = "Nombre de servicio";
                worksheet.Cells["B1"].Value = "Precio del servicio";
                worksheet.Cells["C1"].Value = "Especialidad";

                using (var range = worksheet.Cells["A1:C1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#26a69a"));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                int row = 2;
                foreach (var ProcedimientoS in listaProcedimiento)
                {
                    worksheet.Cells[row, 1].Value = ProcedimientoS.Nombre_Servicio;
                    worksheet.Cells[row, 2].Value = ProcedimientoS.Precio_Servicio;
                    worksheet.Cells[row, 3].Value = ProcedimientoS.Especialidad;

                    worksheet.Cells[row, 2].Style.Numberformat.Format = "\"₡\" #,##0.00"; 


                    using (var range = worksheet.Cells[row, 1, row, 3])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    row++;
                }

                worksheet.Column(1).Width = 20;
                worksheet.Column(2).Width = 25;
                worksheet.Column(3).Width = 30;

                string fileName = "Informe_Procedimientos";

                if (!string.IsNullOrEmpty(Procedimientocostos))
                {
                    fileName += "_" + Procedimientocostos.Replace(" ", "_");
                }

                if (!string.IsNullOrEmpty(servicio2))
                {
                    fileName += "_" + servicio2.Replace(" ", "_");
                }

                fileName += ".xlsx";

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        //Historia de usuario 05
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public ActionResult Personalizacion(string nombreUser)
        {
            var user = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(nombreUser))
            {
                user = user.Where(r => r.Nombre.Contains(nombreUser));
            }

            ViewBag.NombreUser = nombreUser;
            return View(user.ToList());
        }

        public ActionResult ExportarExcelPersonalizacion(string nombreUser)
        {
            // Establecer el contexto de la licencia de EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var user = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(nombreUser))
            {
                user = user.Where(r => r.Nombre.Contains(nombreUser));
            }

            var listauser = user.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Informe de recetas");

                worksheet.Cells["A1"].Value = "Nombre";
                worksheet.Cells["B1"].Value = "Apellido";
                worksheet.Cells["C1"].Value = "Edad del paciente";
                worksheet.Cells["D1"].Value = "Direccion";
                worksheet.Cells["E1"].Value = "Cedula";
                worksheet.Cells["F1"].Value = "Email";
                worksheet.Cells["G1"].Value = "Número de teléfono";
                worksheet.Cells["H1"].Value = "Género";

                using (var range = worksheet.Cells["A1:H1"])
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
                foreach (var users in listauser)
                {
                    worksheet.Cells[row, 1].Value = users.Nombre;
                    worksheet.Cells[row, 2].Value = users.Apellido;
                    worksheet.Cells[row, 3].Value = users.Edad_Paciente;
                    worksheet.Cells[row, 4].Value = users.Direccion;
                    worksheet.Cells[row, 5].Value = users.Cedula;
                    worksheet.Cells[row, 6].Value = users.Email; 
                    worksheet.Cells[row, 7].Value = users.PhoneNumber;
                    worksheet.Cells[row, 8].Value = users.Genero_Paciente;

                    using (var range = worksheet.Cells[row, 1, row, 8])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    row++;
                }

                worksheet.Column(1).Width = 20; 
                worksheet.Column(2).Width = 25;
                worksheet.Column(3).Width = 30; 
                worksheet.Column(4).Width = 25; 
                worksheet.Column(5).Width = 20; 
                worksheet.Column(6).Width = 25;
                worksheet.Column(7).Width = 25;
                worksheet.Column(8).Width = 25;


                var stream = new MemoryStream(package.GetAsByteArray());
                string fileName = "Informe_Usuario";

                if (!string.IsNullOrEmpty(nombreUser))
                {
                    fileName += "_" + nombreUser.Replace(" ", "_");
                }

                fileName += ".xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        public ActionResult GuardarPlantillaInforme()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GuardarPlantillaInforme(string nombrePlantilla, string camposSeleccionados)
        {
            if (!string.IsNullOrEmpty(nombrePlantilla) && !string.IsNullOrEmpty(camposSeleccionados))
            {
                var plantilla = new PlantillaInforme
                {
                    NombrePlantilla = nombrePlantilla,
                    CamposSeleccionados = camposSeleccionados,
                    FechaCreacion = DateTime.Now
                };

                _context.PlantillaInforme.Add(plantilla);
                _context.SaveChanges();

                return RedirectToAction("ListarPlantillasInformes");
            }

            return View("Error");
        }

        public ActionResult ListarPlantillasInformes()
        {
            var plantillas = _context.PlantillaInforme.ToList();
            return View(plantillas);
        }

        //Historia de usuario 06
        [Authorize(Roles = "Administrador")]
        public ActionResult DatosPaciente(string direccion, string genero, int? edad)
        {
            var pacientes = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(direccion))
            {
                pacientes = pacientes.Where(r => r.Direccion.Contains(direccion));
            }

            if (!string.IsNullOrEmpty(genero))
            {
                pacientes = pacientes.Where(r => r.Genero_Paciente.ToString() == genero);
            }

            if (edad.HasValue)
            {
                pacientes = pacientes.Where(r => r.Edad_Paciente == edad);
            }

            ViewBag.Direccion = direccion;
            ViewBag.Genero = genero;
            ViewBag.Edad = edad;

            return View(pacientes.ToList());
        }

        public ActionResult ExportarExcelDatosPaciente(string direccion, string genero, int? edad)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var pacientes = _context.Users.AsQueryable();

            // Filtro para Direccion
            if (!string.IsNullOrEmpty(direccion))
            {
                pacientes = pacientes.Where(r => r.Direccion.Contains(direccion));
            }

            // Filtro para Genero
            if (!string.IsNullOrEmpty(genero))
            {
                pacientes = pacientes.Where(r => r.Genero_Paciente.ToString() == genero);
            }

            // Filtro para Edad
            if (edad.HasValue)
            {
                pacientes = pacientes.Where(r => r.Edad_Paciente == edad);
            }

            var listaPacientes = pacientes.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Informe de Pacientes");

                worksheet.Cells["A1"].Value = "Dirección";
                worksheet.Cells["B1"].Value = "Género";
                worksheet.Cells["C1"].Value = "Edad";
                worksheet.Cells["D1"].Value = "Nombre ";
                worksheet.Cells["E1"].Value = "Apellido";
                worksheet.Cells["F1"].Value = "Correo Electónico";

                using (var range = worksheet.Cells["A1:F1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#26a69a"));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                int row = 2;
                foreach (var paciente in listaPacientes)
                {
                    worksheet.Cells[row, 1].Value = paciente.Direccion;
                    worksheet.Cells[row, 2].Value = paciente.Genero_Paciente;
                    worksheet.Cells[row, 3].Value = paciente.Edad_Paciente;
                    worksheet.Cells[row, 4].Value = paciente.Nombre;
                    worksheet.Cells[row, 5].Value = paciente.Apellido;
                    worksheet.Cells[row, 6].Value = paciente.Email;

                    using (var range = worksheet.Cells[row, 1, row, 6])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    row++;
                }

                worksheet.Column(1).Width = 50;
                worksheet.Column(2).Width = 15;
                worksheet.Column(3).Width = 10;
                worksheet.Column(4).Width = 30;
                worksheet.Column(5).Width = 20;
                worksheet.Column(6).Width = 50;

                string fileName = "Informe_Pacientes";

                if (!string.IsNullOrEmpty(direccion))
                {
                    fileName += "_" + direccion.Replace(" ", "_");
                }

                if (!string.IsNullOrEmpty(genero))
                {
                    fileName += "_" + genero;
                }

                if (edad.HasValue)
                {
                    fileName += "_Edad_" + edad.Value;
                }

                fileName += ".xlsx";

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        //Historia de usuario 08 
        [Authorize(Roles = "Administrador")]
        public ActionResult AtencionCliente(string SaludEvaluada)
        {
            var saludevaluada = _context.Atencion_Cliente.AsQueryable();
            if (!string.IsNullOrEmpty(SaludEvaluada))
            {
                saludevaluada = saludevaluada.Where(r => r.Tipo_Servicio.Contains(SaludEvaluada));
            }

            ViewBag.AreaSalud = SaludEvaluada;
            return View(saludevaluada.ToList());
        }

        public ActionResult ExportarExcelAtencionCliente(string SaludEvaluada)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var saludevaluada = _context.Atencion_Cliente.AsQueryable();

            if (!string.IsNullOrEmpty(SaludEvaluada))
            {
                saludevaluada = saludevaluada.Where(r => r.Tipo_Servicio.Contains(SaludEvaluada));
            }

            var listaAreaS = saludevaluada.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Informe de Areas de Salud Evaluadas");

                // Encabezados de tabla
                worksheet.Cells["A1"].Value = "Fecha de la evaluación";
                worksheet.Cells["B1"].Value = "Área de salud";
                worksheet.Cells["C1"].Value = "Comentario del paciente";
                worksheet.Cells["D1"].Value = "Prioridad de mejora";
                worksheet.Cells["E1"].Value = "Tipo de servicio";
                worksheet.Cells["F1"].Value = "Clasificación del problema";



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
                foreach (var Area in listaAreaS)
                {
                    worksheet.Cells[row, 1].Value = Area.Fechas_Comentario.ToString("dd/MM/yy");
                    worksheet.Cells[row, 2].Value = Area.Salud_Evaluada;
                    worksheet.Cells[row, 3].Value = Area.Comentarios_Paciente;
                    worksheet.Cells[row, 4].Value = Area.Prioridad_Mejora;
                    worksheet.Cells[row, 5].Value = Area.Tipo_Servicio;
                    worksheet.Cells[row, 6].Value = Area.Clasificacion_Problema;

                    using (var range = worksheet.Cells[row, 1, row, 6])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    row++;
                }

                // Configuración de ancho de columnas
                worksheet.Column(1).Width = 20; 
                worksheet.Column(2).Width = 25; 
                worksheet.Column(3).Width = 30; 
                worksheet.Column(4).Width = 25; 
                worksheet.Column(5).Width = 20;
                worksheet.Column(6).Width = 25; 

                var stream = new MemoryStream(package.GetAsByteArray());
                string fileName = "Informe_Atencion_Cliente";

                if (!string.IsNullOrEmpty(SaludEvaluada))
                {
                    fileName += "_" + SaludEvaluada.Replace(" ", "_");
                }

                fileName += ".xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        //Historia de usuario 10 
        [Authorize(Roles = "Administrador")]
        public ActionResult Monitoreo(string usuarios)
        {
            var Usuarios = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(usuarios))
            {
                Usuarios = Usuarios.Where(r => r.Nombre.Contains(usuarios));
            }

            ViewBag.Nombre = usuarios;
            return View(Usuarios.ToList());
        }

        public ActionResult ExportarExcelMonitoreo(string usuarios)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var Usuarios = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(usuarios))
            {
                Usuarios = Usuarios.Where(r => r.Nombre.Contains(usuarios));
            }

            var listaAreaS = Usuarios.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Informe de Areas de Salud Evaluadas");

                // Encabezados de tabla
                worksheet.Cells["A1"].Value = "Nombre";
                worksheet.Cells["B1"].Value = "Apellidos";
                worksheet.Cells["C1"].Value = "Dirección";
                worksheet.Cells["D1"].Value = "Genero";
                worksheet.Cells["E1"].Value = "Edad";
                worksheet.Cells["F1"].Value = "Correo";



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
                foreach (var Area in listaAreaS)
                {
                    worksheet.Cells[row, 1].Value = Area.Nombre;
                    worksheet.Cells[row, 2].Value = Area.Apellido;
                    worksheet.Cells[row, 3].Value = Area.Direccion;
                    worksheet.Cells[row, 4].Value = Area.Genero_Paciente;
                    worksheet.Cells[row, 5].Value = Area.Edad_Paciente;
                    worksheet.Cells[row, 6].Value = Area.Email;

                    using (var range = worksheet.Cells[row, 1, row, 6])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    row++;
                }

                // Configuración de ancho de columnas
                worksheet.Column(1).Width = 20;
                worksheet.Column(2).Width = 25;
                worksheet.Column(3).Width = 30;
                worksheet.Column(4).Width = 25;
                worksheet.Column(5).Width = 20;
                worksheet.Column(6).Width = 50;

                var stream = new MemoryStream(package.GetAsByteArray());
                string fileName = "Informe_Usuarios";

                if (!string.IsNullOrEmpty(usuarios))
                {
                    fileName += "_" + usuarios.Replace(" ", "_");
                }

                fileName += ".xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        //Historia de usuario 12
        [Authorize(Roles = "Administrador")]
        public ActionResult Rempleados(string nombreEmpleado)
        {
            var empleado = _context.Empleado.AsQueryable();
            if (!string.IsNullOrEmpty(nombreEmpleado))
            {
                empleado = empleado.Where(r => r.Nombre.Contains(nombreEmpleado));
            }
            ViewBag.NombreEmpleado = nombreEmpleado;
            return View(empleado.ToList());
        }

        public ActionResult ExportarExcelRempleados(string nombreEmpleado)
        {
            // Establecer el contexto de la licencia de EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var empleado = _context.Empleado.AsQueryable();

            if (!string.IsNullOrEmpty(nombreEmpleado))
            {
                empleado = empleado.Where(r => r.Nombre.Contains(nombreEmpleado));
            }

            var listaempleado = empleado.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Informe de empleados");

                // Encabezados de tabla
                worksheet.Cells["A1"].Value = "Fecha de creación";
                worksheet.Cells["B1"].Value = "Estado";
                worksheet.Cells["C1"].Value = "Comentarios del administrador";
                worksheet.Cells["D1"].Value = "Nombre";
                worksheet.Cells["E1"].Value = "Apellidos";
                worksheet.Cells["F1"].Value = "Cédula";
                worksheet.Cells["G1"].Value = "Correo";
                worksheet.Cells["H1"].Value = "Jornada";
                worksheet.Cells["I1"].Value = "Departamento";


                // Estilo de encabezado
                using (var range = worksheet.Cells["A1:I1"])
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
                foreach (var empleadox in listaempleado)
                {
                    worksheet.Cells[row, 1].Value = empleadox.Fecha_registro.ToString("dd/MM/yy");
                    worksheet.Cells[row, 2].Value = empleadox.Estado.Descripcion;
                    worksheet.Cells[row, 3].Value = empleadox.Comentarios;
                    worksheet.Cells[row, 4].Value = empleadox.Nombre;
                    worksheet.Cells[row, 5].Value = empleadox.Apellido;
                    worksheet.Cells[row, 6].Value = empleadox.Cedula;
                    worksheet.Cells[row, 7].Value = empleadox.Correo;
                    worksheet.Cells[row, 8].Value = empleadox.Jornada;
                    worksheet.Cells[row, 9].Value = empleadox.Departamento;


                    using (var range = worksheet.Cells[row, 1, row, 9])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    row++;
                }

                // Configuración de ancho de columnas
                worksheet.Column(1).Width = 20;
                worksheet.Column(2).Width = 25; 
                worksheet.Column(3).Width = 50; 
                worksheet.Column(4).Width = 25;
                worksheet.Column(5).Width = 20; 
                worksheet.Column(6).Width = 25;
                worksheet.Column(7).Width = 50;
                worksheet.Column(8).Width = 25;
                worksheet.Column(9).Width = 25;


                var stream = new MemoryStream(package.GetAsByteArray());
                string fileName = "Informe_Empleado";

                if (!string.IsNullOrEmpty(nombreEmpleado))
                {
                    fileName += "_" + nombreEmpleado.Replace(" ", "_");
                }

                fileName += ".xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }


    }
}
   