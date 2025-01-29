using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoClinica.Controllers
{
    public class ContabilidadController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ContabilidadController()
        {
            _context = new ApplicationDbContext();
        }

        #region Index
        // GET: Contabilidad
        //[Authorize(Roles = "Contador")]
        public ActionResult Index()
        {
            var listaRegistros = _context.Contabilidad.ToList();
            return View(listaRegistros);
        }
        #endregion

        #region Detalles de contabilidad
        // GET: Contabilidad/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        #endregion

        // GET: Contabilidad/Create
        #region Creacion Contabilidad
        public ActionResult Create()
        {
            //tipo de Registro
            ViewBag.TipoRegistro = new SelectList(_context.Tipo_Registro, "Id_Tipo_Registro", "Nombre");
            ViewBag.Estado_Contabilidad = new SelectList(_context.Estado_Contabilidad, "Id_Estado_Contabilidad", "Nombre");
            ViewBag.TipoTransaccion = new SelectList(_context.Tipo_Transaccion, "Id_Tipo_Transaccion", "Nombre");
            return View();
        }
        
        // POST: Contabilidad/Create
        [HttpPost]
        public ActionResult Create(Contabilidad model)
        {

            try
            {

                if (ModelState.IsValid)
                {
                    int idSeleccionado = model.Id_Tipo_Transaccion; // Aquí obtienes el ID del dropdown

                    try
                    {
                        // Guardar en la base de datos
                        _context.Contabilidad.Add(model);
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
                ViewBag.TipoRegistro = new SelectList(_context.Tipo_Registro, "Id_Tipo_Registro", "Nombre");
                ViewBag.Estado_Contabilidad = new SelectList(_context.Estado_Contabilidad, "Id_Estado_Contabilidad", "Nombre");
                ViewBag.TipoTransaccion = new SelectList(_context.Tipo_Transaccion, "Id_Tipo_Transaccion", "Nombre");

                return View(model);


            }
            catch
            {
                return View();
            }
        }
        #endregion

        // GET: Contabilidad/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Contabilidad/Edit/5
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

        // GET: Contabilidad/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Contabilidad/Delete/5
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
