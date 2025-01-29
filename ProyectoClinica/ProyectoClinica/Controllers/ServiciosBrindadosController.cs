using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoClinica.Controllers
{
    public class ServiciosBrindadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiciosBrindadosController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: ServiciosBrindados
        public ActionResult Index()
        {
            var listaRegistros = _context.Servicios_Brindados.ToList();
            return View(listaRegistros);
        }

        // GET: ServiciosBrindados/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ServiciosBrindados/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiciosBrindados/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ServiciosBrindados/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ServiciosBrindados/Edit/5
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

        // GET: ServiciosBrindados/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ServiciosBrindados/Delete/5
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
