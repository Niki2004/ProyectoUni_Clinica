using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace ProyectoClinica.Controllers
{
    public class CajaChicaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CajaChicaController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize(Roles = "Administrador,Contador")]
        // GET: CajaChica
        public ActionResult Index()
        {
            var listaRegistros = _context.Caja_Chica.ToList();
            return View(listaRegistros);
        }

        [Authorize(Roles = "Administrador,Contador")]
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

            ViewBag.Contabilidad = new SelectList(_context.Contabilidad, "Id_Contabilidad", "ClienteProveedor");

            return View(caja);
        }

        [Authorize(Roles = "Administrador,Contador")]
        // GET: CajaChica/Create
        public ActionResult Create()
        {
            ViewBag.Servicio = new SelectList(_context.Caja_Chica, "Id_Caja_Chica");
            ViewBag.Contabilidad = new SelectList(_context.Contabilidad, "Id_Contabilidad", "ClienteProveedor");
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

        [Authorize(Roles = "Administrador,Contador")]
        // GET: CajaChica/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var caja = _context.Caja_Chica.Find(id);
            if (caja == null)
                return HttpNotFound();
            ViewBag.Caja_Chica = new SelectList(_context.Caja_Chica, "Id_Caja_Chica");
            ViewBag.Contabilidad = new SelectList(_context.Contabilidad, "Id_Contabilidad", "ClienteProveedor");
            return View(caja);
        }

        // POST: CajaChica/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Caja_Chica caja)
        {

            if (ModelState.IsValid)
            {
                _context.Entry(caja).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Caja_Chica = new SelectList(_context.Caja_Chica, "Id_Caja_Chica");
            return View(caja);
        }

    }
}
