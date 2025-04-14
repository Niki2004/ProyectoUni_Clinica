using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
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

        [Authorize(Roles = "Administrador")]
        // GET: Servicios
        public ActionResult Index()
        {
            var listaRegistros = _context.Servicio.ToList();
            return View(listaRegistros);
        }

        [Authorize(Roles = "Administrador")]
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

        [Authorize(Roles = "Administrador")]
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

        [Authorize(Roles = "Administrador")]
        // GET: Servicios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var servicios = await _context.Servicio.FindAsync(id);

            if (servicios == null)
                return HttpNotFound();

            ViewBag.Servicio = new SelectList(_context.Servicio, "Id_Servicio");
            return View(servicios);
        }

        // POST: Servicios/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(servicio).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al guardar los cambios: " + ex.Message);
                }
            }

            ViewBag.Servicio = new SelectList(_context.Servicio, "Id_Servicio");
            return View(servicio);

        }
        
    }
}
