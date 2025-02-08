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

namespace ProyectoClinica.Controllers
{
    public class EmpleadosController : Controller
    {
        //Base de datos
        private ApplicationDbContext BaseDatos = new ApplicationDbContext();

        // GET: Empleados
        //[Authorize(Roles = "Administrador")]
        [HttpGet]

        //Información de la clinica
        //public ActionResult VistaAdmin()
        //{
        //    return View();
        //}


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

        
        public ActionResult Create()
        {
            ViewBag.Id_Estado = new SelectList(BaseDatos.Estado, "Id_Estado", "Descripcion");
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Estado,Comentarios,Nombre,Apellido,Cedula,Correo,Jornada,Fecha_registro,Departamento")] Empleado empleado)
        {
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

        ////preguntar
        //public ActionResult DesactivarPerfil()
        //{
        //    return View();
        //}



    }

}