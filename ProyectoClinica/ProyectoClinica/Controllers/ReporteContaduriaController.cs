using OfficeOpenXml.Style;
using OfficeOpenXml;
using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoClinica.Controllers
{
    public class ReporteContaduriaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReporteContaduriaController()
        {
            _context = new ApplicationDbContext();
        }

        //Historia de usuario 09 
        [Authorize(Roles = "Administrador")]
        public ActionResult ReporteFinanciero(string TipoTransaccion)
        {
            var tipotransaccion = _context.Contabilidad.AsQueryable();
            if (!string.IsNullOrEmpty(TipoTransaccion))
            {
                tipotransaccion = tipotransaccion.Where(r => r.Estatus_pago.Contains(TipoTransaccion));
            }

            ViewBag.Transaccion = TipoTransaccion;
            return View(tipotransaccion.ToList());
        }

        public ActionResult ExportarExcelFinanciero(string TipoTransaccion)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var tipotransaccion = _context.Contabilidad.AsQueryable();

            if (!string.IsNullOrEmpty(TipoTransaccion))
            {
                tipotransaccion = tipotransaccion.Where(r => r.Estatus_pago.Contains(TipoTransaccion));
            }

            var listaAreaS = tipotransaccion.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Informe de Contabilidad");

                // Título grande con fondo verde claro
                using (var range = worksheet.Cells["A1:G1"])
                {
                    range.Merge = true;
                    range.Value = "Informe de Contabilidad Financiera";
                    range.Style.Font.Bold = true;
                    range.Style.Font.Size = 16;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#B8E6B8")); // Verde claro
                    range.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                worksheet.Row(1).Height = 30; // Altura del título

                // Encabezados de tabla en la fila 3
                worksheet.Cells["A3"].Value = "Fecha del registro";
                worksheet.Cells["B3"].Value = "Fecha de vencimiento";
                worksheet.Cells["C3"].Value = "Proveedor";
                worksheet.Cells["D3"].Value = "Pagos";
                worksheet.Cells["E3"].Value = "Monto Anticipado";
                worksheet.Cells["F3"].Value = "Impuestos Aplicados";
                worksheet.Cells["G3"].Value = "Comentarios";

                // Estilo de encabezado
                using (var range = worksheet.Cells["A3:G3"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#6BB56B")); // Verde más oscuro
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }

                // Agregar datos
                int row = 4;
                foreach (var Area in listaAreaS)
                {
                    worksheet.Cells[row, 1].Value = Area.Fecha_Registro.ToString("dd/MM/yy");
                    worksheet.Cells[row, 2].Value = Area.Fecha_Vencimiento.ToString("dd/MM/yy");
                    worksheet.Cells[row, 3].Value = Area.ClienteProveedor;
                    worksheet.Cells[row, 4].Value = Area.Conta_pago;
                    worksheet.Cells[row, 5].Value = Area.Monto_Anticipo;
                    worksheet.Cells[row, 6].Value = Area.Impuesto_Aplicado;
                    worksheet.Cells[row, 7].Value = Area.Comentarios;

                    // Estilo de las filas alternas
                    using (var range = worksheet.Cells[row, 1, row, 7])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        // Colorear filas pares con verde claro
                        if (row % 2 == 0)
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#E8F5E8")); // Verde claro
                        }
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
                worksheet.Column(7).Width = 50;

                // Ajustar el alto de las filas
                worksheet.Row(3).Height = 25; // Encabezados

                // Crear bordes para toda la tabla
                using (var range = worksheet.Cells[3, 1, row - 1, 7])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }

                var stream = new MemoryStream(package.GetAsByteArray());
                string fileName = "Informe_Contaduria";

                if (!string.IsNullOrEmpty(TipoTransaccion))
                {
                    fileName += "_" + TipoTransaccion.Replace(" ", "_");
                }

                fileName += ".xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        //Historia de usuario 13 
        [Authorize(Roles = "Contador")]
        public ActionResult Impuestos(string TipoMovimiento)
        {
            var tipomovimiento = _context.Caja_Chica.AsQueryable();
            if (!string.IsNullOrEmpty(TipoMovimiento))
            {
                tipomovimiento = tipomovimiento.Where(r => r.Tipo_Movimiento.Contains(TipoMovimiento));
            }

            ViewBag.Tipomovimiento = TipoMovimiento;
            return View(tipomovimiento.ToList());
        }

        public ActionResult ExportarExcelImpuestos(string TipoMovimiento)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var Tipomovimiento = _context.Caja_Chica.AsQueryable();

            if (!string.IsNullOrEmpty(TipoMovimiento))
            {
                Tipomovimiento = Tipomovimiento.Where(r => r.Tipo_Movimiento.Contains(TipoMovimiento));
            }

            var listaAreaS = Tipomovimiento.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Informe de Movimientos");

                // Título grande con fondo verde claro
                using (var range = worksheet.Cells["A1:G1"])
                {
                    range.Merge = true;
                    range.Value = "Informe de Movimientos de Caja Chica";
                    range.Style.Font.Bold = true;
                    range.Style.Font.Size = 16;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#B8E6B8")); // Verde claro
                    range.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                worksheet.Row(1).Height = 30; // Altura del título

                // Encabezados en fila 2
                worksheet.Cells["A2"].Value = "Fecha de Movimiento";
                worksheet.Cells["B2"].Value = "Tipo de Movimiento";
                worksheet.Cells["C2"].Value = "Concepto";
                worksheet.Cells["D2"].Value = "Monto";
                worksheet.Cells["E2"].Value = "Saldo Actual";
                worksheet.Cells["F2"].Value = "Saldo Anterior";
                worksheet.Cells["G2"].Value = "Beneficiario";

                // Estilo de encabezado
                using (var range = worksheet.Cells["A2:G2"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#6BB56B")); // Verde más oscuro
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }

                int row = 3;
                foreach (var Area in listaAreaS)
                {
                    worksheet.Cells[row, 1].Value = Area.Fecha_Movimiento.ToString("dd/MM/yy");
                    worksheet.Cells[row, 2].Value = Area.Tipo_Movimiento;
                    worksheet.Cells[row, 3].Value = Area.Concepto;
                    worksheet.Cells[row, 4].Value = Area.Monto;
                    worksheet.Cells[row, 5].Value = Area.Saldo_Actual;
                    worksheet.Cells[row, 6].Value = Area.Saldo_Anterior;
                    worksheet.Cells[row, 7].Value = Area.Beneficiario;

                    // Estilo de las filas alternas
                    using (var range = worksheet.Cells[row, 1, row, 7])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        // Colorear filas pares con verde claro
                        if (row % 2 == 0)
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#E8F5E8")); // Verde claro
                        }
                    }
                    row++;
                }

                // Configuración de ancho de columnas
                worksheet.Column(1).Width = 20; // Fecha
                worksheet.Column(2).Width = 25; // Tipo de Movimiento
                worksheet.Column(3).Width = 30; // Concepto
                worksheet.Column(4).Width = 20; // Monto
                worksheet.Column(5).Width = 20; // Saldo Actual
                worksheet.Column(6).Width = 25; // Saldo Anterior
                worksheet.Column(7).Width = 25; // Beneficiario

                // Crear bordes para toda la tabla
                using (var range = worksheet.Cells[2, 1, row - 1, 7])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }

                var stream = new MemoryStream(package.GetAsByteArray());
                string fileName = "Informe_Movimientos_Caja_Chica";

                if (!string.IsNullOrEmpty(TipoMovimiento))
                {
                    fileName += "_" + TipoMovimiento.Replace(" ", "_");
                }

                fileName += ".xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}