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
    public class PagosXNominaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagosXNominaController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize(Roles = "Administrador")]
        // GET: PagosXNomina
        public ActionResult Index()
        {

            var listaRegistros = _context.PagosXNomina.ToList();
            return View(listaRegistros);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult IndexProveedor(int id)
        {
            if (id == null)
                return View();
            
            var listaRegistros = _context.PagosXNomina
                                         .Where(p => p.Id_Contabilidad == id)
                                         .ToList();

            return View(listaRegistros);
        }

        [Authorize(Roles = "Administrador")]
        // GET: PagosXNomina/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound(); // Mejor indicar que el ID es inválido
            }

            var pagosxnomina = await _context.PagosXNomina
                                    .FirstOrDefaultAsync(p => p.Id_PagosXNomina == id); // Asegura que traes el dato correcto

            if (pagosxnomina == null)
            {
                return HttpNotFound(); // Si el pago no existe, muestra un error 404
            }

            // Obtener la lista de bancos desde la base de datos
            var contabilidad = await _context.Contabilidad
                                   .Select(b => new SelectListItem
                                   {
                                       Value = b.Id_Contabilidad.ToString(),
                                       Text = b.ClienteProveedor
                                   })
                                   .ToListAsync();

            var pagos = await _context.Pagos
                                   .Select(b => new SelectListItem
                                   {
                                       Value = b.Id_Pago.ToString(),
                                       Text = b.Numero_Referencia
                                   })
                                   .ToListAsync();

            var empleado = await _context.Empleado
                                   .Select(b => new SelectListItem
                                   {
                                       Value = b.Id_Empleado.ToString(),
                                       Text = b.Nombre
                                   })
                                   .ToListAsync();

            // Asignar datos a ViewBag (siempre que haya un pago existente)
            ViewBag.Contabilidad = new SelectList(contabilidad, "Value", "Text", pagosxnomina.Id_Contabilidad);
            ViewBag.Pagos = new SelectList(pagos, "Value", "Text", pagosxnomina.Id_Pago);
            ViewBag.Empleado = new SelectList(empleado, "Value", "Text", pagosxnomina.Id_Empleado);

            return View(pagosxnomina); // Retorna la vista con el modelo cargado
        }

        [Authorize(Roles = "Administrador")]
        // GET: PagosXNomina/Create
        public ActionResult Create()
        {
            ViewBag.Contabilidad = new SelectList(_context.Contabilidad, "Id_Contabilidad", "ClienteProveedor");
            ViewBag.Pagos = new SelectList(_context.Pagos, "Id_Pago", "Numero_Referencia");
            ViewBag.Empleado = new SelectList(_context.Empleado, "Id_Empleado", "Nombre");
            return View();
        }

        // POST: PagosXNomina/Create
        [HttpPost]
        public ActionResult Create(PagosXNomina model)
        {
            // Validación de fechas
            DateTime hoy = DateTime.Today;
            var fechas = new[] { model.Fecha_Pago, model.Fecha_Registro };
            foreach (var fecha in fechas)
            {
                if (fecha < hoy)
                {
                    ModelState.AddModelError("", "No se permiten fechas pasadas.");
                    ViewBag.Contabilidad = new SelectList(_context.Contabilidad, "Id_Contabilidad", "ClienteProveedor");
                    ViewBag.Pagos = new SelectList(_context.Pagos, "Id_Pago", "Numero_Referencia");
                    ViewBag.Empleado = new SelectList(_context.Empleado, "Id_Empleado", "Nombre");
                    return View(model);
                }
                if (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday)
                {
                    ModelState.AddModelError("", "No se permiten fechas en sábado ni domingo.");
                    ViewBag.Contabilidad = new SelectList(_context.Contabilidad, "Id_Contabilidad", "ClienteProveedor");
                    ViewBag.Pagos = new SelectList(_context.Pagos, "Id_Pago", "Numero_Referencia");
                    ViewBag.Empleado = new SelectList(_context.Empleado, "Id_Empleado", "Nombre");
                    return View(model);
                }
            }

            try
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        //Guardar en la base de datos
                        _context.PagosXNomina.Add(model);
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
                ViewBag.PagosXNomina = new SelectList(_context.PagosXNomina, "Id_PagosXNomina");
                return View(model);


            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Administrador")]
        // GET: PagosXNomina/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.Contabilidad = new SelectList(_context.Contabilidad, "Id_Contabilidad", "ClienteProveedor");
            ViewBag.Pagos = new SelectList(_context.Pagos, "Id_Pago", "Numero_Referencia");
            ViewBag.Empleado = new SelectList(_context.Empleado, "Id_Empleado", "Nombre");

            var pagosxnomina = _context.PagosXNomina.Find(id);

            if (pagosxnomina == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            }

            // Obtener la lista de bancos desde la base de datos
            var contabilidad = _context.Contabilidad
                                .Select(b => new SelectListItem
                                {
                                    Value = b.Id_Contabilidad.ToString(),
                                    Text = b.ClienteProveedor
                                })
                                .ToList();

            var pagos = _context.Pagos
                                 .Select(b => new SelectListItem
                                 {
                                     Value = b.Id_Pago.ToString(),
                                     Text = b.Numero_Referencia
                                 })
                                 .ToList();

            var empleado = _context.Empleado
                                 .Select(b => new SelectListItem
                                 {
                                     Value = b.Id_Empleado.ToString(),
                                     Text = b.Nombre
                                 })
                                 .ToList();

            Console.WriteLine(contabilidad);
            Console.WriteLine(pagos);
            Console.WriteLine(empleado);

            // Pasar la lista de bancos a la vista usando ViewBag
            ViewBag.Contabilidad = new SelectList(contabilidad, "Value", "Text", pagosxnomina.Id_Contabilidad);
            ViewBag.Pagos = new SelectList(pagos, "Value", "Text", pagosxnomina.Id_Pago);
            ViewBag.Empleado = new SelectList(empleado, "Value", "Text", pagosxnomina.Id_Empleado);

            return View(pagosxnomina);
        }

        // POST: PagosXNomina/Edit/5
        [HttpPost]
        public ActionResult Edit(PagosXNomina pago)
        {
            // Validación de fechas
            DateTime hoy = DateTime.Today;
            var fechas = new[] { pago.Fecha_Pago, pago.Fecha_Registro };
            foreach (var fecha in fechas)
            {
                if (fecha < hoy)
                {
                    ModelState.AddModelError("", "No se permiten fechas pasadas.");
                    ViewBag.Contabilidad = new SelectList(_context.Contabilidad, "Id_Contabilidad", "ClienteProveedor");
                    ViewBag.Pagos = new SelectList(_context.Pagos, "Id_Pago", "Numero_Referencia");
                    ViewBag.Empleado = new SelectList(_context.Empleado, "Id_Empleado", "Nombre");
                    return View(pago);
                }
                if (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday)
                {
                    ModelState.AddModelError("", "No se permiten fechas en sábado ni domingo.");
                    ViewBag.Contabilidad = new SelectList(_context.Contabilidad, "Id_Contabilidad", "ClienteProveedor");
                    ViewBag.Pagos = new SelectList(_context.Pagos, "Id_Pago", "Numero_Referencia");
                    ViewBag.Empleado = new SelectList(_context.Empleado, "Id_Empleado", "Nombre");
                    return View(pago);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Entry(pago).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pago);
        }

    }
}
