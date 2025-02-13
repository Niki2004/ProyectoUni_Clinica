using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProyectoClinica.Controllers
{
    public class ProductosContaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductosContaController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: ProductosConta
        public ActionResult Index()
        {
            var listaRegistros = _context.Productos_Conta.ToList();
            return View(listaRegistros);
        }

        // GET: ProductosConta/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductosConta/Create
        public ActionResult Create()
        {
            ViewBag.Facturacion_Productos_Conta = new SelectList(_context.Facturacion_Productos_Conta, "Id_Factura_Producto", "Cantidad_Vendida");
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


        // GET: ProductosConta/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductosConta/Edit/5
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

        // GET: ProductosConta/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductosConta/Delete/5
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
