using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult ConfirmarPago(Factura factura, Dictionary<int, decimal> pagosPorMetodo, int[] serviciosSeleccionados)
        {
            if (factura == null || pagosPorMetodo == null || pagosPorMetodo.Values.Sum() != factura.TotalPagado)
            {
                ModelState.AddModelError("", "El total de los pagos no coincide con el total de la factura.");
                return RedirectToAction("RealizarPago");
            }

            if (factura.Descuento == null)
            {
                factura.Descuento = _context.Descuento
                    .FirstOrDefault(d => d.Nombre_Descuento == "Sin Descuento");
                factura.Descuento_Aplicado = 0;
            }
            factura.FechaHora = DateTime.Now;
            _context.Factura.Add(factura);
            _context.SaveChanges();

            // Guardar los métodos de pago utilizados
            foreach (var metodoPago in pagosPorMetodo)
            {
                var metodoPagoUtilizado = new Metodo_Pago_Utilizado
                {
                    Id_Factura = factura.Id_Factura,
                    Id_MetodoPago = metodoPago.Key,
                    Monto = metodoPago.Value
                };
                _context.Metodo_Pago_Utilizado.Add(metodoPagoUtilizado);
            }

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

            TempData["SuccessMessage"] = "Factura registrada exitosamente.";
            return RedirectToAction("Index");
        }






    }
}
