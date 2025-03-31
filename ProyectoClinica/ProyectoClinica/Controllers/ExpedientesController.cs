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

namespace ProyectoClinica.Controllers
{
    public class ExpedientesController : Controller
    {
        //Conexión BD
        private ApplicationDbContext BaseDatos = new ApplicationDbContext();
        // GET: Expedientes

        //[Authorize(Roles = "Administrador")]
        public ActionResult Expedientes()
        {
            return View();
        }

        //[Authorize(Roles = "Medico")]
        public ActionResult MdExpedientes()
        {
            return View();
        }

        //[Authorize(Roles = "Medico")]
        public ActionResult VistaHistorial()
        {
            var citas = BaseDatos.Cita.ToList();
            return View(citas);
        }


        //[Authorize(Roles = "Administrador")]
        public ActionResult VistaHistorialMd()
        {
            var citas = BaseDatos.Cita.ToList();
            return View(citas);
        }

        //[Authorize(Roles = "Administrador")]
        public ActionResult RolExpe()
        {
            ViewBag.Id_Empleado = new SelectList(BaseDatos.Empleado, "Id_Empleado", "Nombre");
            return View();
        }


        [HttpPost]
        //[Authorize(Roles = "Administrador")]
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

        public ActionResult SubirImagenes()
        {
            return View();
        }

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

    }




}




