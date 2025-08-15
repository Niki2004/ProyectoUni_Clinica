using Humanizer;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;

namespace ProyectoClinica.Controllers
{
    public class EmpleadosController : Controller
    {
        //Base de datos
        private ApplicationDbContext BaseDatos = new ApplicationDbContext();
        
        // Managers para gestión de roles de ASP.NET Identity
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmpleadosController()
        {
            BaseDatos = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(BaseDatos));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(BaseDatos));
        }

        [HttpGet]
        //Información de la clinica

        [Authorize(Roles = "Administrador")]
        public ActionResult VistaAdmin()
        {
            return View();

        }
        [Authorize(Roles = "Administrador")]
        public ActionResult Empleados()
        {
            var empleados = BaseDatos.Empleado.ToList(); // Obtiene la lista de empleados
            return View(empleados);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public ActionResult VistaEmpleados()
        {
            var empleados = BaseDatos.Empleado.ToList(); // vistas de empleado
            return View(empleados);
        }

        [HttpGet]
        public ActionResult vistaEliminar()
        {
            var empleados=BaseDatos.Empleado.ToList();
            return View(empleados);
        }


        //-----------------------------------------------------------------controller creacion de empleados----------------------------------------------------------------------

        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.Id_Estado = new SelectList(BaseDatos.Estado, "Id_Estado", "Descripcion");
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Estado,Comentarios,Nombre,Apellido,Cedula,Correo,Jornada,Fecha_registro,Departamento")] Empleado empleado)
        {
            if (empleado.Fecha_registro< DateTime.Today)
            {
                ModelState.AddModelError("Fecha_registro", "No se puede crear en una fecha pasada.");
            }

            var diaSemana = (int)empleado.Fecha_registro.DayOfWeek; // 0 = Domingo, 6 = Sábado
            if (diaSemana == 0 || diaSemana == 6)
            {
                ModelState.AddModelError("Fecha_registro", "No se puede crear los sábados ni domingos.");
            }

            if (ModelState.IsValid)
            {
                BaseDatos.Empleado.Add(empleado);
                BaseDatos.SaveChanges();
                TempData["SuccessMessage"] = "El empleado se ha creado correctamente.";//para que muestre el comentario en el index 
                return RedirectToAction("Empleados/Empleados");
            }
            ViewBag.Id_Estado = new SelectList(BaseDatos.Estado, "Id_Estado", "Descripcion", empleado.Id_Estado);
            return View(empleado);
        }





        //    //-----------------------------------------------------------------Controller Asignacion de roles-------------------------------------------------------------------------------------

        [Authorize(Roles = "Administrador")]
        public ActionResult Rol()
        {
            ViewBag.Id_Empleado = new SelectList(BaseDatos.Empleado, "Id_Empleado", "Nombre");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rol([Bind(Include = "Id_Empleado,Nombre")] RolAsignacion rolAsignacion)
        {
            if (ModelState.IsValid)
            {
                BaseDatos.RolAsignacion.Add(rolAsignacion);
                BaseDatos.SaveChanges();
                TempData["SuccessMessage"] = "Se Asigno el Rol Correctamente.";
                return RedirectToAction("Empleados/Empleados");
            }

            ViewBag.Id_Empleado = new SelectList(BaseDatos.Empleado, "Id_Empleado", "Nombre", rolAsignacion.Id_Empleado);
            return View(rolAsignacion);
        }



        //-----------------------------------------------------------------Controller Editar -------------------------------------------------------------------------------------

        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = BaseDatos.Empleado.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Estado = new SelectList(BaseDatos.Estado, "Id_Estado", "Descripcion", empleado.Id_Estado);
            return View(empleado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Empleado,Id_Estado,Comentarios,Nombre,Apellido,Cedula,Correo,Jornada,Fecha_registro,Departamento")] Empleado empleado)
        {
            if (empleado.Fecha_registro < DateTime.Today)
            {
                ModelState.AddModelError("Fecha_registro", "No se puede crear en una fecha pasada.");
            }

            var diaSemana = (int)empleado.Fecha_registro.DayOfWeek; // 0 = Domingo, 6 = Sábado
            if (diaSemana == 0 || diaSemana == 6)
            {
                ModelState.AddModelError("Fecha_registro", "No se puede crear los sábados ni domingos.");
            }
            if (ModelState.IsValid)
            {
                BaseDatos.Entry(empleado).State = EntityState.Modified;
                BaseDatos.SaveChanges();
                return RedirectToAction("VistaEmpleados");
            }
            ViewBag.Id_Estado = new SelectList(BaseDatos.Estado, "Id_Estado", "Descripcion", empleado.Id_Estado);
            return View(empleado);
        }



        //-----------------------------------------------------------------Controller Desactivar -------------------------------------------------------------------------------------

        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = BaseDatos.Empleado.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empleado empleado = BaseDatos.Empleado.Find(id);
            if (empleado != null)
            {
                BaseDatos.Empleado.Remove(empleado);
                BaseDatos.SaveChanges();
                TempData["SuccessMessage"] = "Se Elimino Correctamente";
            }
            return RedirectToAction("vistaEliminar");
        }

        //-----------------------------------------------------------------Controller Buscar -------------------------------------------------------------------------------------

        [Authorize(Roles = "Administrador")]
        public ActionResult Buscar(string nombre, string cedula)
        {
            var empleados = BaseDatos.Empleado.AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
            {
                empleados = empleados.Where(e => e.Nombre.Contains(nombre));
            }

            if (!string.IsNullOrEmpty(cedula))
            {
                empleados = empleados.Where(e => e.Cedula.Contains(cedula));
            }

            return View(empleados.ToList());
        }

        //-----------------------------------------------------------------Controller Agregar archivos  -------------------------------------------------------------------------------------


        [Authorize(Roles = "Administrador")]
        // aun no descarga el pdf solo lo sube a la base de datos
        public ActionResult file()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult File(HttpPostedFileBase file, string nombre)
        {
            if (file != null && file.ContentLength > 0)
            {
                
                if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                {
                    ModelState.AddModelError("file", "Solo se permiten archivos PDF.");
                    return View();
                }

                
                var fileName = Path.GetFileName(file.FileName);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

                
                var uploadDir = "~/Uploads";
                var uploadPath = Server.MapPath(uploadDir);

                
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var path = Path.Combine(uploadPath, uniqueFileName);

                
                file.SaveAs(path);

               
                PDF pdfRecord = new PDF
                {
                    Nombre = nombre,
                    Ruta = Path.Combine(uploadDir, uniqueFileName),
                    FechaSubida = DateTime.Now
                };

                BaseDatos.PDF.Add(pdfRecord);
                BaseDatos.SaveChanges();

                TempData["SuccessMessage"] = "El PDF se subio con exito.";
                return RedirectToAction("Empleados/Empleados");
            }
            else
            {
                ModelState.AddModelError("file", "Debe seleccionar un archivo PDF.");
            }

            return View();
        }

        //-----------------------------------------------------------------Controller ver los pdfs -------------------------------------------------------------------------------------

        [Authorize(Roles = "Administrador")]
        public ActionResult vistaPDF()
        {
            // Mapea la ruta ~/Uploads en el servidor
            var uploadsPath = Server.MapPath("~/Uploads");

            // Si la carpeta no existe, puedes crearla o manejar el error
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            // Obtiene todos los archivos con extensión .pdf
            var pdfFiles = Directory.GetFiles(uploadsPath, "*.pdf");

            return View(pdfFiles);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarPDF(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return Json(new { success = false, message = "Nombre de archivo no válido" });
                }

                var uploadsPath = Server.MapPath("~/Uploads");
                var filePath = Path.Combine(uploadsPath, fileName);

                if (System.IO.File.Exists(filePath))
                {
                    // Eliminar el registro de la base de datos si existe
                    var pdfRecord = BaseDatos.PDF.FirstOrDefault(p => p.Ruta.Contains(fileName));
                    if (pdfRecord != null)
                    {
                        BaseDatos.PDF.Remove(pdfRecord);
                        BaseDatos.SaveChanges();
                    }

                    // Eliminar el archivo físico
                    System.IO.File.Delete(filePath);
                    return Json(new { success = true, message = "Archivo eliminado correctamente" });
                }
                else
                {
                    return Json(new { success = false, message = "El archivo no existe" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar el archivo: " + ex.Message });
            }
        }

        //-----------------------------------------------------------------Controller Historial -------------------------------------------------------------------------------------
        [Authorize(Roles = "Administrador")]
        public ActionResult vistaHistorial()
        {
            var RolAsignacion = BaseDatos.RolAsignacion.Include("Empleado").ToList();
            return View(RolAsignacion);
        }
        public ActionResult detallesHistorial(int id)
        {
            var rolAsignacion = BaseDatos.RolAsignacion
                .Include("Empleado")
                .FirstOrDefault(r => r.Id_Rol == id);

            if (rolAsignacion == null)
            {
                return HttpNotFound();
            }

            return View(rolAsignacion);
        }


        //-----------------------------------------------------------------Controller Evaluacion -------------------------------------------------------------------------------------


        [Authorize(Roles = "Administrador")]
        public ActionResult Evaluacion()
        {
            
            ViewBag.Id_Empleado = new SelectList(BaseDatos.Empleado, "Id_Empleado", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Evaluacion(Evaluacion evaluacion)//creacion de la evaluacion 
        {
            if (evaluacion.Fecha_Evaluacion < DateTime.Today)
            {
                ModelState.AddModelError("Fecha_Evaluacion", "No se puede crear en una fecha pasada.");
            }

            var diaSemana = (int)evaluacion.Fecha_Evaluacion.DayOfWeek; // 0 = Domingo, 6 = Sábado
            if (diaSemana == 0 || diaSemana == 6)
            {
                ModelState.AddModelError("Fecha_Evaluacion", "No se puede crear los sábados ni domingos.");
            }
            if (ModelState.IsValid)
            {
                
                BaseDatos.Evaluacion.Add(evaluacion);
                BaseDatos.SaveChanges();
                TempData["SuccessMessage"] = "Se evaluo con exito";
                return RedirectToAction("vistaEvaluacion"); // Redirige a una lista de evaluaciones o a donde gustes
            }

            // Si la validación falla, vuelve a cargar los datos necesarios para la vista
            ViewBag.Id_Empleado = new SelectList(BaseDatos.Empleado, "Id_Empleado", "Nombre");
            return View(evaluacion);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult vistaEvaluacion()
        {
            
            var evaluaciones = BaseDatos.Evaluacion.Include("Empleado").ToList();
            return View(evaluaciones);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BaseDatos.Dispose();
                _userManager?.Dispose();
                _roleManager?.Dispose();
            }
            base.Dispose(disposing);
        }





        //---------------------------------------------------------------- AsignacionRolesTemporales -------------------------------------------------------------------------------------
        [Authorize(Roles = "Administrador")]
        public ActionResult vistaRolEmpleado()
        {
            var asignacionRolesTemporales = BaseDatos.AsignacionRolesTemporales.Include(a => a.ApplicationUser);
            return View(asignacionRolesTemporales.ToList());
        }


        // GET: AsignacionRolesTemporales/Details/5
        public ActionResult vistaRolDetalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AsignacionRolesTemporales asignacionRolesTemporales = BaseDatos.AsignacionRolesTemporales.Find(id);
            if (asignacionRolesTemporales == null)
            {
                return HttpNotFound();
            }
            return View(asignacionRolesTemporales);
        }


        // GET: AsignacionRolesTemporales/Create
        public ActionResult vistaRolCrear()
        {
            // Cargamos la lista de usuarios para ambos campos
            ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "UserName");
            ViewBag.Id_Usuario = new SelectList(BaseDatos.Users, "Id", "UserName");
            ViewBag.Id_Departamento = new SelectList(BaseDatos.Departamentos, "Id_Departamento", "Nombre_Departamento");
            return View();
        }

        // POST: AsignacionRolesTemporales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAsignacionRol(AsignacionRolesTemporales asignacionRolesTemporales)
        {
            // Validación de fechas
            DateTime hoy = DateTime.Today;
            if (asignacionRolesTemporales.Fecha_Inicio < hoy)
            {
                ModelState.AddModelError("Fecha_Inicio", "No se permiten fechas pasadas.");
                ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                ViewBag.Id_Usuario = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                ViewBag.Id_Departamento = new SelectList(BaseDatos.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                return View("vistaRolCrear", asignacionRolesTemporales);
            }
            
            if (asignacionRolesTemporales.Fecha_Inicio.DayOfWeek == DayOfWeek.Saturday || asignacionRolesTemporales.Fecha_Inicio.DayOfWeek == DayOfWeek.Sunday)
            {
                ModelState.AddModelError("Fecha_Inicio", "No se permiten fechas en sábado ni domingo.");
                ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                ViewBag.Id_Usuario = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                ViewBag.Id_Departamento = new SelectList(BaseDatos.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                return View("vistaRolCrear", asignacionRolesTemporales);
            }
            
            if (asignacionRolesTemporales.Fecha_Fin < hoy)
            {
                ModelState.AddModelError("Fecha_Fin", "No se permiten fechas pasadas.");
                ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                ViewBag.Id_Usuario = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                ViewBag.Id_Departamento = new SelectList(BaseDatos.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                return View("vistaRolCrear", asignacionRolesTemporales);
            }
            
            if (asignacionRolesTemporales.Fecha_Fin.DayOfWeek == DayOfWeek.Saturday || asignacionRolesTemporales.Fecha_Fin.DayOfWeek == DayOfWeek.Sunday)
            {
                ModelState.AddModelError("Fecha_Fin", "No se permiten fechas en sábado ni domingo.");
                ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                ViewBag.Id_Usuario = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                ViewBag.Id_Departamento = new SelectList(BaseDatos.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                return View("vistaRolCrear", asignacionRolesTemporales);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    // Verificar que los campos requeridos no sean nulos
                    if (asignacionRolesTemporales.Id == null || asignacionRolesTemporales.Id_Departamento == null)
                    {
                        ModelState.AddModelError("", "Debe seleccionar un usuario y un departamento");
                        throw new Exception("Campos requeridos faltantes");
                    }

                    // Verificar que no exista una asignación activa para el mismo usuario y departamento
                    var asignacionExistente = BaseDatos.AsignacionRolesTemporales
                        .FirstOrDefault(a => a.Id_Usuario == asignacionRolesTemporales.Id_Usuario && 
                                           a.Id_Departamento == asignacionRolesTemporales.Id_Departamento && 
                                           a.Estado == "Activo");

                    if (asignacionExistente != null)
                    {
                        ModelState.AddModelError("", "Ya existe una asignación activa para este usuario en este departamento.");
                        ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                        ViewBag.Id_Usuario = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                        ViewBag.Id_Departamento = new SelectList(BaseDatos.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                        return View("vistaRolCrear", asignacionRolesTemporales);
                    }

                    // Asignamos la fecha actual al crear el registro
                    asignacionRolesTemporales.Fecha_Inicio = DateTime.Now;
                    
                    // Asignamos el estado por defecto
                    asignacionRolesTemporales.Estado = "Activo";
                    
                    // Agregamos el registro al contexto
                    BaseDatos.AsignacionRolesTemporales.Add(asignacionRolesTemporales);
                    
                    // Intentamos guardar los cambios
                    BaseDatos.SaveChanges();
                    
                    // Asignar el rol correspondiente al usuario en AspNetUsers
                    AsignarRolDepartamento(asignacionRolesTemporales.Id_Usuario, asignacionRolesTemporales.Id_Departamento);
                    
                    // Crear notificación para el usuario asignado
                    CrearNotificacion(asignacionRolesTemporales.Id_Usuario, 
                                    "Se le ha asignado un rol temporal en el departamento " + 
                                    ObtenerNombreDepartamento(asignacionRolesTemporales.Id_Departamento));
                    
                    TempData["SuccessMessage"] = "Registro creado exitosamente y rol asignado al usuario";
                    return RedirectToAction("vistaRolEmpleado");
                }
                else
                {
                    // Si hay errores de validación, los mostramos
                    foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        ModelState.AddModelError("", modelError.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear el registro: " + ex.Message);
                // Log del error
                System.Diagnostics.Debug.WriteLine("Error en Create: " + ex.Message);
            }

            // Si hay error, recargamos las listas desplegables
            ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id);
            ViewBag.Id_Usuario = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
            ViewBag.Id_Departamento = new SelectList(BaseDatos.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
            return View("vistaRolCrear", asignacionRolesTemporales);
        }

        [Authorize(Roles = "Administrador")]

        // GET: AsignacionRolesTemporales/Edit/5
        public ActionResult vistaRolEditar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AsignacionRolesTemporales asignacionRolesTemporales = BaseDatos.AsignacionRolesTemporales.Find(id);
            if (asignacionRolesTemporales == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id);
            ViewBag.Id_Usuario = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
            ViewBag.Id_Departamento = new SelectList(BaseDatos.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
            return View(asignacionRolesTemporales);
        }
        [Authorize(Roles = "Administrador")]
        // POST: AsignacionRolesTemporales/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAsignacionRol([Bind(Include = "Id_AsignacionRoles,Id,Id_Usuario,Id_Departamento,Fecha_Inicio,Fecha_Fin,Estado,Motivo")] AsignacionRolesTemporales asignacionRolesTemporales)
        {
            // Validación de fechas
            DateTime hoy = DateTime.Today;
            if (asignacionRolesTemporales.Fecha_Inicio < hoy)
            {
                ModelState.AddModelError("Fecha_Inicio", "No se permiten fechas pasadas.");
                ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                ViewBag.Id_Usuario = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                ViewBag.Id_Departamento = new SelectList(BaseDatos.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                return View("vistaRolEditar", asignacionRolesTemporales);
            }
            
            if (asignacionRolesTemporales.Fecha_Inicio.DayOfWeek == DayOfWeek.Saturday || asignacionRolesTemporales.Fecha_Inicio.DayOfWeek == DayOfWeek.Sunday)
            {
                ModelState.AddModelError("Fecha_Inicio", "No se permiten fechas en sábado ni domingo.");
                ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                ViewBag.Id_Usuario = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                ViewBag.Id_Departamento = new SelectList(BaseDatos.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                return View("vistaRolEditar", asignacionRolesTemporales);
            }
            
            if (asignacionRolesTemporales.Fecha_Fin < hoy)
            {
                ModelState.AddModelError("Fecha_Fin", "No se permiten fechas pasadas.");
                ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                ViewBag.Id_Usuario = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                ViewBag.Id_Departamento = new SelectList(BaseDatos.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                return View("vistaRolEditar", asignacionRolesTemporales);
            }
            
            if (asignacionRolesTemporales.Fecha_Fin.DayOfWeek == DayOfWeek.Saturday || asignacionRolesTemporales.Fecha_Fin.DayOfWeek == DayOfWeek.Sunday)
            {
                ModelState.AddModelError("Fecha_Fin", "No se permiten fechas en sábado ni domingo.");
                ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id);
                ViewBag.Id_Usuario = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
                ViewBag.Id_Departamento = new SelectList(BaseDatos.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                return View("vistaRolEditar", asignacionRolesTemporales);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    // Verificar que los campos requeridos no sean nulos
                    if (asignacionRolesTemporales.Id == null || asignacionRolesTemporales.Id_Departamento == null)
                    {
                        ModelState.AddModelError("", "Debe seleccionar un usuario y un departamento");
                        throw new Exception("Campos requeridos faltantes");
                    }

                    // Obtener el registro original para comparar cambios
                    var asignacionOriginal = BaseDatos.AsignacionRolesTemporales
                        .AsNoTracking()
                        .FirstOrDefault(a => a.Id_AsignacionRoles == asignacionRolesTemporales.Id_AsignacionRoles);

                    // Actualizamos el registro
                    BaseDatos.Entry(asignacionRolesTemporales).State = EntityState.Modified;
                    
                    // Intentamos guardar los cambios
                    BaseDatos.SaveChanges();
                    
                    // Si cambió el departamento, actualizar el rol del usuario
                    if (asignacionOriginal != null && asignacionOriginal.Id_Departamento != asignacionRolesTemporales.Id_Departamento)
                    {
                        // Remover rol anterior
                        RemoverRolDepartamento(asignacionRolesTemporales.Id_Usuario, asignacionOriginal.Id_Departamento);
                        // Asignar nuevo rol
                        AsignarRolDepartamento(asignacionRolesTemporales.Id_Usuario, asignacionRolesTemporales.Id_Departamento);
                    }
                    
                    // Crear notificación si cambió el departamento o el estado
                    if (asignacionOriginal != null && 
                        (asignacionOriginal.Id_Departamento != asignacionRolesTemporales.Id_Departamento ||
                         asignacionOriginal.Estado != asignacionRolesTemporales.Estado))
                    {
                        string mensaje = "Su asignación de rol temporal ha sido modificada. ";
                        if (asignacionOriginal.Id_Departamento != asignacionRolesTemporales.Id_Departamento)
                        {
                            mensaje += $"Nuevo departamento: {ObtenerNombreDepartamento(asignacionRolesTemporales.Id_Departamento)}. ";
                        }
                        if (asignacionOriginal.Estado != asignacionRolesTemporales.Estado)
                        {
                            mensaje += $"Nuevo estado: {asignacionRolesTemporales.Estado}.";
                        }
                        
                        CrearNotificacion(asignacionRolesTemporales.Id_Usuario, mensaje);
                    }
                    
                    TempData["SuccessMessage"] = "Registro actualizado exitosamente";
                    return RedirectToAction("vistaRolEmpleado");
                }
                else
                {
                    // Si hay errores de validación, los mostramos
                    foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        ModelState.AddModelError("", modelError.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar el registro: " + ex.Message);
                // Log del error
                System.Diagnostics.Debug.WriteLine("Error en Edit: " + ex.Message);
            }

            // Si hay error, recargamos las listas desplegables
            ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id);
            ViewBag.Id_Usuario = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
            ViewBag.Id_Departamento = new SelectList(BaseDatos.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
            return View("vistaRolEditar", asignacionRolesTemporales);
        }



        public ActionResult vistaRolEliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AsignacionRolesTemporales asignacionRolesTemporales = BaseDatos.AsignacionRolesTemporales.Find(id);
            if (asignacionRolesTemporales == null)
            {
                return HttpNotFound();
            }
            return View(asignacionRolesTemporales);
        }

        // POST: AsignacionRolesTemporales/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarEliminacion(int id)
        {
            try
            {
                AsignacionRolesTemporales asignacionRolesTemporales = BaseDatos.AsignacionRolesTemporales
                    .Include(a => a.Departamentos)
                    .FirstOrDefault(a => a.Id_AsignacionRoles == id);
                
                if (asignacionRolesTemporales != null)
                {
                    // Remover el rol del usuario antes de eliminar la asignación
                    RemoverRolDepartamento(asignacionRolesTemporales.Id_Usuario, asignacionRolesTemporales.Id_Departamento);
                    
                    // Crear notificación antes de eliminar
                    CrearNotificacion(asignacionRolesTemporales.Id_Usuario, 
                                    "Su asignación de rol temporal en el departamento " + 
                                    ObtenerNombreDepartamento(asignacionRolesTemporales.Id_Departamento) + 
                                    " ha sido eliminada.");
                    
                    BaseDatos.AsignacionRolesTemporales.Remove(asignacionRolesTemporales);
                    BaseDatos.SaveChanges();
                    TempData["SuccessMessage"] = "Registro eliminado exitosamente y rol removido del usuario";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar el registro: " + ex.Message;
            }
            return RedirectToAction("vistaRolEmpleado");
        }

        // Método para asignar rol según el departamento
        private void AsignarRolDepartamento(string userId, int departamentoId)
        {
            try
            {
                var departamento = BaseDatos.Departamentos.FirstOrDefault(d => d.Id_Departamento == departamentoId);
                if (departamento == null) return;

                // Obtener el nombre del departamento y mapearlo al rol correspondiente
                string nombreDepartamento = departamento.Nombre_Departamento;
                string rolAsignar = MapearDepartamentoARol(nombreDepartamento);
                
                if (!string.IsNullOrEmpty(rolAsignar))
                {
                    // Verificar si el rol existe en AspNetRoles
                    if (_roleManager.RoleExists(rolAsignar))
                    {
                        // Obtener todos los roles actuales del usuario
                        var rolesActuales = _userManager.GetRoles(userId);
                        
                        // Remover todos los roles existentes (excepto Usuario si existe)
                        foreach (var rol in rolesActuales)
                        {
                            if (rol != "Usuario")
                            {
                                var resultadoRemover = _userManager.RemoveFromRole(userId, rol);
                                if (!resultadoRemover.Succeeded)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Error al remover rol {rol}: {string.Join(", ", resultadoRemover.Errors)}");
                                }
                            }
                        }
                        
                        // Asignar el nuevo rol
                        var resultadoAsignacion = _userManager.AddToRole(userId, rolAsignar);
                        if (resultadoAsignacion.Succeeded)
                        {
                            System.Diagnostics.Debug.WriteLine($"Rol {rolAsignar} asignado exitosamente al usuario {userId}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Error al asignar rol {rolAsignar} al usuario {userId}: {string.Join(", ", resultadoAsignacion.Errors)}");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"El rol {rolAsignar} no existe en AspNetRoles");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en AsignarRolDepartamento: {ex.Message}");
            }
        }

        // Método para remover rol según el departamento
        private void RemoverRolDepartamento(string userId, int departamentoId)
        {
            try
            {
                var departamento = BaseDatos.Departamentos.FirstOrDefault(d => d.Id_Departamento == departamentoId);
                if (departamento == null) return;

                string nombreDepartamento = departamento.Nombre_Departamento;
                string rolRemover = MapearDepartamentoARol(nombreDepartamento);
                
                if (!string.IsNullOrEmpty(rolRemover))
                {
                    var resultado = _userManager.RemoveFromRole(userId, rolRemover);
                    if (resultado.Succeeded)
                    {
                        System.Diagnostics.Debug.WriteLine($"Rol {rolRemover} removido exitosamente del usuario {userId}");
                        
                        // Asignar rol "Usuario" por defecto si no tiene ningún rol
                        var rolesActuales = _userManager.GetRoles(userId);
                        if (!rolesActuales.Any())
                        {
                            _userManager.AddToRole(userId, "Usuario");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Error al remover rol {rolRemover} del usuario {userId}: {string.Join(", ", resultado.Errors)}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en RemoverRolDepartamento: {ex.Message}");
            }
        }

        // Método para mapear departamento a rol
        private string MapearDepartamentoARol(string nombreDepartamento)
        {
            // Mapeo directo de departamentos a roles existentes en AspNetRoles
            switch (nombreDepartamento.ToLower())
            {
                case "contabilidad":
                    return "Contador";
                case "medicina":
                    return "Medico";
                case "secretaria":
                    return "Secretaria";
                case "auditor":
                    return "Auditor";
                case "administrador":
                    return "Administrador";
                case "usuario":
                    return "Usuario";
                default:
                    // Si no hay mapeo específico, retornar el nombre del departamento tal como está
                    // asumiendo que coincide con el nombre del rol en AspNetRoles
                    return nombreDepartamento;
            }
        }

        // Método para verificar acceso al departamento
        private bool TieneAccesoDepartamento(string userId, int departamentoId)
        {
            return BaseDatos.AsignacionRolesTemporales
                .Any(a => a.Id_Usuario == userId && 
                          a.Id_Departamento == departamentoId && 
                          a.Estado == "Activo" &&
                          a.Fecha_Fin >= DateTime.Now);
        }

        // Método para obtener el nombre del departamento
        private string ObtenerNombreDepartamento(int departamentoId)
        {
            var departamento = BaseDatos.Departamentos.FirstOrDefault(d => d.Id_Departamento == departamentoId);
            return departamento?.Nombre_Departamento ?? "Departamento Desconocido";
        }

        // Método para crear notificaciones
        private void CrearNotificacion(string userId, string mensaje)
        {
            try
            {
                var notificacion = new Notificacion
                {
                    Mensaje = mensaje,
                    Fecha = DateTime.Now
                };

                BaseDatos.Notificacion.Add(notificacion);
                BaseDatos.SaveChanges();

                // También almacenamos en sesión para notificaciones en tiempo real
                var notificaciones = Session["Notificaciones"] as List<string> ?? new List<string>();
                notificaciones.Add(mensaje);
                Session["Notificaciones"] = notificaciones;
                Session["ContadorNotificaciones"] = notificaciones.Count;
            }
            catch (Exception ex)
            {
                // Log del error de notificación
                System.Diagnostics.Debug.WriteLine("Error al crear notificación: " + ex.Message);
            }
        }


    }

}