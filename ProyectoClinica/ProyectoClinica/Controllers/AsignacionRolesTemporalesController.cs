using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoClinica.Models;

namespace ProyectoClinica.Controllers
{
    public class AsignacionRolesTemporalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AsignacionRolesTemporalesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: AsignacionRolesTemporales
        public ActionResult Index()
        {
            var asignacionRolesTemporales = _context.AsignacionRolesTemporales.Include(a => a.ApplicationUser);
            return View(asignacionRolesTemporales.ToList());
        }

        // GET: AsignacionRolesTemporales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AsignacionRolesTemporales asignacionRolesTemporales = _context.AsignacionRolesTemporales.Find(id);
            if (asignacionRolesTemporales == null)
            {
                return HttpNotFound();
            }
            return View(asignacionRolesTemporales);
        }

        // GET: AsignacionRolesTemporales/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(_context.AsignacionRolesTemporales, "Id", "Nombre");
            return View();
        }

        // POST: AsignacionRolesTemporales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_AsignacionRoles,Id,Fecha_Inicio,Fecha_Fin,Estado")] AsignacionRolesTemporales asignacionRolesTemporales)
        {
            if (ModelState.IsValid)
            {
                _context.AsignacionRolesTemporales.Add(asignacionRolesTemporales);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(_context.AsignacionRolesTemporales, "Id", "Nombre", asignacionRolesTemporales.Id);
            return View(asignacionRolesTemporales);
        }

        // GET: AsignacionRolesTemporales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AsignacionRolesTemporales asignacionRolesTemporales = _context.AsignacionRolesTemporales.Find(id);
            if (asignacionRolesTemporales == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(_context.AsignacionRolesTemporales, "Id", "Nombre", asignacionRolesTemporales.Id);
            return View(asignacionRolesTemporales);
        }

        // POST: AsignacionRolesTemporales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_AsignacionRoles,Id,Fecha_Inicio,Fecha_Fin,Estado")] AsignacionRolesTemporales asignacionRolesTemporales)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(asignacionRolesTemporales).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(_context.AsignacionRolesTemporales, "Id", "Nombre", asignacionRolesTemporales.Id);
            return View(asignacionRolesTemporales);
        }

        // GET: AsignacionRolesTemporales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AsignacionRolesTemporales asignacionRolesTemporales = _context.AsignacionRolesTemporales.Find(id);
            if (asignacionRolesTemporales == null)
            {
                return HttpNotFound();
            }
            return View(asignacionRolesTemporales);
        }

        // POST: AsignacionRolesTemporales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AsignacionRolesTemporales asignacionRolesTemporales = _context.AsignacionRolesTemporales.Find(id);
            _context.AsignacionRolesTemporales.Remove(asignacionRolesTemporales);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
