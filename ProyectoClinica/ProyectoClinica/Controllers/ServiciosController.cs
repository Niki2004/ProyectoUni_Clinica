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
    public class ServiciosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiciosController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Servicios
        public ActionResult Index()
        {
            var listaRegistros = _context.Servicio.ToList();
            return View(listaRegistros);
        }

        // GET: Servicios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var servicio = await _context.Servicio.FirstOrDefaultAsync(m => m.Id_Servicio == id);

            if (servicio == null)
            {
                return View(servicio);
            }

            return View(servicio);
        }

        // GET: Servicios/Create
        public ActionResult Create()
        {
            ViewBag.Servicio = new SelectList(_context.Servicio, "Id_Servicio");
            return View();
        }

        // POST: Servicios/Create
        [HttpPost]
        public ActionResult Create(Servicio model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    int idSeleccionado = model.Id_Servicio; // Aquí obtienes el ID del dropdown

                    try
                    {
                        // Guardar en la base de datos
                        _context.Servicio.Add(model);
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
                ViewBag.Servicio = new SelectList(_context.Servicio, "Id_Servicio");
                return View(model);


            }
            catch
            {
                return View();
            }
        }

        // GET: Servicios/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Servicios/Edit/5
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

        // GET: Servicios/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Servicios/Delete/5
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
