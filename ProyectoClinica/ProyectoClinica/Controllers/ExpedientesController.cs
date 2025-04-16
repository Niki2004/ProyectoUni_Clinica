using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using ProyectoClinica.Models;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;
using System.Net;

namespace ProyectoClinica.Controllers
{
    public class ExpedientesController : Controller
    {
        //Conexión BD
        private ApplicationDbContext BaseDatos = new ApplicationDbContext();
        // GET: Expedientes

        [Authorize(Roles = "Administrador")]
        public ActionResult Expedientes()
        {
            return View();
        }

        [Authorize(Roles = "Medico")]
        public ActionResult MdExpedientes()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult VistaHistorial()
        {
            var citas = BaseDatos.Cita.ToList();
            return View(citas);
        }


        [Authorize(Roles = "Medico")]
        public ActionResult VistaHistorialMd()
        {
            var citas = BaseDatos.Cita.ToList();
            return View(citas);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult RolExpe()
        {
            ViewBag.Id_Empleado = new SelectList(BaseDatos.Empleado, "Id_Empleado", "Nombre");
            return View();
        }

       [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RolExpe([Bind(Include = "Id_Empleado,Nombre")] RolAsignacion rolAsignacion)
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



        [Authorize(Roles = "Administrador")]
        public ActionResult drive()
        {
            return View();
        }

        static async Task Main(string[] args)
        {
            string credentialPath = @"C:\path\to\credentials.json"; 
            string filePath = @"C:\path\to\file.pdf"; 
            string folderId = "ID_DE_LA_CARPETA"; 

            GoogleCredential credential = GoogleCredential.FromFile(credentialPath)
                .CreateScoped(DriveService.ScopeConstants.DriveFile);

            DriveService service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "MiAppDrive"
            });

            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = Path.GetFileName(filePath),
                Parents = folderId != null ? new[] { folderId } : null
            };

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) 
            {
                var request = service.Files.Create(fileMetadata, stream, "application/octet-stream");
                request.Fields = "id";
                var result = await request.UploadAsync();

                if (result.Status == UploadStatus.Completed)
                {
                    Console.WriteLine("Archivo subido correctamente, ID: " + request.ResponseBody.Id);
                }
                else
                {
                    Console.WriteLine("Error al subir archivo: " + result.Exception.Message);
                }
            } 
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Respaldo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubirADrive(HttpPostedFileBase archivo)
        {
            try
            {
                if (archivo != null && archivo.ContentLength > 0)
                {
                    string uploadPath = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    string fileName = Path.GetFileName(archivo.FileName);
                    string uniqueFileName = $"{DateTime.Now:yyyyMMddHHmmss}_{fileName}";
                    string filePath = Path.Combine(uploadPath, uniqueFileName);

                    archivo.SaveAs(filePath);

                    TempData["SuccessMessage"] = "Archivo guardado exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Por favor seleccione un archivo válido.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al guardar el archivo: " + ex.Message;
            }

            return RedirectToAction("Respaldo");
        }

        [Authorize(Roles = "Medico")]
        public ActionResult SubirImagenes()
        {
            return View();
        }

        [Authorize(Roles = "Medico")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubirImagenes(IEnumerable<HttpPostedFileBase> imagenes)
        {
            try
            {
                if (imagenes != null && imagenes.Any())
                {
                    string uploadPath = Server.MapPath("~/Imagenes/");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    foreach (var imagen in imagenes)
                    {
                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            string fileName = Path.GetFileName(imagen.FileName);
                            string uniqueFileName = $"{DateTime.Now:yyyyMMddHHmmss}_{fileName}";
                            string filePath = Path.Combine(uploadPath, uniqueFileName);

                            imagen.SaveAs(filePath);
                        }
                    }

                    TempData["SuccessMessage"] = "Imágenes guardadas exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Por favor seleccione al menos una imagen.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al guardar las imágenes: " + ex.Message;
            }

            return RedirectToAction("SubirImagenes");
        }


        //---------------------------------------------------------------- AsignacionRolesTemporales -------------------------------------------------------------------------------------

        public ActionResult vistaRolExpedienteExp()
        {
            var asignacionRolesTemporales = BaseDatos.AsignacionRolesTemporales
                .Include("ApplicationUser")
                .Include("UsuarioAsignado")
                .Include("Departamentos")
                .ToList();
            return View(asignacionRolesTemporales);
        }


        // GET: AsignacionRolesTemporales/Details/5

        public ActionResult vistaRolDetallesExp(int? id)
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
        public ActionResult vistaRolCrearExp()
        {
            
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

                   
                    asignacionRolesTemporales.Fecha_Inicio = DateTime.Now;

                   
                    asignacionRolesTemporales.Estado = "Activo";

                    
                    BaseDatos.AsignacionRolesTemporales.Add(asignacionRolesTemporales);

                    
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

            
            ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id);
            ViewBag.Id_Usuario = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
            ViewBag.Id_Departamento = new SelectList(BaseDatos.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
            return View("vistaRolCrear", asignacionRolesTemporales);
        }




        // GET: AsignacionRolesTemporales/Edit/5
        public ActionResult vistaRolEditarExp(int? id)
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

                    
                    BaseDatos.Entry(asignacionRolesTemporales).State = EntityState.Modified;

                    
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

            
            ViewBag.Id = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id);
            ViewBag.Id_Usuario = new SelectList(BaseDatos.Users, "Id", "UserName", asignacionRolesTemporales.Id_Usuario);
            ViewBag.Id_Departamento = new SelectList(BaseDatos.Departamentos, "Id_Departamento", "Nombre_Departamento", asignacionRolesTemporales.Id_Departamento);
            return View("vistaRolEditar", asignacionRolesTemporales);
        }


        public ActionResult vistaRolEliminarExp(int? id)
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




