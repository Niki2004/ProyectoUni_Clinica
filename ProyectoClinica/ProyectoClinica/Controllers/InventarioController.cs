using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoClinica.Controllers
{
    public class InventarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventarioController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Inventario
        public ActionResult Index(string searchString, string tipoArticulo, string marca)
        {
            var inventario = _context.Inventario.AsQueryable();

            // Filtrar por nombre del artículo
            if (!string.IsNullOrEmpty(searchString))
            {
                inventario = inventario.Where(i => i.NombreArticulo.Contains(searchString));
            }

            // Filtrar por tipo de artículo
            if (!string.IsNullOrEmpty(tipoArticulo))
            {
                inventario = inventario.Where(i => i.TipoArticulo == tipoArticulo);
            }

            // Filtrar por marca
            if (!string.IsNullOrEmpty(marca))
            {
                inventario = inventario.Where(i => i.Marca == marca);
            }

            // Obtener lista única de marcas para el filtro
            ViewBag.Marcas = new SelectList(_context.Inventario.Select(i => i.Marca).Distinct());

            // Obtener lista única de tipos de artículo
            ViewBag.TiposArticulo = new SelectList(new List<string> { "Equipo Médico", "Medicamento", "Suministro" });

            return View(inventario.ToList());
        }

        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var inventario = _context.Inventario.Find(id);
            if (inventario == null)
            {
                return HttpNotFound();
            }

            return View(inventario);
        }


        [HttpGet]
        public ActionResult Crear()
        {
            ViewBag.Id_Estado = new SelectList(_context.Estado, "Id_Estado", "Descripcion");
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Inventario inventario)
        {
            inventario.FechaIngreso = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Inventario.Add(inventario);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Estado = new SelectList(_context.Estado, "Id_Estado", "Descripcion", inventario.Id_Estado);
            return View(inventario);
        }

        [HttpGet]

        public ActionResult Editar(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var inventario = _context.Inventario.Find(id);
            if (inventario == null)
                return HttpNotFound();

            ViewBag.Id_Estado = new SelectList(_context.Estado, "Id_Estado", "Descripcion", inventario.Id_Estado);
            return View(inventario);
        }

        [HttpPost]
        public ActionResult Editar(Inventario inventario)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(inventario).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Estado = new SelectList(_context.Estado, "Id_Estado", "Descripcion", inventario.Id_Estado);
            return View(inventario);
        }

        [HttpGet]
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var Inventario = _context.Inventario.SingleOrDefault(l => l.Id_Articulo == id);
            if (Inventario == null)
                return HttpNotFound();
            return View(Inventario);
        }

        [HttpPost, ActionName("Eliminar")]
        public ActionResult ConfirmarEliminar(int? id)
        {
            var inventario = _context.Inventario.Find(id);
            _context.Inventario.Remove(inventario);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }



    }

}
