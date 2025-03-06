using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;

namespace ProyectoClinica.Controllers
{
    public class FacturaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacturaController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Descuento

        [HttpGet]
        public ActionResult VistaSEC()
        {

            return View();
        }


        public ActionResult Index()
        {
            
            return View();
        }

       

        // GET: Cargar servicios y métodos de pago
        [HttpGet]
        public ActionResult RealizarPago()
        {
            ViewBag.Servicios = new MultiSelectList(
                _context.Servicio.ToList(),
                "Id_Servicio",
                "Nombre_Servicio"
            );

            ViewBag.MetodosPago = _context.Metodo_Pago.ToList();
            return View();
        }

        // POST: Procesar servicios seleccionados y mostrar detalle
        [HttpPost]
        public ActionResult RealizarPago(int[] serviciosSeleccionados)
        {
            if (serviciosSeleccionados == null || !serviciosSeleccionados.Any())
            {
                ModelState.AddModelError("", "Debe seleccionar al menos un servicio.");
                return RedirectToAction("RealizarPago");
            }

            var servicios = _context.Servicio
                .Where(s => serviciosSeleccionados.Contains(s.Id_Servicio))
                .ToList();

            var subtotal = servicios.Sum(s => s.Precio_Servicio);
            var impuesto = subtotal * 0.13m;
            var total = subtotal + impuesto;

            ViewBag.ServiciosSeleccionados = servicios;
            ViewBag.ServiciosIds = string.Join(",", serviciosSeleccionados); // IDs de los servicios seleccionados
            ViewBag.Subtotal = subtotal;
            ViewBag.Impuesto = impuesto;
            ViewBag.Total = total;

            ViewBag.MetodosPago = _context.Metodo_Pago.ToList();
            return View("DetallesPago");
        }

        [HttpPost]
        public ActionResult GenerarFactura(int[] serviciosSeleccionados, int[] metodosPagoSeleccionados, string cedulaCliente, string nombreCliente)
        {
            if (serviciosSeleccionados == null || !serviciosSeleccionados.Any())
            {
                ModelState.AddModelError("", "No se seleccionaron servicios.");
                return RedirectToAction("RealizarPago");
            }

            if (metodosPagoSeleccionados == null || !metodosPagoSeleccionados.Any())
            {
                ModelState.AddModelError("", "Debe seleccionar al menos un método de pago.");
                return RedirectToAction("DetallesPago");
            }

            var servicios = _context.Servicio
                .Where(s => serviciosSeleccionados.Contains(s.Id_Servicio))
                .ToList();
            var metodos = _context.Metodo_Pago
                .Where(s => metodosPagoSeleccionados.Contains(s.Id_MetodoPago))
                .ToList();

            var subtotal = servicios.Sum(s => s.Precio_Servicio);
            var impuesto = subtotal * 0.13m;
            var total = subtotal + impuesto;

            var factura = new Factura
            {
                CedulaCliente = cedulaCliente,
                NombreCliente = nombreCliente,
                FechaHora = DateTime.Now,
                Subtotal = subtotal,
                Impuesto = impuesto,
                TotalPagado = total
            };

            ViewBag.ServiciosSeleccionados = servicios;
            ViewBag.MetodosPagoSeleccionados = metodos;
           

            return View("Factura", factura);
        }




        [HttpPost]
        public ActionResult ConfirmarPago(Factura factura, int[] metodosPagoSeleccionados, int[] serviciosSeleccionados)
        {

            Debug.WriteLine("Factura Recibida:");
            Debug.WriteLine($"CedulaCliente: {factura.CedulaCliente}");
            Debug.WriteLine($"NombreCliente: {factura.NombreCliente}");
            Debug.WriteLine($"Subtotal: {factura.Subtotal}");
            Debug.WriteLine($"Impuesto: {factura.Impuesto}");
            Debug.WriteLine($"TotalPagado: {factura.TotalPagado}");


            if (factura == null && metodosPagoSeleccionados == null && serviciosSeleccionados == null)
            {
                ModelState.AddModelError("", "Alguno de los items es nulo");
                return RedirectToAction("RealizarPago");
            }

            if (factura.Id_Descuento == 0) {
                factura.Id_Descuento = 1;
            }


            factura.FechaHora = DateTime.Now;
            _context.Factura.Add(factura);
            _context.SaveChanges();

            // Guardar los métodos de pago utilizados
            foreach (var metodoPago in metodosPagoSeleccionados)
            {
                var metodoPagoUtilizado = new Metodo_Pago_Utilizado
                {
                    Id_Factura = factura.Id_Factura, 
                    Id_MetodoPago = metodoPago,
                    Monto = 0
                };
                _context.Metodo_Pago_Utilizado.Add(metodoPagoUtilizado);
            }
            _context.SaveChanges(); 
        

            // Guardar los servicios brindados
            foreach (var servicioId in serviciosSeleccionados)
            {
                var servicioBrindado = new Servicios_Brindados
                {
                    Id_Factura = factura.Id_Factura,
                    Id_Servicio = servicioId
                };
                _context.Servicios_Brindados.Add(servicioBrindado);
            }

            _context.SaveChanges();

           
            return RedirectToAction("OpcionesDespuesDePago", new { idFactura = factura.Id_Factura });
        }


