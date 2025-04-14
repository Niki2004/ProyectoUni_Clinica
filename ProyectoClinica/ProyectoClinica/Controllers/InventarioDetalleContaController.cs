using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace ProyectoClinica.Controllers
{
    public class InventarioDetalleContaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventarioDetalleContaController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize(Roles = "Administrador,Contador")]
        // GET: ProductosConta
        public ActionResult Index()
        {
            var listaRegistros = _context.Inventario_Detalle_Conta.ToList();
            return View(listaRegistros);
        }

        [Authorize(Roles = "Administrador,Contador")]
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


        [Authorize(Roles = "Administrador,Contador")]
        // GET: InventarioDetalleConta/Create
        public ActionResult Create()
        {
            ViewBag.Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento");
            ViewBag.Producto = new SelectList(_context.Productos_Conta, "Id_Producto", "Nombre_producto");
            return View();
        }

        // POST: InventarioDetalleConta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Inventario_Detalle,Id_Inventario_Encabezado,Id_Departamento,Id_Producto,Fecha_Entrada,Fecha_Salida,Cantidad_Stock,Cantidad_Salida,Precio")] Inventario_Detalle_Conta inventario_Detalle_Conta)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si el producto existe
                    var producto = _context.Productos_Conta.Find(inventario_Detalle_Conta.Id_Producto);
                    if (producto == null)
                    {
                        ModelState.AddModelError("Id_Producto", "El producto seleccionado no existe.");
                        ViewBag.Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento");
                        ViewBag.Producto = new SelectList(_context.Productos_Conta, "Id_Producto", "Nombre_producto");
                        return View(inventario_Detalle_Conta);
                    }

                    // Verificar si el departamento existe
                    var departamento = _context.Departamentos.Find(inventario_Detalle_Conta.Id_Departamento);
                    if (departamento == null)
                    {
                        ModelState.AddModelError("Id_Departamento", "El departamento seleccionado no existe.");
                        ViewBag.Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento");
                        ViewBag.Producto = new SelectList(_context.Productos_Conta, "Id_Producto", "Nombre_producto");
                        return View(inventario_Detalle_Conta);
                    }

                    _context.Inventario_Detalle_Conta.Add(inventario_Detalle_Conta);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Inventario creado exitosamente.";
                    return RedirectToAction("Index", "InventarioDetalleConta");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al guardar el registro: " + ex.Message);
                }
            }

            // Si el modelo no es válido, recargar los ViewBag
            ViewBag.Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento");
            ViewBag.Producto = new SelectList(_context.Productos_Conta, "Id_Producto", "Nombre_producto");
            return View(inventario_Detalle_Conta);
        }


        [Authorize(Roles = "Administrador,Contador")]
        // GET: InventarioDetalleConta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Obtener el registro específico con sus relaciones
            var inventario_Detalle_Conta = _context.Inventario_Detalle_Conta
                .Include(i => i.Departamento)
                .Include(i => i.Producto)
                .FirstOrDefault(i => i.Id_Inventario_Detalle == id);

            if (inventario_Detalle_Conta == null)
            {
                return HttpNotFound();
            }

            // Solo inicializar los dropdowns necesarios
            ViewBag.Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento", inventario_Detalle_Conta.Id_Departamento);
            ViewBag.Producto = new SelectList(_context.Productos_Conta, "Id_Producto", "Nombre_producto", inventario_Detalle_Conta.Id_Producto);

            return View(inventario_Detalle_Conta);
        }

        // POST: InventarioDetalleConta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Inventario_Detalle,Id_Inventario_Encabezado,Id_Departamento,Id_Producto,Fecha_Entrada,Fecha_Salida,Cantidad_Stock,Cantidad_Salida,Precio")] Inventario_Detalle_Conta inventario_Detalle_Conta)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(inventario_Detalle_Conta).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al actualizar el registro: " + ex.Message);
                }
            }

            ViewBag.Departamento = new SelectList(_context.Departamentos, "Id_Departamento", "Nombre_Departamento", inventario_Detalle_Conta.Id_Departamento);
            ViewBag.Producto = new SelectList(_context.Productos_Conta, "Id_Producto", "Nombre_producto", inventario_Detalle_Conta.Id_Producto);
            return View(inventario_Detalle_Conta);
        }

    }
}
