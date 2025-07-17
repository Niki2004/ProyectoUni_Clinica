using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ProyectoClinica.Controllers
{
    public class PagosDiariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagosDiariosController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize(Roles = "Administrador")]
        // GET: PagosDiarios
        public ActionResult Index()
        {
            var listaRegistros = _context.Pagos_Diarios.ToList();
            return View(listaRegistros);
        }

        [Authorize(Roles = "Administrador")]
        // GET: PagosDiarios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index"); 

            var pagos_diarios = await _context.Pagos_Diarios
                                              .FirstOrDefaultAsync(p => p.Id_Pago_Diario == id);

            if (pagos_diarios == null)
                return RedirectToAction("Index");


            // Obtener la lista de bancos desde la base de datos
            var contabilidad = _context.Contabilidad
                                 .Select(b => new SelectListItem
                                 {
                                     Value = b.Id_Contabilidad.ToString(),
                                     Text = b.ClienteProveedor
                                 })
                                 .ToList();

            var empleado = _context.Empleado
                                 .Select(b => new SelectListItem
                                 {
                                     Value = b.Id_Empleado.ToString(),
                                     Text = b.Nombre
                                 })
                                 .ToList();


            ViewBag.Contabilidad = new SelectList(contabilidad, "Value", "Text", pagos_diarios?.Id_Contabilidad);
            ViewBag.Empleado = new SelectList(empleado, "Value", "Text", pagos_diarios?.Id_Empleado);


            return View(pagos_diarios);
        }

        [Authorize(Roles = "Administrador")]
        // GET: PagosDiarios/Create
        public ActionResult Create()
        {


            ViewBag.Contabilidad = new SelectList(_context.Contabilidad, "Id_Contabilidad", "ClienteProveedor");
            ViewBag.Empleado = new SelectList(_context.Empleado, "Id_Empleado", "Nombre");


            return View();
        }

        // POST: PagosDiarios/Create
        [HttpPost]
        public ActionResult Create(Pagos_Diarios model)
        {
            // Validación de fechas
            DateTime hoy = DateTime.Today;
            var fechas = new[] { model.Fecha_Pago, model.Fecha_Registro };
            foreach (var fecha in fechas)
            {
                if (fecha < hoy)
                {
                    ModelState.AddModelError("", "No se permiten fechas pasadas.");
                    ViewBag.Contabilidad = new SelectList(_context.Contabilidad, "Id_Contabilidad", "ClienteProveedor", model.Id_Contabilidad);
                    ViewBag.Empleado = new SelectList(_context.Empleado, "Id_Empleado", "Nombre", model.Id_Empleado);
                    return View(model);
                }
                if (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday)
                {
                    ModelState.AddModelError("", "No se permiten fechas en sábado ni domingo.");
                    ViewBag.Contabilidad = new SelectList(_context.Contabilidad, "Id_Contabilidad", "ClienteProveedor", model.Id_Contabilidad);
                    ViewBag.Empleado = new SelectList(_context.Empleado, "Id_Empleado", "Nombre", model.Id_Empleado);
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
                        _context.Pagos_Diarios.Add(model);
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
                ViewBag.Pagos_Diarios = new SelectList(_context.Pagos_Diarios, "Id_Pago_Diario");
                return View(model);


            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Administrador")]
        // GET: PagosDiarios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.Contabilidad = new SelectList(_context.Contabilidad, "Id_Contabilidad", "ClienteProveedor");
            ViewBag.Empleado = new SelectList(_context.Empleado, "Id_Empleado", "Nombre");

            var pago = _context.Pagos_Diarios.Find(id);

            if (pago == null)
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

            var empleado = _context.Empleado
                                 .Select(b => new SelectListItem
                                 {
                                     Value = b.Id_Empleado.ToString(),
                                     Text = b.Nombre
                                 })
                                 .ToList();

            Console.WriteLine(contabilidad);
            Console.WriteLine(empleado);

            // Pasar la lista de bancos a la vista usando ViewBag
            ViewBag.Contabilidad = new SelectList(contabilidad, "Value", "Text", pago.Id_Contabilidad);
            ViewBag.Empleado = new SelectList(empleado, "Value", "Text", pago.Id_Empleado);

            return View(pago);
        }

        // POST: PagosDiarios/Edit/5
        [HttpPost]
        public ActionResult Edit(Pagos_Diarios pago)
        {
            // Validación de fechas
            DateTime hoy = DateTime.Today;
            var fechas = new[] { pago.Fecha_Pago, pago.Fecha_Registro };
            foreach (var fecha in fechas)
            {
                if (fecha < hoy)
                {
                    ModelState.AddModelError("", "No se permiten fechas pasadas.");
                    ViewBag.Contabilidad = new SelectList(_context.Contabilidad, "Id_Contabilidad", "ClienteProveedor", pago.Id_Contabilidad);
                    ViewBag.Empleado = new SelectList(_context.Empleado, "Id_Empleado", "Nombre", pago.Id_Empleado);
                    return View(pago);
                }
                if (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday)
                {
                    ModelState.AddModelError("", "No se permiten fechas en sábado ni domingo.");
                    ViewBag.Contabilidad = new SelectList(_context.Contabilidad, "Id_Contabilidad", "ClienteProveedor", pago.Id_Contabilidad);
                    ViewBag.Empleado = new SelectList(_context.Empleado, "Id_Empleado", "Nombre", pago.Id_Empleado);
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
