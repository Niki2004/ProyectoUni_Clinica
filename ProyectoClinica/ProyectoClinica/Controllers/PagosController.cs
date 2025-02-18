using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProyectoClinica.Controllers
{
    public class PagosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagosController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Pagos
        public ActionResult Index()
        {
            var listaRegistros = _context.Pagos.ToList();
            return View(listaRegistros);

        }

        // GET: Pagos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return View();

            var pago = _context.Pagos.Find(id);


            // Obtener la lista de bancos desde la base de datos
            var bancos = _context.Bancos
                                 .Select(b => new SelectListItem
                                 {
                                     Value = b.Id_Banco.ToString(),
                                     Text = b.Nombre_Banco
                                 })
                                 .ToList();

            ViewBag.Banco = new SelectList(bancos, "Value", "Text", pago.Id_Banco);




            if (pago == null)
            {
                return View(pago);
            }

            return View(pago);
        }

        // GET: Pagos/Create
        public ActionResult Create()
        {
            //ViewBag.Pagos = new SelectList(_context.Pagos, "Id_Pago");


            ViewBag.Banco = new SelectList(_context.Bancos, "Id_Banco", "Nombre_Banco");

            return View();
        }

        // POST: Pagos/Create  
        [HttpPost]
        public ActionResult Create(Pagos model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        //Guardar en la base de datos
                        _context.Pagos.Add(model);
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
                ViewBag.Pagos = new SelectList(_context.Pagos, "Id_Pago");
                return View(model);


            }
            catch
            {
                return View();
            }
        }

        // GET: Pagos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.Bancos = new SelectList(_context.Bancos, "Id_Banco", "Nombre_Banco");

            var pago = _context.Pagos.Find(id);
            
            if (pago == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            }

            // Obtener la lista de bancos desde la base de datos
            var bancos = _context.Bancos
                                 .Select(b => new SelectListItem
                                 {
                                     Value = b.Id_Banco.ToString(),
                                     Text = b.Nombre_Banco
                                 })
                                 .ToList();

            Console.WriteLine(bancos);

            // Pasar la lista de bancos a la vista usando ViewBag
            ViewBag.Banco = new SelectList(bancos, "Value", "Text", pago.Id_Banco);

            return View(pago);
        }

        // POST: Pagos/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Pagos pago)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(pago).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.Bancos = new SelectList(_context.Bancos, "Id_Banco", "Nombre_Banco");
            
            return View(pago);
        }

        // GET: Pagos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Pagos/Delete/5
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
