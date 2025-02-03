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
    public class CajaChicaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CajaChicaController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: CajaChica
        public ActionResult Index()
        {
            var listaRegistros = _context.Caja_Chica.ToList();
            return View(listaRegistros);
        }

        // GET: CajaChica/Details/5
        public async Task<ActionResult> Details(int? id)
        {

            if (id == null)
            {
                return View();
            }

            var caja = await _context.Caja_Chica.FirstOrDefaultAsync(m => m.Id_Caja_Chica == id);

            if (caja == null)
            {
                return View(caja);
            }

            return View(caja);
        }

        // GET: CajaChica/Create
        public ActionResult Create()
        {
            ViewBag.Servicio = new SelectList(_context.Caja_Chica, "Id_Caja_Chica");
            return View();
        }

        // POST: CajaChica/Create
        [HttpPost]
        public ActionResult Create(Caja_Chica model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    int idSeleccionado = model.Id_Caja_Chica; // Aquí obtienes el ID del dropdown

                    try
                    {
                        // Guardar en la base de datos
                        _context.Caja_Chica.Add(model);
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
                ViewBag.Caja_Chica = new SelectList(_context.Caja_Chica, "Id_Caja_Chica");
                return View(model);


            }
            catch
            {
                return View();
            }
        }

        // GET: CajaChica/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CajaChica/Edit/5
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

        // GET: CajaChica/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CajaChica/Delete/5
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
