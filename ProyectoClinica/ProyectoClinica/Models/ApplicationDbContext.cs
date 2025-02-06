using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //Tabla para la base de datos *Esto hace la conexión*
        public DbSet<Administrativa> Administrativa { get; set; }
        public DbSet<Asientos_Contables> Asientos_Contables { get; set; }
        public DbSet<Atencion_Cliente> Atencion_Cliente { get; set; }
        public DbSet<Auditoria_Alerta> Auditoria_Alerta { get; set; }
        public DbSet<Avisos> Avisos { get; set; }
        public DbSet<Bancos> Bancos { get; set; }
        public DbSet<Busquedas_exportaciones> Busquedas_exportaciones { get; set; }
        public DbSet<Caja_Chica> Caja_Chica { get; set; }
        public DbSet<Cita> Cita { get; set; }
        public DbSet<Conciliaciones_Bancarias> Conciliaciones_Bancarias { get; set; }
        public DbSet<Contabilidad> Contabilidad { get; set; }
        public DbSet<Copago> Copago { get; set; }
        public DbSet<Descuento> Descuento { get; set; }
        public DbSet<Diarios_Contables> Diarios_Contables { get; set; }
        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<Estado_Asistencia> Estado_Asistencia { get; set; }
        public DbSet<Estado_Contabilidad> Estado_Contabilidad { get; set; }
        public DbSet<Factura> Factura { get; set; }
        public DbSet<Inventario> Inventario { get; set; }
        public DbSet<Medico> Medico { get; set; }
        public DbSet<Modificacion_Receta> Modificacion_Receta { get; set; }
        public DbSet<Metodo_Pago> Metodo_Pago { get; set; }
        public DbSet<Metodo_Pago_Utilizado> Metodo_Pago_Utilizado { get; set; }
        public DbSet<Nota_Paciente> Nota_Paciente { get; set; }
        public DbSet<Pagos> Pagos { get; set; }
        public DbSet<Receta> Receta { get; set; }
        public DbSet<Reporte> Reporte { get; set; }
        public DbSet<Respaldo> Respaldo { get; set; }
        public DbSet<Rol_Permiso> Rol_Permiso { get; set; }
        public DbSet<Servicio> Servicio { get; set; }
        public DbSet<Servicios_Brindados> Servicios_Brindados { get; set; }
        public DbSet<Solicitud_Receta> Solicitud_Receta { get; set; }
        public DbSet<Tipo_Registro> Tipo_Registro { get; set; }
        public DbSet<Tipo_Transaccion> Tipo_Transaccion { get; set; }
        public DbSet<Notificacion> Notificacion { get; set; }

        public DbSet<RolAsignacion> RolAsignacion { get; set; }

        public DbSet<Documento> Documento { get; set; }
        public DbSet<NotificacionEmpleado> NotificacionEmpleado { get; set; }
        public DbSet<Evaluacion> Evaluacion { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Administrativa>().ToTable("Administrativa");
            modelBuilder.Entity<Asientos_Contables>().ToTable("Asientos_Contables");
            modelBuilder.Entity<Atencion_Cliente>().ToTable("Atencion_Cliente");
            modelBuilder.Entity<Auditoria_Alerta>().ToTable("Auditoria_Alerta");
            modelBuilder.Entity<Avisos>().ToTable("Avisos");
            modelBuilder.Entity<Bancos>().ToTable("Bancos");
            modelBuilder.Entity<Busquedas_exportaciones>().ToTable("Busquedas_exportaciones");
            modelBuilder.Entity<Caja_Chica>().ToTable("Caja_Chica");
            modelBuilder.Entity<Cita>().ToTable("Cita");
            modelBuilder.Entity<Conciliaciones_Bancarias>().ToTable("Conciliaciones_Bancarias");
            modelBuilder.Entity<Contabilidad>().ToTable("Contabilidad");
            modelBuilder.Entity<Copago>().ToTable("Copago");
            modelBuilder.Entity<Descuento>().ToTable("Descuento");
            modelBuilder.Entity<Diarios_Contables>().ToTable("Diarios_Contables");
            modelBuilder.Entity<Empleado>().ToTable("Empleado");
            modelBuilder.Entity<Estado>().ToTable("Estado");
            modelBuilder.Entity<Estado_Asistencia>().ToTable("Estado_Asistencia");
            modelBuilder.Entity<Estado_Contabilidad>().ToTable("Estado_Contabilidad");
            modelBuilder.Entity<Factura>().ToTable("Factura");
            modelBuilder.Entity<Inventario>().ToTable("Inventario");
            modelBuilder.Entity<Medico>().ToTable("Medico");
            modelBuilder.Entity<Modificacion_Receta>().ToTable("Modificacion_Receta");
            modelBuilder.Entity<Nota_Paciente>().ToTable("Nota_Paciente");
            modelBuilder.Entity<Pagos>().ToTable("Pagos");
            modelBuilder.Entity<Receta>().ToTable("Receta");
            modelBuilder.Entity<Respaldo>().ToTable("Respaldo");
            modelBuilder.Entity<Rol_Permiso>().ToTable("Rol_Permiso");
            modelBuilder.Entity<Servicio>().ToTable("Servicio");
            modelBuilder.Entity<Servicios_Brindados>().ToTable("Servicios_Brindados");
            modelBuilder.Entity<Solicitud_Receta>().ToTable("Solicitud_Receta");
            modelBuilder.Entity<Tipo_Registro>().ToTable("Tipo_Registro");
            modelBuilder.Entity<Tipo_Transaccion>().ToTable("Tipo_Transaccion");
            modelBuilder.Entity<Notificacion>().ToTable("Notificacion");
            modelBuilder.Entity<RolAsignacion>().ToTable("RolAsignacion");
            modelBuilder.Entity<Documento>().ToTable("Documento");
            modelBuilder.Entity<NotificacionEmpleado>().ToTable("NotificacionEmpleado");
            modelBuilder.Entity<Evaluacion>().ToTable("Evaluacion");




            // Configuración de relaciones para evitar ciclos de cascada
            // Relación entre Reporte y Cita
            modelBuilder.Entity<Reporte>()
                .HasRequired(r => r.Cita)
                .WithMany()
                .HasForeignKey(r => r.Id_Cita)
                .WillCascadeOnDelete(false);  // Aquí se evita la cascada

            modelBuilder.Entity<Reporte>()
                .HasRequired(r => r.Medico)
                .WithMany(m => m.Reportes)
                .HasForeignKey(r => r.Id_Medico)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Reporte>()
                .HasRequired(r => r.Factura)
                .WithMany()
                .HasForeignKey(r => r.Id_Factura)
                .WillCascadeOnDelete(false);// Previene la cascada

            modelBuilder.Entity<Reporte>()
                .HasRequired(r => r.Contabilidad)
                .WithMany()
                .HasForeignKey(r => r.Id_Contabilidad)
                .WillCascadeOnDelete(false);

            // Relación entre Reporte y ApplicationUser
            modelBuilder.Entity<Reporte>()
                .HasRequired(r => r.ApplicationUser)
                .WithMany()
                .HasForeignKey(r => r.Id)
                .WillCascadeOnDelete(false); // Evitar cascada

            // Configuración para Factura y Descuento
            modelBuilder.Entity<Factura>()
                .HasRequired(f => f.Descuento_Aplicado)
                .WithMany()
                .HasForeignKey(f => f.Id_Descuento)
                .WillCascadeOnDelete(false);

            // Otras relaciones
            modelBuilder.Entity<Contabilidad>()
                .HasRequired(c => c.Tipo_Registro)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contabilidad>()
                .HasRequired(c => c.Estado_Contabilidad)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contabilidad>()
                .HasRequired(c => c.Tipo_Transaccion)
                .WithMany()
                .WillCascadeOnDelete(false);

            // ---
            modelBuilder.Entity<Conciliaciones_Bancarias>()
                .HasRequired(c => c.Diarios_Contables)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Conciliaciones_Bancarias>()
                .HasRequired(c => c.Tipo_Registro)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Conciliaciones_Bancarias>()
                .HasRequired(c => c.Bancos)
                .WithMany()
                .WillCascadeOnDelete(true);

            
        }

        public System.Data.Entity.DbSet<ProyectoClinica.Models.ApplicationRol> IdentityRoles { get; set; }
    }
}