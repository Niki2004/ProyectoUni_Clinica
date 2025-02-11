using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return View();

            var movimientos_bancarios = _context.Movimientos_Bancarios.Find(id);


            // Obtener la lista de bancos desde la base de datos
            var diario = _context.Diarios_Contables
                                 .Select(b => new SelectListItem
                                 {
                                     Value = b.Id_Diario.ToString(),
                                     Text = b.Codigo_Diario
                                 })
                                 .ToList();

            var conciliaciones = _context.Conciliaciones_Bancarias
                                 .Select(b => new SelectListItem
                                 {
                                     Value = b.Id_Conciliacion.ToString(),
                                     Text = b.Saldo_Contable.ToString()
                                 })
                                 .ToList();


            var pagos = _context.Pagos
                                .Select(b => new SelectListItem
                                {
                                    Value = b.Id_Pago.ToString(),
                                    Text = b.Tipo_Pago
                                })
                                .ToList();


            var bancos = _context.Bancos
                                .Select(b => new SelectListItem
                                {
                                    Value = b.Id_Banco.ToString(),
                                    Text = b.Nombre_Banco
                                })
                                .ToList();

            ViewBag.Diarios_Contables = new SelectList(diario, "Value", "Text", movimientos_bancarios.Id_Diario);
            ViewBag.Conciliaciones_Bancarias = new SelectList(conciliaciones, "Value", "Text", movimientos_bancarios.Id_Conciliacion);
            ViewBag.Pagos = new SelectList(pagos, "Value", "Text", movimientos_bancarios.Id_Pago);
            ViewBag.Bancos = new SelectList(bancos, "Value", "Text", movimientos_bancarios.Id_Banco);


            if (movimientos_bancarios == null)
            {
                return View(movimientos_bancarios);
            }

            return View(movimientos_bancarios);
           
        }

        // GET: MovimientosBancarios/Create
        public ActionResult Create()
        {

            ViewBag.Diarios_Contables = new SelectList(_context.Diarios_Contables, "Id_Diario", "Codigo_Diario");
            ViewBag.Conciliaciones_Bancarias = new SelectList(_context.Conciliaciones_Bancarias, "Id_Conciliacion", "Saldo_Contable");
            ViewBag.Pagos = new SelectList(_context.Pagos, "Id_Pago", "Tipo_Pago");
            ViewBag.Bancos = new SelectList(_context.Bancos, "Id_Banco", "Nombre_Banco");

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
                ViewBag.Movimientos_Bancarios = new SelectList(_context.Movimientos_Bancarios, "Id_Movimiento");
                return View(model);


            }
            catch
            {
                return View();
            }
        }

        // GET: MovimientosBancarios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {

            ViewBag.Diarios_Contables = new SelectList(_context.Diarios_Contables, "Id_Diario", "Codigo_Diario");
            ViewBag.Conciliaciones_Bancarias = new SelectList(_context.Conciliaciones_Bancarias, "Id_Conciliacion", "Saldo_Contable");
            ViewBag.Pagos = new SelectList(_context.Pagos, "Id_Pago", "Tipo_Pago");
            ViewBag.Bancos = new SelectList(_context.Bancos, "Id_Banco", "Nombre_Banco");

            var movimientos_bancarios = _context.Movimientos_Bancarios.Find(id);

            if (movimientos_bancarios == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            }

            // Obtener la lista de bancos desde la base de datos
            var diario = _context.Diarios_Contables
                                .Select(b => new SelectListItem
                                {
                                    Value = b.Id_Diario.ToString(),
                                    Text = b.Codigo_Diario
                                })
                                .ToList();

            var conciliaciones = _context.Conciliaciones_Bancarias
                                 .Select(b => new SelectListItem
                                 {
                                     Value = b.Id_Conciliacion.ToString(),
                                     Text = b.Saldo_Contable.ToString()
                                 })
                                 .ToList();


            var pagos = _context.Pagos
                                .Select(b => new SelectListItem
                                {
                                    Value = b.Id_Pago.ToString(),
                                    Text = b.Tipo_Pago
                                })
                                .ToList();


            var bancos = _context.Bancos
                                .Select(b => new SelectListItem
                                {
                                    Value = b.Id_Banco.ToString(),
                                    Text = b.Nombre_Banco
                                })
                                .ToList();

            Console.WriteLine(diario);
            Console.WriteLine(conciliaciones);
            Console.WriteLine(pagos);
            Console.WriteLine(bancos);

            // Pasar la lista de bancos a la vista usando ViewBag
            ViewBag.Diarios_Contables = new SelectList(diario, "Value", "Text", movimientos_bancarios.Id_Diario);
            ViewBag.Conciliaciones_Bancarias = new SelectList(conciliaciones, "Value", "Text", movimientos_bancarios.Id_Conciliacion);
            ViewBag.Pagos = new SelectList(pagos, "Value", "Text", movimientos_bancarios.Id_Pago);
            ViewBag.Bancos = new SelectList(bancos, "Value", "Text", movimientos_bancarios.Id_Banco);

            return View(movimientos_bancarios);
            
        }

        // POST: MovimientosBancarios/Edit/5
        [HttpPost]
        public ActionResult Edit(Movimientos_Bancarios movimientos_bancarios)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(movimientos_bancarios).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movimientos_bancarios);
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
