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
    public class ConciliacionesBancariasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConciliacionesBancariasController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: ConciliacionesBancarias
        public ActionResult Index()
        {
            var listaRegistros = _context.Conciliaciones_Bancarias.ToList();
            return View(listaRegistros);
        }

        // GET: ConciliacionesBancarias/Details/5
        public async Task<ActionResult> Details(int? id)
        {
           if (id == null)
                return View();

            var conciliacion = _context.Conciliaciones_Bancarias.Find(id);


            // Obtener la lista de bancos desde la base de datos
            var bancos = _context.Bancos
                                 .Select(b => new SelectListItem
                                 {
                                     Value = b.Id_Banco.ToString(),
                                     Text = b.Nombre_Banco
                                 })
                                 .ToList();

            var diarios = _context.Diarios_Contables
                                .Select(b => new SelectListItem
                                {
                                    Value = b.Id_Diario.ToString(),
                                    Text = b.Descripcion
                                })
                                .ToList();

            var registro = _context.Tipo_Registro
                                .Select(b => new SelectListItem
                                {
                                    Value = b.Id_Tipo_Registro.ToString(),
                                    Text = b.Nombre
                                })
                                .ToList();

            ViewBag.Banco = new SelectList(bancos, "Value", "Text", conciliacion.Id_Banco);
            ViewBag.Diarios_Contables = new SelectList(diarios, "Value", "Text", conciliacion.Id_Diario);
            ViewBag.TipoRegistro = new SelectList(registro, "Value", "Text", conciliacion.Tipo_Registro);




            if (conciliacion == null)
            {
                return View(conciliacion);
            }

            return View(conciliacion);
        }

        // GET: ConciliacionesBancarias/Create
        public ActionResult Create()
        {
            ViewBag.Banco = new SelectList(_context.Bancos, "Id_Banco", "Nombre_Banco");
            ViewBag.Diarios_Contables = new SelectList(_context.Diarios_Contables, "Id_Diario", "Descripcion");
            ViewBag.TipoRegistro = new SelectList(_context.Tipo_Registro, "Id_Tipo_Registro", "Nombre");

            return View();
        }

        // POST: ConciliacionesBancarias/Create
        [HttpPost]
        public ActionResult Create(Conciliaciones_Bancarias model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        //Guardar en la base de datos
                        _context.Conciliaciones_Bancarias.Add(model);
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
                ViewBag.Banco = new SelectList(_context.Bancos, "Id_Banco", "Nombre_Banco");
                ViewBag.Diarios_Contables = new SelectList(_context.Diarios_Contables, "Id_Diario", "Descripcion");
                ViewBag.TipoRegistro = new SelectList(_context.Tipo_Registro, "Id_Tipo_Registro", "Nombre");
                return View(model);


            }
            catch
            {
                return View();
            }
        }

        // GET: ConciliacionesBancarias/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.Bancos = new SelectList(_context.Bancos, "Id_Banco", "Nombre_Banco");
            ViewBag.Diarios_Contables = new SelectList(_context.Diarios_Contables, "Id_Diario", "Descripcion");
            ViewBag.TipoRegistro = new SelectList(_context.Tipo_Registro, "Id_Tipo_Registro", "Nombre");

            var conciliacion = _context.Conciliaciones_Bancarias.Find(id);

            if (conciliacion == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            }

            // Obtener la lista de bancos desde la base de datos
            var bancos = _context.Bancos
                                 .Select(b => new SelectListItem
                                 {
                                     Value = b.Id_Banco.ToString(),
                                     Text = b.Nombre_Banco
                                 })
                                 .ToList();

            var diarios = _context.Diarios_Contables
                                .Select(b => new SelectListItem
                                {
                                    Value = b.Id_Diario.ToString(),
                                    Text = b.Descripcion
                                })
                                .ToList();

            var registro = _context.Tipo_Registro
                                .Select(b => new SelectListItem
                                {
                                    Value = b.Id_Tipo_Registro.ToString(),
                                    Text = b.Nombre
                                })
                                .ToList();

            Console.WriteLine(bancos);

            // Pasar la lista de bancos a la vista usando ViewBag
            ViewBag.Banco = new SelectList(bancos, "Value", "Text", conciliacion.Id_Banco);
            ViewBag.Diarios_Contables = new SelectList(diarios, "Value", "Text", conciliacion.Id_Diario);
            ViewBag.TipoRegistro = new SelectList(registro, "Value", "Text", conciliacion.Tipo_Registro);

            return View(conciliacion);
        }

        // POST: ConciliacionesBancarias/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Conciliaciones_Bancarias concilaicion)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(concilaicion).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.Bancos = new SelectList(_context.Bancos, "Id_Banco", "Nombre_Banco");

            return View(concilaicion);
        }

        // GET: ConciliacionesBancarias/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ConciliacionesBancarias/Delete/5
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
