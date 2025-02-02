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

        [HttpGet]
        public ActionResult RealizarPago()
        {
            ViewBag.Servicios = new MultiSelectList(
                _context.Servicio.ToList(),
                "Id_Servicio",
                "Nombre_Servicio"
            );
            return View();
        }

        [HttpPost]
        public ActionResult RealizarPago(int[] serviciosSeleccionados)
        {
            if (serviciosSeleccionados == null || serviciosSeleccionados.Length == 0)
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
            ViewBag.Subtotal = subtotal.ToString("C");
            ViewBag.Impuesto = impuesto.ToString("C");
            ViewBag.Total = total.ToString("C");

            ViewBag.MetodosPago = new[]
            {
        "Efectivo",
        "Tarjeta",
        "Transferencia",
        "Crédito"
    };

            return View("DetallesPago");
        }

        [HttpPost]
        public ActionResult DetallesPago(int[] serviciosSeleccionados, string[] metodosPago, string codigoDescuento, Dictionary<string, decimal> pagosPorMetodo = null)
        {
            var servicios = _context.Servicio
                .Where(s => serviciosSeleccionados.Contains(s.Id_Servicio))
                .ToList();

            var subtotal = servicios.Sum(s => s.Precio_Servicio);
            var descuento = string.Equals(codigoDescuento, "ADULTO", System.StringComparison.OrdinalIgnoreCase) ? subtotal * 0.15m : 0;
            var impuesto = (subtotal - descuento) * 0.13m;
            var total = (subtotal - descuento) + impuesto;

            ViewBag.ServiciosSeleccionados = servicios;
            ViewBag.MetodosPagoSeleccionados = metodosPago;
            ViewBag.Subtotal = subtotal.ToString("C");
            ViewBag.Descuento = descuento.ToString("C");
            ViewBag.Impuesto = impuesto.ToString("C");
            ViewBag.Total = total.ToString("C");

            ViewBag.PagosPorMetodo = pagosPorMetodo ?? new Dictionary<string, decimal>();

            ViewBag.MetodosPago = new[]
            {
        "Efectivo",
        "Tarjeta",
        "Transferencia",
        "Crédito"
    };

            return View("DetallesPago");
        }





    }
}
