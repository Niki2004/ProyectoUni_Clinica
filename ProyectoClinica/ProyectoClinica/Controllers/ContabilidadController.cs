using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoClinica.Controllers
{
    public class ContabilidadController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ContabilidadController()
        {
            _context = new ApplicationDbContext();
        }

        #region Index
        // GET: Contabilidad
        public ActionResult Index()
        {
            var registros = new List<Contabilidad>
            {
                new Contabilidad
                {
                    Id_Tipo_Registro = 1,
                    Id_Estado_Contabilidad = 2,
                    Id_Tipo_Transaccion = 3,
                    ClienteProveedor = "Empresa XYZ",
                    Fecha_Registro = DateTime.Now,
                    Fecha_Vencimiento = DateTime.Now.AddDays(30),
                    Monto = 1500.00m,
                    Monto_Anticipo = 500.00m,
                    Impuesto_Aplicado = 18.00m,
                    Descuento_Aplicado = 50.00m,
                    Comentarios = "Pago parcial",
                    Fecha_Cierre = DateTime.Now,
                    Ingresos_Totales = 5000.00m,
                    Total_Pagos_Pendientes = 2000.00m,
                    Total_Sueldos = 300000,
                    Observaciones_Ingresos = "Pendiente de revisión"
                },
                new Contabilidad
                {
                    Id_Tipo_Registro = 2,
                    Id_Estado_Contabilidad = 1,
                    Id_Tipo_Transaccion = 4,
                    ClienteProveedor = "Cliente ABC",
                    Fecha_Registro = DateTime.Now.AddDays(-10),
                    Fecha_Vencimiento = DateTime.Now.AddDays(20),
                    Monto = 2500.00m,
                    Monto_Anticipo = 1000.00m,
                    Impuesto_Aplicado = 20.00m,
                    Descuento_Aplicado = 100.00m,
                    Comentarios = "Factura generada",
                    Fecha_Cierre = DateTime.Now.AddDays(5),
                    Ingresos_Totales = 7000.00m,
                    Total_Pagos_Pendientes = 1500.00m,
                    Total_Sueldos = 400000,
                    Observaciones_Ingresos = "Verificado"
                }
            };

            return View(registros);
        }
        #endregion

        #region Detalles de contabilidad
        // GET: Contabilidad/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        #endregion

        // GET: Contabilidad/Create
        public ActionResult Create()
        {
            //tipo de Registro
            ViewBag.TipoRegistro = new SelectList(_context.Tipo_Registro, "Id_Tipo_Registro", "Nombre");
            ViewBag.Estado_Contabilidad = new SelectList(_context.Estado_Contabilidad, "Id_Estado_Contabilidad", "Nombre");
            ViewBag.TipoTransaccion = new SelectList(_context.Tipo_Transaccion, "Id_Tipo_Transaccion", "Nombre");
            return View();
        }

        // POST: Contabilidad/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Contabilidad/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Contabilidad/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Contabilidad/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Contabilidad/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
