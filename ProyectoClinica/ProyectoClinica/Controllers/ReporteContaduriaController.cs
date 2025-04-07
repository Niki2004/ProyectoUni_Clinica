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

                // Encabezados de tabla
                worksheet.Cells["A1"].Value = "Fecha del registro";
                worksheet.Cells["B1"].Value = "Fecha de vencimiento";
                worksheet.Cells["C1"].Value = "Proveedor";
                worksheet.Cells["D1"].Value = "Pagos";
                worksheet.Cells["E1"].Value = "Monto Anticipado";
                worksheet.Cells["F1"].Value = "Impuestos Aplicados";
                worksheet.Cells["G1"].Value = "Comentarios";

                // Estilo de encabezado
                using (var range = worksheet.Cells["A1:G1"])
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
                    worksheet.Cells[row, 1].Value = Area.Fecha_Registro.ToString("dd/MM/yy");
                    worksheet.Cells[row, 2].Value = Area.Fecha_Vencimiento.ToString("dd/MM/yy");
                    worksheet.Cells[row, 3].Value = Area.ClienteProveedor;
                    worksheet.Cells[row, 4].Value = Area.Conta_pago;
                    worksheet.Cells[row, 5].Value = Area.Monto_Anticipo;
                    worksheet.Cells[row, 6].Value = Area.Impuesto_Aplicado;
                    worksheet.Cells[row, 7].Value = Area.Comentarios;

                    using (var range = worksheet.Cells[row, 1, row, 7])
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
                worksheet.Column(6).Width = 50;

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

                // Encabezados de tabla
                worksheet.Cells["A1"].Value = "Fecha de Movimiento";
                worksheet.Cells["B1"].Value = "Tipo de Movimiento";
                worksheet.Cells["C1"].Value = "Concepto";
                worksheet.Cells["D1"].Value = "Monto";
                worksheet.Cells["E1"].Value = "Saldo Actual";
                worksheet.Cells["F1"].Value = "Saldo Anterior";
                worksheet.Cells["G1"].Value = "Beneficiario";



                // Estilo de encabezado
                using (var range = worksheet.Cells["A1:G1"])
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
                    worksheet.Cells[row, 1].Value = Area.Fecha_Movimiento.ToString("dd/MM/yy");
                    worksheet.Cells[row, 2].Value = Area.Tipo_Movimiento;
                    worksheet.Cells[row, 3].Value = Area.Concepto;
                    worksheet.Cells[row, 4].Value = Area.Monto;
                    worksheet.Cells[row, 5].Value = Area.Saldo_Actual;
                    worksheet.Cells[row, 6].Value = Area.Saldo_Anterior;
                    worksheet.Cells[row, 7].Value = Area.Beneficiario;

                    using (var range = worksheet.Cells[row, 1, row, 7])
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
                worksheet.Column(7).Width = 25;

                var stream = new MemoryStream(package.GetAsByteArray());
                string fileName = "Informe_Tipo_Movimiento";

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