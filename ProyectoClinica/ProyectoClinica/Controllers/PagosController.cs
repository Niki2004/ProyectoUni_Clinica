using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProyectoClinica.Controllers
{
    public class PagosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagosController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize(Roles = "Administrador,Contador")]
        // GET: Pagos
        public ActionResult Index()
        {
            var listaRegistros = _context.Pagos.ToList();
            return View(listaRegistros);

        }

        [Authorize(Roles = "Administrador,Contador")]
        // GET: Pagos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return View();

            var pago = _context.Pagos.Find(id);


            // Obtener la lista de bancos desde la base de datos
            var bancos = _context.Bancos
                                 .Select(b => new SelectListItem
                                 {
                                     Value = b.Id_Banco.ToString(),
                                     Text = b.Nombre_Banco
                                 })
                                 .ToList();

            ViewBag.Banco = new SelectList(bancos, "Value", "Text", pago.Id_Banco);




            if (pago == null)
            {
                return View(pago);
            }

            return View(pago);
        }

        [Authorize(Roles = "Administrador,Contador")]
        // GET: Pagos/Create
        public ActionResult Create()
        {
            //ViewBag.Pagos = new SelectList(_context.Pagos, "Id_Pago");


            ViewBag.Banco = new SelectList(_context.Bancos, "Id_Banco", "Nombre_Banco");

            return View();
        }

        // POST: Pagos/Create  
        [HttpPost]
        public ActionResult Create(Pagos model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        //Guardar en la base de datos
                        _context.Pagos.Add(model);
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
                ViewBag.Pagos = new SelectList(_context.Pagos, "Id_Pago");
                return View(model);


            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Administrador,Contador")]
        // GET: Pagos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Usar SingleOrDefaultAsync para búsqueda asíncrona
            var pago = await _context.Pagos.SingleOrDefaultAsync(p => p.Id_Pago == id);

            if (pago == null)
            {
                return HttpNotFound();
            }

            // Cargar la lista de bancos una sola vez
            var bancos = await _context.Bancos.ToListAsync();
            ViewBag.Banco = new SelectList(bancos, "Id_Banco", "Nombre_Banco", pago.Id_Banco);

            return View(pago);
        }

        // POST: Pagos/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Pagos pago)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener el pago existente de la base de datos
                    var pagoExistente = await _context.Pagos.SingleOrDefaultAsync(p => p.Id_Pago == pago.Id_Pago);

                    if (pagoExistente == null)
                    {
                        return HttpNotFound();
                    }

                    // Mantener el Id_Banco original
                    pago.Id_Banco = pagoExistente.Id_Banco;

                    // Actualizar solo los campos modificables
                    _context.Entry(pagoExistente).CurrentValues.SetValues(pago);

                    // Guardar los cambios
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "No se pudo guardar los cambios. Inténtelo de nuevo y si el problema persiste, contacte al administrador.");
                }
            }

            // Si llegamos aquí, algo falló, volver a cargar los bancos
            var bancos = await _context.Bancos.ToListAsync();
            ViewBag.Banco = new SelectList(bancos, "Id_Banco", "Nombre_Banco", pago.Id_Banco);

            return View(pago);
        }

    }
}
