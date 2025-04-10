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

        [HttpGet]
        //Información de la clinica
        public ActionResult VistaAdmin()
        {
            return View();

        }

        public ActionResult Empleados()
        {
            var empleados = BaseDatos.Empleado.ToList(); // Obtiene la lista de empleados
            return View(empleados);
        }

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
            }
            base.Dispose(disposing);
        }





        //---------------------------------------------------------------- AsignacionRolesTemporales -------------------------------------------------------------------------------------

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

                    // Asignamos la fecha actual al crear el registro
                    asignacionRolesTemporales.Fecha_Inicio = DateTime.Now;

                    // Asignamos el estado por defecto
                    asignacionRolesTemporales.Estado = "Activo";

                    // Agregamos el registro al contexto
                    BaseDatos.AsignacionRolesTemporales.Add(asignacionRolesTemporales);

                    // Intentamos guardar los cambios
                    BaseDatos.SaveChanges();

                    TempData["SuccessMessage"] = "Registro creado exitosamente";
                    return RedirectToAction("vistaRolEmpleado");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear el registro: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Error en Create: " + ex.Message);
            }

            // Si hay error, recargamos las listas desplegables
            ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id);
            ViewBag.Id_Usuario = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
            ViewBag.Id_Departamento = new SelectList(BaseDatos.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
                return View("vistaRolCrear", asignacionRolesTemporales);
        }



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

        // POST: AsignacionRolesTemporales/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAsignacionRol([Bind(Include = "Id_AsignacionRoles,Id,Id_Usuario,Id_Departamento,Fecha_Inicio,Fecha_Fin,Estado,Motivo")] AsignacionRolesTemporales asignacionRolesTemporales)
        {
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

                    // Actualizamos el registro
                    BaseDatos.Entry(asignacionRolesTemporales).State = EntityState.Modified;

                    // Intentamos guardar los cambios
                    BaseDatos.SaveChanges();

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
                AsignacionRolesTemporales asignacionRolesTemporales = BaseDatos.AsignacionRolesTemporales.Find(id);
                if (asignacionRolesTemporales != null)
                {
                    BaseDatos.AsignacionRolesTemporales.Remove(asignacionRolesTemporales);
                    BaseDatos.SaveChanges();
                    TempData["SuccessMessage"] = "Registro eliminado exitosamente";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar el registro: " + ex.Message;
            }
            return RedirectToAction("vistaRolEmpleado");
        }



    }

}