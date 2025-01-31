using Humanizer;
using Microsoft.Ajax.Utilities;
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

namespace ProyectoClinica.Controllers
{
    public class EmpleadosController : Controller
    {
        //Base de datos
        private ApplicationDbContext BaseDatos = new ApplicationDbContext();

        // GET: Empleados
        //[Authorize(Roles = "Administrador")]

        [HttpGet]
        public ActionResult Empleados()
        {
            var empleados = BaseDatos.Empleado.ToList(); // Obtiene la lista de empleados
            return View(empleados);
        }


        //-----------------------------------------------------------------controller creacion de empleados----------------------------------------------------------------------
        //[HttpGet]
        //public ActionResult CrearEmpleado()
        //{
        //    ViewBag.Medico = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre");
        //    ViewBag.Estado = new SelectList(BaseDatos.Estado_Asistencia, "Id_Estado", "Nombre");
        //    ViewBag.Users = new SelectList(BaseDatos.Users, "Id_Usuario", "Nombre");
        //    return View();
        //}

        [HttpGet]
        public ActionResult CrearEmpleado()
        {
            // Preparar datos necesarios para listas desplegables si las tienes
            ViewBag.Medicos = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre");
            ViewBag.Estados = new SelectList(BaseDatos.Estado, "Id_Estado", "Descripcion");
            ViewBag.Usuarios = new SelectList(BaseDatos.Users, "Id", "UserName");

            var empleado = new Empleado();
            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearEmpleado(Empleado empleado)
        {
           
            try
            {
                // Agregar valores por defecto para campos requeridos
                empleado.Fecha_registro = DateTime.Now;
                empleado.Fecha_actualizacion = DateTime.Now;
                empleado.Fecha_proxima_evaluacion = DateTime.Now.AddMonths(6); // ejemplo
                empleado.Administrador_modificacion = "Sistema"; // o el usuario actual
                empleado.documentos = ""; // o un valor por defecto
                empleado.Departamento = ""; // o un valor por defecto
                empleado.Historial_capacitaciones = ""; // o un valor por defecto

                // Intentar guardar
                BaseDatos.Empleado.Add(empleado);
                BaseDatos.SaveChanges();

                TempData["SuccessMessage"] = "El empleado se ha creado correctamente.";
                return RedirectToAction("Empleados");
            }
            catch (Exception ex)
            {
                // Loguear el error completo
                var mensajeError = ex.Message;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    mensajeError += " | " + ex.Message;
                }
                ModelState.AddModelError("", "Error al crear el empleado: " + mensajeError);
            }

            // Recargar las listas desplegables
            ViewBag.Medicos = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre", empleado.Id_Medico);
            ViewBag.Estados = new SelectList(BaseDatos.Estado, "Id_Estado", "Descripcion", empleado.Id_Estado);
            ViewBag.Usuarios = new SelectList(BaseDatos.Users, "Id", "UserName", empleado.Id_Usuario);
            return View(empleado);
        }



        //[HttpPost]
        //public ActionResult CrearEmpleado(Empleado empleado)
        //{
        //    //if (ModelState.IsValid)
        //    //{
        //    //    BaseDatos.Empleado.Add(empleado);
        //    //    BaseDatos.SaveChanges();
        //    //    return RedirectToAction("Empleados");

        //    //}
        //    //return View(empleado);

        //    try
        //    {

        //        if (ModelState.IsValid)
        //        {
        //            int idSeleccionado = empleado.Id_Empleado; // Aquí obtienes el ID del dropdown

        //            try
        //            {
        //                // Guardar en la base de datos
        //                BaseDatos.Empleado.Add(empleado);
        //                BaseDatos.SaveChanges();

        //                return RedirectToAction("Empleados");
        //            }
        //            catch (Exception ex)
        //            {
        //                // Si hay un error, muestra el mensaje en el log o en el modelo para que el usuario lo vea
        //                ModelState.AddModelError("", "Ocurrió un error al guardar los datos: " + ex.Message);
        //            }



        //        }

        //        // Si el modelo no es válido o hubo un error, repite el proceso y pasa la vista con el modelo
        //        // Esto permitirá que los datos enviados por el usuario se mantengan en el formulario
        //        ViewBag.Medico = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre");
        //        ViewBag.Estado = new SelectList(BaseDatos.Estado_Asistencia, "Id_Estado", "Nombre");
        //        ViewBag.Users = new SelectList(BaseDatos.Users, "Id_Usuario", "Nombre");

        //        return View(empleado);


        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}




        //    //-----------------------------------------------------------------Controller Asignacion de roles-------------------------------------------------------------------------------------

        [HttpGet]
        public ActionResult AsignacionRoles()
        {
            return View();
        }



        [HttpPost]
        public ActionResult AsignacionRoles(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                BaseDatos.Empleado.Add(empleado);
                BaseDatos.SaveChanges();

                return RedirectToAction("Empleados");
            }


            return View(empleado);
        }


        //-----------------------------------------------------------------Controller Editar -------------------------------------------------------------------------------------

        //controller Editar
        [HttpGet]
        public ActionResult EditarEmpleado(int id)
        {
            // Buscar el empleado en la base de datos por su ID
            var empleado = BaseDatos.Empleado.Find(id);

            if (empleado == null)
            {
                return HttpNotFound(); // Si el empleado no se encuentra, retorna error 
            }

            return View(empleado); // Pasa el empleado a la vista para su edición
        }

        [HttpPost]
        public ActionResult EditarEmpleado(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Buscar el empleado en la base de datos por su ID
                    var empleadoExistente = BaseDatos.Empleado.Find(empleado.Id_Empleado);

                    if (empleadoExistente == null)
                    {
                        return HttpNotFound(); // Si no se encuentra el empleado, retorna error 404
                    }

                    // Actualiza los valores del empleado existente
                    empleadoExistente.Nombre = empleado.Nombre;
                    empleadoExistente.Apellido = empleado.Apellido;
                    empleadoExistente.Cedula = empleado.Cedula;
                    empleadoExistente.Correo = empleado.Correo;
                    empleadoExistente.Departamento = empleado.Departamento;
                    empleadoExistente.FechaInicio = empleado.FechaInicio;
                    empleadoExistente.FechaFin = empleado.FechaFin;

                    // Guarda los cambios
                    BaseDatos.SaveChanges();
                    return RedirectToAction("Empleados"); // Redirige a la lista de empleados
                }
                catch (Exception ex)
                {
                    // Si ocurre un error, se agrega el mensaje al modelo
                    ModelState.AddModelError("", "Error al actualizar el empleado: " + ex.Message);
                }
            }

            return View(empleado); // Si el modelo no es válido, vuelve a mostrar el formulario con los errores
        }




        //-----------------------------------------------------------------Controller Desactivar -------------------------------------------------------------------------------------

        //preguntar
        public ActionResult DesactivarPerfil()
        {
            return View();
        }



    }

}