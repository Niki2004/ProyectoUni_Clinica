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
    public class DepartamentosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartamentosController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize(Roles = "Administrador")]
        // GET: Departamentos
        public ActionResult Index()
        {
            var listaRegistros = _context.Departamentos.ToList();
            return View(listaRegistros);
        }

        [Authorize(Roles = "Administrador")]
        // GET: Departamentos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return View();

            var departamentos = _context.Departamentos.Find(id);

            if (departamentos == null)
            {
                return View(departamentos);
            }

            return View(departamentos);
        }

        [Authorize(Roles = "Administrador")]
        // GET: Departamentos/Create
        public ActionResult Create()
        {
            ViewBag.Inventario_Detalle_Conta = new SelectList(_context.Inventario_Detalle_Conta, "Id_Inventario_Detalle", "Cantidad_Stock");
            return View();
        }

        // POST: Departamentos/Create
        [HttpPost]
        public ActionResult Create(Departamentos model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        //Guardar en la base de datos
                        _context.Departamentos.Add(model);
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
                ViewBag.Departamentos = new SelectList(_context.Departamentos, "Id_");
                return View(model);
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Administrador")]
        // GET: Departamentos/Edit/5
        public ActionResult Edit(int id)
        {
            var departamentos = _context.Departamentos.Find(id);

            if (departamentos == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            }

            return View(departamentos);
        }

        // POST: Departamentos/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Departamentos departamentos)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(departamentos).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(departamentos);
        }

    }
}
