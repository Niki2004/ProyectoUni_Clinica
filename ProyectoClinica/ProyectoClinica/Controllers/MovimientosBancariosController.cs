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

        [Authorize(Roles = "Administrador,Auditor")]
        // GET: MovimientosBancarios
        public ActionResult Index()
        {
            var listaRegistros = _context.Movimientos_Bancarios.ToList();
            return View(listaRegistros);
        }


        [Authorize(Roles = "Administrador,Auditor")]
        public ActionResult AjustesBancarios(int id)
        {
            var movimiento = _context.Movimientos_Bancarios.Find(id);
            if (movimiento == null)
            {
                return HttpNotFound();
            }

            // Calcular la diferencia entre ingresos y egresos
            decimal diferencia = movimiento.Ingresos - movimiento.Egresos;

            if (diferencia != 0)
            {
                // Crear un nuevo registro para ajustar el saldo
                var ajuste = new Movimientos_Bancarios
                {
                    Id_Diario = movimiento.Id_Diario,
                    Id_Conciliacion = movimiento.Id_Conciliacion,
                    Id_Pago = movimiento.Id_Pago,
                    Id_Banco = movimiento.Id_Banco,
                    Fecha_Movimiento = DateTime.Now,
                    Descripcion = "Ajuste automático de conciliación"
                };

                if (diferencia > 0)
                {
                    // Faltan egresos, registrar egreso
                    ajuste.Ingresos = 0;
                    ajuste.Egresos = (int)diferencia;
                }
                else
                {
                    // Faltan ingresos, registrar ingreso
                    ajuste.Ingresos = (int)Math.Abs(diferencia);
                    ajuste.Egresos = 0;
                }

                // Ajustar el saldo del registro original a 0
                movimiento.Saldo = 0;

                // Agregar el ajuste y guardar los cambios
                _context.Movimientos_Bancarios.Add(ajuste);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Administrador,Auditor")]
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

        [Authorize(Roles = "Administrador,Auditor")]
        // GET: MovimientosBancarios/Create
        public ActionResult Create()
        {

            ViewBag.Diarios_Contables = new SelectList(_context.Diarios_Contables, "Id_Diario", "Codigo_Diario");
            ViewBag.Conciliaciones_Bancarias = new SelectList(_context.Conciliaciones_Bancarias, "Id_Conciliacion", "Saldo_Contable");
            ViewBag.Pagos = new SelectList(_context.Pagos, "Id_Pago", "Tipo_Pago");
            ViewBag.Bancos = new SelectList(_context.Bancos, "Id_Banco", "Nombre_Banco");

            
            var totalIngresos = _context.Movimientos_Bancarios.Sum(m => (decimal?)m.Ingresos) ?? 0;
            var totalEgresos = _context.Movimientos_Bancarios.Sum(m => (decimal?)m.Egresos) ?? 0;


            return View();
        }

        // POST: MovimientosBancarios/Create
        [HttpPost]
        public ActionResult Create(Movimientos_Bancarios model)
        {
            // Validación de fecha de movimiento: no permitir sábados ni domingos
            if (model.Fecha_Movimiento.DayOfWeek == DayOfWeek.Saturday || model.Fecha_Movimiento.DayOfWeek == DayOfWeek.Sunday)
            {
                ModelState.AddModelError("Fecha_Movimiento", "No se permiten fechas en sábado ni domingo.");
                ViewBag.Diarios_Contables = new SelectList(_context.Diarios_Contables, "Id_Diario", "Codigo_Diario");
                ViewBag.Conciliaciones_Bancarias = new SelectList(_context.Conciliaciones_Bancarias, "Id_Conciliacion", "Saldo_Contable");
                ViewBag.Pagos = new SelectList(_context.Pagos, "Id_Pago", "Tipo_Pago");
                ViewBag.Bancos = new SelectList(_context.Bancos, "Id_Banco", "Nombre_Banco");
                return View(model);
            }
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Guardar en la base de datos
                        _context.Movimientos_Bancarios.Add(model);
                        _context.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        // Capturar el error y mostrarlo al usuario
                        ModelState.AddModelError("", "Ocurrió un error al guardar los datos: " + ex.Message);
                    }
                }

                // Si el modelo no es válido o hubo un error, repetir el proceso y pasar la vista con el modelo
                ViewBag.Movimientos_Bancarios = new SelectList(_context.Movimientos_Bancarios, "Id_Movimiento");
                return View(model);
            }
            catch
            {
                return View();
            }
        }


        [Authorize(Roles = "Administrador,Auditor")]
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
            // Validación de fecha de movimiento: no permitir sábados ni domingos
            if (movimientos_bancarios.Fecha_Movimiento.DayOfWeek == DayOfWeek.Saturday || movimientos_bancarios.Fecha_Movimiento.DayOfWeek == DayOfWeek.Sunday)
            {
                ModelState.AddModelError("Fecha_Movimiento", "No se permiten fechas en sábado ni domingo.");
                ViewBag.Diarios_Contables = new SelectList(_context.Diarios_Contables, "Id_Diario", "Codigo_Diario");
                ViewBag.Conciliaciones_Bancarias = new SelectList(_context.Conciliaciones_Bancarias, "Id_Conciliacion", "Saldo_Contable");
                ViewBag.Pagos = new SelectList(_context.Pagos, "Id_Pago", "Tipo_Pago");
                ViewBag.Bancos = new SelectList(_context.Bancos, "Id_Banco", "Nombre_Banco");
                return View(movimientos_bancarios);
            }
            if (ModelState.IsValid)
            {
                _context.Entry(movimientos_bancarios).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movimientos_bancarios);
        }
    }
}
