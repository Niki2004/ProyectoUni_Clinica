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

        [HttpGet]
        public ActionResult VistaEmpleados()
        {
            var empleados = BaseDatos.Empleado.ToList(); // vistas de empleado
            return View(empleados);
        }


        //-----------------------------------------------------------------controller creacion de empleados----------------------------------------------------------------------


        //[HttpGet]
        //public ActionResult CrearEmpleado()
        //{
        //    // Preparar datos necesarios para listas desplegables si las tienes
            
        //    ViewBag.Estados = new SelectList(BaseDatos.Estado, "Id_Estado", "Descripcion");
           

        //    var empleado = new Empleado();
        //    return View(empleado);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CrearEmpleado(Empleado empleado)
        //{
           
        //    try
        //    {
        //        // Agregar valores por defecto para campos requeridos
        //        empleado.Fecha_registro = DateTime.Now;

        //        empleado.Departamento = ""; // o un valor por defecto
               

        //        // Intentar guardar
        //        BaseDatos.Empleado.Add(empleado);
        //        BaseDatos.SaveChanges();

        //        TempData["SuccessMessage"] = "El empleado se ha creado correctamente.";
        //        return RedirectToAction("Empleados");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Loguear el error completo
        //        var mensajeError = ex.Message;
        //        while (ex.InnerException != null)
                
        //            mensajeError += " | " + ex.Message;
        //        }
        //        ModelState.AddModelError("", "Error al crear el empleado: " + mensajeError);
        //    }

        //    // Recargar las listas desplegables
            
        //    ViewBag.Estados = new SelectList(BaseDatos.Estado, "Id_Estado", "Descripcion", empleado.Id_Estado);
           
        //    return View(empleado);
        //}






        //    //-----------------------------------------------------------------Controller Asignacion de roles-------------------------------------------------------------------------------------

        //[HttpGet]
        //public ActionResult AsignacionRoles()
        //{
        //    // Preparar datos necesarios para listas desplegables si las tienes
        //    ViewBag.Medicos = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre");
        //    ViewBag.Estados = new SelectList(BaseDatos.Estado, "Id_Estado", "Descripcion");
        //    ViewBag.Usuarios = new SelectList(BaseDatos.Users, "Id", "UserName");

        //    var empleado = new Empleado();
        //    return View(empleado);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AsignacionRoles(Empleado empleado)
        //{

        //    try
        //    {
        //        // Agregar valores por defecto para campos requeridos
        //        empleado.Fecha_registro = DateTime.Now;
        //        empleado.Fecha_actualizacion = DateTime.Now;
        //        empleado.Fecha_proxima_evaluacion = DateTime.Now.AddMonths(6); // ejemplo
        //        empleado.Administrador_modificacion = "Sistema"; // o el usuario actual
        //        empleado.documentos = ""; // o un valor por defecto
        //        empleado.Departamento = ""; // o un valor por defecto
        //        empleado.Historial_capacitaciones = ""; // o un valor por defecto

        //        // Intentar guardar
        //        BaseDatos.Empleado.Add(empleado);
        //        BaseDatos.SaveChanges();

        //        TempData["SuccessMessage"] = "El empleado se ha creado correctamente.";
        //        return RedirectToAction("Empleados");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Loguear el error completo
        //        var mensajeError = ex.Message;
        //        while (ex.InnerException != null)
        //        {
        //            ex = ex.InnerException;
        //            mensajeError += " | " + ex.Message;
        //        }
        //        ModelState.AddModelError("", "Error al crear el empleado: " + mensajeError);
        //    }

        //    // Recargar las listas desplegables
        //    ViewBag.Medicos = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre", empleado.Id_Medico);
        //    ViewBag.Estados = new SelectList(BaseDatos.Estado, "Id_Estado", "Descripcion", empleado.Id_Estado);
        //    ViewBag.Usuarios = new SelectList(BaseDatos.Users, "Id", "UserName", empleado.Id_Usuario);
        //    return View(empleado);
        //}




        //-----------------------------------------------------------------Controller Editar -------------------------------------------------------------------------------------

        ////controller Editar
        //[HttpGet]
        //public ActionResult EditarEmpleado(int? id)
        //{
        //    if (id == null) return RedirectToAction("Empleados");

        //    var empleado = BaseDatos.Empleado.Find(id);
        //    if (empleado == null) return RedirectToAction("Empleados");

        //    ViewBag.Medicos = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre", empleado.Id_Medico);
        //    ViewBag.Estados = new SelectList(BaseDatos.Estado, "Id_Estado", "Descripcion", empleado.Id_Estado);
        //    ViewBag.Usuarios = new SelectList(BaseDatos.Users, "Id", "UserName", empleado.Id_Usuario);

        //    return View(empleado);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditarEmpleado(Empleado empleado)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        BaseDatos.Entry(empleado).State = EntityState.Modified;
        //        BaseDatos.SaveChanges();
        //        return RedirectToAction("VistaEmpleados");
        //    }

        //    ViewBag.Medicos = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre", empleado.Id_Medico);
        //    ViewBag.Estados = new SelectList(BaseDatos.Estado, "Id_Estado", "Descripcion", empleado.Id_Estado);
        //    ViewBag.Usuarios = new SelectList(BaseDatos.Users, "Id", "UserName", empleado.Id_Usuario);

        //    return View(empleado);

        //}

        //private void ActualizarEmpleado(Empleado empleadoDb, Empleado empleado)
        //{
        //    empleadoDb.Id_Medico = empleado.Id_Medico;
        //    empleadoDb.Id_Estado = empleado.Id_Estado;
        //    empleadoDb.Id_Usuario = empleado.Id_Usuario;
        //    empleadoDb.Comentarios = empleado.Comentarios;
        //    empleadoDb.Nombre = empleado.Nombre;
        //    empleadoDb.Apellido = empleado.Apellido;
        //    empleadoDb.Cedula = empleado.Cedula;
        //    empleadoDb.Correo = empleado.Correo;
        //    empleadoDb.Jornada = empleado.Jornada;
        //    empleadoDb.Fecha_vencimiento_contrato = empleado.Fecha_vencimiento_contrato;
        //    empleadoDb.FechaInicio = empleado.FechaInicio;
        //    empleadoDb.FechaFin = empleado.FechaFin;
        //    empleadoDb.Fecha_actualizacion = DateTime.Now;
        //    empleadoDb.Administrador_modificacion = "Sistema";
        //}

        //private void PrepararViewBags(Empleado empleado)
        //{
        //    ViewBag.Medicos = new SelectList(BaseDatos.Medico, "Id_Medico", "Nombre", empleado.Id_Medico);
        //    ViewBag.Estados = new SelectList(BaseDatos.Estado, "Id_Estado", "Descripcion", empleado.Id_Estado);
        //    ViewBag.Usuarios = new SelectList(BaseDatos.Users, "Id", "UserName", empleado.Id_Usuario);
        //}

        //private string ObtenerMensajeError(Exception ex)
        //{
        //    var mensajeError = ex.Message;
        //    while (ex.InnerException != null)
        //    {
        //        ex = ex.InnerException;
        //        mensajeError += " | " + ex.Message;
        //    }
        //    return mensajeError;
        //}







        //-----------------------------------------------------------------Controller Desactivar -------------------------------------------------------------------------------------

        ////preguntar
        //public ActionResult DesactivarPerfil()
        //{
        //    return View();
        //}



    }

}