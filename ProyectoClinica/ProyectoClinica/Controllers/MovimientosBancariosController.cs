using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ProyectoClinica.Controllers
{
    public class MovimientosBancariosController : Controller
    {

        private readonly ApplicationDbContext _context;

        public MovimientosBancariosController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: MovimientosBancarios
        public ActionResult Index()
        {
            var listaRegistros = _context.Movimientos_Bancarios.ToList();
            return View(listaRegistros);
        }

        // GET: MovimientosBancarios/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MovimientosBancarios/Create
        public ActionResult Create()
        {

            ViewBag.Id_Diario = new SelectList(_context.Diarios_Contables, "Id_Diario", "Codigo_Diario");
            ViewBag.Id_Conciliacion = new SelectList(_context.Conciliaciones_Bancarias, "Id_Conciliacion", "Saldo_Contable");
            ViewBag.Id_Pagos_Diarios = new SelectList(_context.Pagos_Diarios, "Id_Pagos_Diarios", "Monto");
            return View();
        }

        // POST: MovimientosBancarios/Create
        [HttpPost]
        public ActionResult Create(Movimientos_Bancarios model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        //Guardar en la base de datos
                        _context.Movimientos_Bancarios.Add(model);
                        _context.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        // Si hay un error, muestra el mensaje en el log o en el modelo para que el usuario lo vea
                        ModelState.AddModelError("", "Ocurrió un error al guardar los datos: " + ex.Message);
                    }



                }

                // Si el modelo no es válido o hubo un error, repite el proceso y pasa la vista con el modelo
                // Esto permitirá que los datos enviados por el usuario se mantengan en el formulario
                ViewBag.Id_Diario = new SelectList(_context.Diarios_Contables, "Id_Diario", "Codigo_Diario");
                ViewBag.Id_Conciliacion = new SelectList(_context.Conciliaciones_Bancarias, "Id_Conciliacion", "Saldo_Contable");
                ViewBag.Id_Pagos_Diarios = new SelectList(_context.Pagos_Diarios, "Id_Pagos_Diarios", "Monto");
                return View(model);


            }
            catch
            {
                return View();
            }
        }

        // GET: MovimientosBancarios/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MovimientosBancarios/Edit/5
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

        // GET: MovimientosBancarios/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MovimientosBancarios/Delete/5
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
