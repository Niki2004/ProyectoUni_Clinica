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
    public class InventarioEncabezadoContaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventarioEncabezadoContaController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Departamentos
        public ActionResult Index()
        {

            var listaRegistros = _context.Inventario_Encabezado_Conta.ToList();

            return View(listaRegistros);  

        }

        // GET: InventarioContaEncabezado/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return View();

            var inventario = _context.Inventario_Encabezado_Conta.Find(id);

            if (inventario == null)
            {
                return View(inventario);
            }

            return View(inventario);
        }

        // GET: InventarioContaEncabezado/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InventarioContaEncabezado/Create
        [HttpPost]
        public ActionResult Create(Inventario_Encabezado_Conta model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        //Guardar en la base de datos
                        _context.Inventario_Encabezado_Conta.Add(model);
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
                ViewBag.Inventario_Encabezado_Conta = new SelectList(_context.Inventario_Encabezado_Conta, "Id_Inventario_Encabezado");
                return View(model);
            }
            catch
            {
                return View();
            }
        }

        // GET: InventarioContaEncabezado/Edit/5
        public ActionResult Edit(int? id)
        {
            var inventario = _context.Inventario_Encabezado_Conta.Find(id);

            if (inventario == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            }

            return View(inventario);
        }

        // POST: InventarioContaEncabezado/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Inventario_Encabezado_Conta inventario)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(inventario).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inventario);
        }

        // GET: InventarioContaEncabezado/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InventarioContaEncabezado/Delete/5
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
