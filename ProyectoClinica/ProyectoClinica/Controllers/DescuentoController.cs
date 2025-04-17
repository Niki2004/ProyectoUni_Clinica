using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProyectoClinica.Controllers
{
    public class DescuentoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DescuentoController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize(Roles = "Contador,Secretaria,Administrador")]
        // GET: Descuento
        public ActionResult Index()
        {
            var listaRegistros = _context.Descuento.ToList();
            return View(listaRegistros);
        }

        [Authorize(Roles = "Contador,Secretaria,Administrador")]
        // GET: Descuento/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var descuento = await _context.Descuento.FirstOrDefaultAsync(m => m.Id_Descuento == id);

            if (descuento == null)
            {
                return View(descuento);
            }

            return View(descuento);
        }

        [Authorize(Roles = "Contador,Secretaria,Administrador")]
        // GET: Descuento/Create
        public ActionResult Create()
        {
            ViewBag.Descuento = new SelectList(_context.Descuento, "Id_Descuento");
            return View();
        }

        [Authorize(Roles = "Contador,Secretaria,Administrador")]
        // POST: Descuento/Create
        [HttpPost]
        public ActionResult Create(Descuento model)
        {

            try
            {

                if (ModelState.IsValid)
                {
                    int idSeleccionado = model.Id_Descuento; // Aquí obtienes el ID del dropdown

                    try
                    {
                        // Guardar en la base de datos
                        _context.Descuento.Add(model);
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
                ViewBag.Descuento = new SelectList(_context.Descuento, "Id_Descuento");
                return View(model);


            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Contador,Secretaria,Administrador")]
        // GET: Descuento/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var descuento = _context.Descuento.Find(id);
            if (descuento == null)
                return HttpNotFound();
            ViewBag.Descuento = new SelectList(_context.Descuento, "Id_Descuento");
            return View(descuento);
        }

        [Authorize(Roles = "Contador,Secretaria,Administrador")]
        // POST: Descuento/Edit/5
        [HttpPost]
        public async Task<ActionResult>Edit(Descuento descuento)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(descuento).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Descuento = new SelectList(_context.Descuento, "Id_Descuento");
            return View(descuento);
        }

        [Authorize(Roles = "Contador,Secretaria,Administrador")]
        // GET: Descuento/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        [Authorize(Roles = "Contador,Secretaria,Administrador")]
        // POST: Descuento/Delete/5
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
