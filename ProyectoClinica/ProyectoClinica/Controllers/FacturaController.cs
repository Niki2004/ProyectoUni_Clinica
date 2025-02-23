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
            var factura = _context.Factura.Find(idFactura);
            if (factura == null)
            {
                return Json(new { success = false, message = "Factura no encontrada" });
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

            string mensaje = $"Factura No: {factura.Id_Factura}\n" +
                             $"Fecha: {factura.FechaHora:dd/MM/yyyy HH:mm}\n" +
                             $"Total Pagado: ${factura.TotalPagado:N2}\n\n" +
                             "Métodos de Pago Utilizados:\n" +
                             (metodosPago.Any() ? string.Join("\n", metodosPago) : "No registrados") + "\n\n" +
                             "Servicios Brindados:\n" +
                             (servicios.Any() ? string.Join("\n", servicios) : "No registrados");

            try
            {
                // Aquí puedes integrar un servicio de correo real como SendGrid o SMTP
                Debug.WriteLine($"Enviando factura al correo: {correo}\n{mensaje}");
                return Json(new { success = true, message = "Factura enviada con éxito" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al enviar el correo: {ex.Message}" });
            }
        }


        [HttpPost]
        public async Task<JsonResult> EnviarFacturaCorreo(int idFactura, List<string> correos)
        {
            try
            {
                if (correos == null || !correos.Any())
                {
                    return Json(new { success = false, message = "❌ No se han proporcionado correos electrónicos." });
                }

                var factura = _context.Factura.Find(idFactura);
                if (factura == null)
                {
                    return Json(new { success = false, message = "❌ Factura no encontrada." });
                }

                string apiKey = WebConfigurationManager.AppSettings["ResendApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                {
                    return Json(new { success = false, message = "❌ API Key de Resend no configurada." });
                }

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", apiKey);

                    var emailRequest = new
                    {
                        from = "onboarding@resend.dev",
                        to = correos,
                        subject = $"Factura #{factura.Id_Factura}",
                        html = $"<h2>Factura de Pago</h2><p><strong>Total:</strong> ${factura.TotalPagado:N2}</p>"
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(emailRequest), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("https://api.resend.com/emails", content);
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        return Json(new { success = false, message = $"❌ Error en Resend: {response.StatusCode} - {responseContent}" });
                    }
                }

                return Json(new { success = true, message = "✅ Factura enviada con éxito." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"❌ Error en el servidor: {ex.Message}" });
            }
        }
    




}
}
