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
        // GET: PagosXNomina
        public ActionResult Index()
        {

            var listaRegistros = _context.PagosXNomina.ToList();
            return View(listaRegistros);
        }

        public ActionResult IndexProveedor(int id)
        {
            if (id == null)
                return View();
            
            var listaRegistros = _context.PagosXNomina
                                         .Where(p => p.Id_Contabilidad == id)
                                         .ToList();

            return View(listaRegistros);
        }

        // GET: PagosXNomina/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return View();

            var pagosxnomina = _context.PagosXNomina.Find(id);


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


            ViewBag.Contabilidad = new SelectList(contabilidad, "Value", "Text", pagosxnomina.Id_Contabilidad);
            ViewBag.Pagos = new SelectList(pagos, "Value", "Text", pagosxnomina.Id_Pago);
            ViewBag.Empleado = new SelectList(empleado, "Value", "Text", pagosxnomina.Id_Empleado);




            if (pagosxnomina == null)
            {
                return View(pagosxnomina);
            }

            return View(pagosxnomina);
        }

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
            if (ModelState.IsValid)
            {
                _context.Entry(pago).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pago);
        }

        // GET: PagosXNomina/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PagosXNomina/Delete/5
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
