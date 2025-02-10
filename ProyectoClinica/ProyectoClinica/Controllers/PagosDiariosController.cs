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
        // GET: PagosDiarios
        public ActionResult Index()
        {
            var listaRegistros = _context.Pagos_Diarios.ToList();
            return View(listaRegistros);
        }

        // GET: PagosDiarios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return View();

            var pagos_diarios = _context.Pagos_Diarios.Find(id);


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


            ViewBag.Contabilidad = new SelectList(contabilidad, "Value", "Text", pagos_diarios.Id_Contabilidad);
            ViewBag.Empleado = new SelectList(empleado, "Value", "Text", pagos_diarios.Id_Empleado);




            if (pagos_diarios == null)
            {
                return View(pagos_diarios);
            }

            return View(pagos_diarios);
        }

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
            if (ModelState.IsValid)
            {
                _context.Entry(pago).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pago);
        }

        // GET: PagosDiarios/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PagosDiarios/Delete/5
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
