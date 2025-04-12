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
    public class ProductosContaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductosContaController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize(Roles = "Administrador")]
        // GET: ProductosConta
        public ActionResult Index()
        {
            var listaRegistros = _context.Productos_Conta.ToList();
            return View(listaRegistros);
        }

        [Authorize(Roles = "Administrador")]
        // GET: ProductosConta/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return View();

            var productos = _context.Productos_Conta.Find(id);

            if (productos == null)
            {
                return View(productos);
            }

            return View(productos);
        }

        [Authorize(Roles = "Administrador")]
        // GET: ProductosConta/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: ProductosConta/Create
        [HttpPost]
        public ActionResult Create(Productos_Conta model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        //Guardar en la base de datos
                        _context.Productos_Conta.Add(model);
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
                ViewBag.Productos_Conta = new SelectList(_context.Productos_Conta, "Id_Producto");
                return View(model);
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Administrador")]
        // GET: ProductosConta/Edit/5
        public ActionResult Edit(int id)
        {
            var productos = _context.Productos_Conta.Find(id);

            if (productos == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            }

            return View(productos);
        }

        // POST: ProductosConta/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Productos_Conta productos)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(productos).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productos);
        }

        // GET: ProductosConta/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

    }
}