        public ActionResult OpcionesDespuesDePago(int idFactura)
        {
            var factura = _context.Factura.Find(idFactura);
            if (factura == null)
            {
                return HttpNotFound();
            }

            // Obtener métodos de pago utilizados
            var metodosPago = _context.Metodo_Pago_Utilizado
                .Where(mp => mp.Id_Factura == idFactura)
                .Select(mp => mp.MetodoPago.Nombre)
                .ToList();

            // Obtener servicios brindados
            var servicios = _context.Servicios_Brindados
                .Where(sb => sb.Id_Factura == idFactura)
                .Select(sb => sb.Servicio.Nombre_Servicio)
                .ToList();

            // Pasar los datos a la vista
            ViewBag.MetodosPago = metodosPago;
            ViewBag.Servicios = servicios;

            return View(factura);
        }



        [HttpPost]
        public JsonResult EnviarFacturaCorreo(int idFactura, string correo)
        {
            try
            {
                var factura = _context.Factura.Find(idFactura);
                if (factura == null)
                {
                    return Json(new { success = false, message = "❌ Factura no encontrada." });
                }

                if (string.IsNullOrEmpty(correo))
                {
                    return Json(new { success = false, message = "❌ No se ha proporcionado un correo electrónico." });
                }

                // Obtener Métodos de Pago Utilizados
                var metodosPago = _context.Metodo_Pago_Utilizado
                    .Where(mp => mp.Id_Factura == idFactura)
                    .Select(mp => mp.MetodoPago.Nombre)
                    .ToList();

                // Obtener Servicios Brindados con sus Precios
                var servicios = _context.Servicios_Brindados
                    .Where(sb => sb.Id_Factura == idFactura)
                    .Select(sb => new { sb.Servicio.Nombre_Servicio, sb.Servicio.Precio_Servicio })
                    .ToList();

                // Construir el contenido del correo con todos los detalles de la factura
                var body = new StringBuilder();
                body.Append("<html><body style='font-family: Arial, sans-serif;'>");
                body.Append("<h2 style='color:#4CAF50;'>Factura de Pago</h2>");
                body.Append($"<p><strong>Factura No:</strong> {factura.Id_Factura}</p>");
                body.Append($"<p><strong>Fecha:</strong> {factura.FechaHora:dd/MM/yyyy HH:mm}</p>");
                body.Append($"<p><strong>Cliente:</strong> {factura.NombreCliente}</p>");
                body.Append($"<p><strong>Cédula:</strong> {factura.CedulaCliente}</p>");
                body.Append($"<p><strong>Subtotal:</strong> ₡{factura.Subtotal:N2}</p>");
                body.Append($"<p><strong>Descuento:</strong> -₡{factura.Descuento:N2}</p>");
                body.Append($"<p><strong>Impuesto:</strong> ₡{factura.Impuesto:N2}</p>");
                body.Append($"<h3 style='color:blue;'>Total Pagado: <strong>₡{factura.TotalPagado:N2}</strong></h3>");

                // Métodos de Pago Utilizados
                body.Append("<h3>Método de Pago:</h3><ul>");
                if (metodosPago.Any())
                {
                    foreach (var metodo in metodosPago)
                    {
                        body.Append($"<li>{metodo}</li>");
                    }
                }
                else
                {
                    body.Append("<li>No registrado</li>");
                }
                body.Append("</ul>");

                // Servicios Brindados con Precios
                body.Append("<h3>Servicios Brindados:</h3><table border='1' cellpadding='5' cellspacing='0' style='width:100%;border-collapse:collapse;'>");
                body.Append("<tr style='background-color:#f2f2f2;'><th>Servicio</th><th>Precio</th></tr>");
                if (servicios.Any())
                {
                    foreach (var servicio in servicios)
                    {
                        body.Append($"<tr><td>{servicio.Nombre_Servicio}</td><td style='text-align:right;'>₡{servicio.Precio_Servicio:N2}</td></tr>");
                    }
                }
                else
                {
                    body.Append("<tr><td colspan='2' style='text-align:center;'>No registrados</td></tr>");
                }
                body.Append("</table>");
                body.Append("</body></html>");

                // Configuración SMTP desde `web.config`
                string smtpServer = WebConfigurationManager.AppSettings["SmtpServer"];
                int smtpPort = int.Parse(WebConfigurationManager.AppSettings["SmtpPort"]);
                string smtpUser = WebConfigurationManager.AppSettings["SmtpUser"];
                string smtpPassword = WebConfigurationManager.AppSettings["SmtpPassword"];

                MailMessage mensaje = new MailMessage
                {
                    From = new MailAddress("facturas@demo.com", "Centro Integral Santo Domingo"),
                    Subject = $"Factura #{factura.Id_Factura}",
                    Body = body.ToString(),
                    IsBodyHtml = true
                };

                mensaje.To.Add(new MailAddress(correo));

                using (SmtpClient smtp = new SmtpClient(smtpServer, smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(smtpUser, smtpPassword);
                    smtp.EnableSsl = true; // Habilita seguridad TLS
                    smtp.Send(mensaje);
                }

                return Json(new { success = true, message = "✅ Factura enviada con éxito." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"❌ Error al enviar el correo: {ex.Message}" });
            }
        }





    }
}
