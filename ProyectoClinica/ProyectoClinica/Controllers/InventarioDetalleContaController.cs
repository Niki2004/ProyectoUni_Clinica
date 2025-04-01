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
    public class InventarioDetalleContaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventarioDetalleContaController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: ProductosConta
        public ActionResult Index()
        {
            var listaRegistros = _context.Inventario_Detalle_Conta.ToList();
            return View(listaRegistros);
        }

        // GET: InventarioDetalleConta/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return View();

            var detalles = _context.Inventario_Detalle_Conta.Find(id);

            if (detalles == null)
            {
                return View(detalles);
            }

            return View(detalles);
        }

        // GET: InventarioDetalleConta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InventarioDetalleConta/Create
        [HttpPost]
        public ActionResult Create(Inventario_Detalle_Conta model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        //Guardar en la base de datos
                        _context.Inventario_Detalle_Conta.Add(model);
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
                ViewBag.Inventario_Detalle_Conta = new SelectList(_context.Inventario_Detalle_Conta, "Id_Inventario_Detalle");
                return View(model);
            }
            catch
            {
                return View();
            }
        }

        // GET: InventarioDetalleConta/Edit/5
        public ActionResult Edit(int id)
        {
            var detalles = _context.Inventario_Detalle_Conta.Find(id);

            if (detalles == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            }

            // Cargar las listas para los dropdowns
            ViewBag.Encabezados = new SelectList(_context.Inventario_Encabezado_Conta, "Id_Inventario_Encabezado", "Id_Inventario_Encabezado");
            ViewBag.Departamentos = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento");
            ViewBag.Productos = new SelectList(_context.Productos_Conta, "Id_Producto", "Nombre_producto");

            return View(detalles);
        }

        // POST: InventarioDetalleConta/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Inventario_Detalle_Conta detalles)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingDetalles = await _context.Inventario_Detalle_Conta.FindAsync(detalles.Id_Inventario_Detalle);
                    if (existingDetalles == null)
                    {
                        return HttpNotFound();
                    }

                    // Marcar la entidad como modificada
                    _context.Entry(existingDetalles).State = EntityState.Modified;

                    // Actualizar las propiedades del objeto existente
                    existingDetalles.Id_Inventario_Encabezado = detalles.Id_Inventario_Encabezado;
                    existingDetalles.Id_Departamento = detalles.Id_Departamento;
                    existingDetalles.Id_Producto = detalles.Id_Producto;
                    existingDetalles.Fecha_Entrada = detalles.Fecha_Entrada;
                    existingDetalles.Fecha_Salida = detalles.Fecha_Salida;
                    existingDetalles.Cantidad_Stock = detalles.Cantidad_Stock;
                    existingDetalles.Cantidad_Salida = detalles.Cantidad_Salida;
                    existingDetalles.Precio = detalles.Precio;

                    try
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                    catch (System.Data.Entity.Core.OptimisticConcurrencyException)
                    {
                        // Si hay un conflicto de concurrencia, recargar los datos y mostrar un mensaje
                        ModelState.AddModelError("", "El registro ha sido modificado por otro usuario. Por favor, actualice la página y vuelva a intentarlo.");
                        return View(detalles);
                    }
                }
                return View(detalles);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al guardar los cambios: " + ex.Message);
                return View(detalles);
            }
        }

        // GET: InventarioDetalleConta/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InventarioDetalleConta/Delete/5
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
