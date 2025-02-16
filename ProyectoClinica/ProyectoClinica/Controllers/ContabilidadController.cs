using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
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

        //Vista para poder verlo más ordenado 
        [HttpGet]
        public ActionResult VistaCON()
        {
            return View();

        }
        public ActionResult VistaGastos()
        {
            var idSuministrosMedicos = _context.Tipo_Registro
                .Where(s => s.Nombre == "Gastos de suministro medicos")
                .Select(s => s.Id_Tipo_Registro)
                .FirstOrDefault(); // O SingleOrDefault() si solo debería haber un resultado



            var suministrosconta = _context.Contabilidad
            .Where(s => s.Id_Tipo_Registro == idSuministrosMedicos)
            .ToList();

            return View(suministrosconta);

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

        public ActionResult Pagos(int id)
        {
            var pago = _context.PagosXNomina.Find(id); // Buscar el pago en la tabla de Pagos

            if (pago == null)
            {
                return HttpNotFound();
            }

            return View("Index", pago); // Redirige a la vista "Pagos.cshtml" con el modelo
        }

        #region Detalles de contabilidad
        // GET: Contabilidad/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var contabilidad = await _context.Contabilidad.FirstOrDefaultAsync(m => m.Id_Contabilidad == id);

            if (contabilidad == null)
            {
                return View(contabilidad);
            }

            return View(contabilidad);
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
            ViewBag.Usuarios = new SelectList(_context.Users, "Id", "UserName");
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
                ViewBag.Usuarios = new SelectList(_context.Users, "Id", "UserName");
                return View(model);


            }
            catch
            {
                return View();
            }
        }
        #endregion

        // GET: Contabilidad/Edit/5
        public async Task<ActionResult>Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var contabilidad = _context.Contabilidad.Find(id);
            if (contabilidad == null)
                return HttpNotFound();

            ViewBag.Id_Tipo_Registro = new SelectList(_context.Tipo_Registro, "Id_Tipo_Registro", "Nombre", contabilidad.Id_Tipo_Registro);
            ViewBag.Id_Estado_Contabilidad = new SelectList(_context.Estado_Contabilidad, "Id_Estado_Contabilidad", "Nombre", contabilidad.Id_Estado_Contabilidad);
            ViewBag.Id_Tipo_Transaccion = new SelectList(_context.Tipo_Transaccion, "Id_Tipo_Transaccion", "Nombre", contabilidad.Id_Tipo_Transaccion);
            ViewBag.Usuarios = new SelectList(_context.Users, "Id", "UserName");
            return View(contabilidad);   
          

        }

        // POST: Contabilidad/Edit/5
        [HttpPost]
        public async Task<ActionResult>Edit(Contabilidad contabilidad)
        {

            if (ModelState.IsValid)
            {
                _context.Entry(contabilidad).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Tipo_Registro = new SelectList(_context.Tipo_Registro, "Id_Tipo_Registro", "Nombre", contabilidad.Id_Tipo_Registro);
            ViewBag.Id_Estado_Contabilidad = new SelectList(_context.Estado_Contabilidad, "Id_Estado_Contabilidad", "Nombre", contabilidad.Id_Estado_Contabilidad);
            ViewBag.Id_Tipo_Transaccion = new SelectList(_context.Tipo_Transaccion, "Id_Tipo_Transaccion", "Nombre", contabilidad.Id_Tipo_Transaccion);
            ViewBag.Usuarios = new SelectList(_context.Users, "Id", "UserName");
            return View(contabilidad);

        }

        private bool ContabilidadExists(int id)
        {
            return _context.Contabilidad.Any(e => e.Id_Contabilidad == id);
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
