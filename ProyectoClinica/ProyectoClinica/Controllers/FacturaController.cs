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
            ViewBag.Subtotal = subtotal;
            ViewBag.Impuesto = impuesto;
            ViewBag.Total = total;

            ViewBag.MetodosPago = _context.Metodo_Pago.ToList();
            return View("DetallesPago");
        }



        [HttpPost]
        public ActionResult GenerarFactura(int[] serviciosSeleccionados,decimal total,decimal impuesto,Dictionary<int, 
            decimal> pagosPorMetodo,string cedulaCliente,string nombreCliente)
        {
            

            // Obtener los servicios seleccionados
            var servicios = _context.Servicio
                .Where(s => serviciosSeleccionados.Contains(s.Id_Servicio))
                .ToList();

            // Crear la factura
            var factura = new Factura
            {
                NumeroRecibo = "A-" + DateTime.Now.Ticks.ToString().Substring(0, 6),
                FechaHora = DateTime.Now,
                MetodoPago = "Múltiples Métodos",
                CedulaCliente = cedulaCliente,
                NombreCliente = nombreCliente,
                Subtotal = total - impuesto,
                Impuesto = impuesto,
                TotalPagado = total
            };

            // Guardar la factura en la base de datos
            _context.Factura.Add(factura);
            _context.SaveChanges();

            // Asociar los métodos de pago utilizados
            foreach (var pago in pagosPorMetodo)
            {
                var metodoPagoUtilizado = new Metodo_Pago_Utilizado
                {
                    Id_Factura = factura.Id_Factura,
                    Id_MetodoPago = pago.Key,
                    Monto = pago.Value
                };
                _context.Metodo_Pago_Utilizado.Add(metodoPagoUtilizado);
            }

            _context.SaveChanges();

            // Preparar la vista
            ViewBag.ServiciosSeleccionados = servicios;
            ViewBag.Factura = factura;
            ViewBag.PagosPorMetodo = pagosPorMetodo;

            return View("Factura", factura);
        }






    }
}
