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
        public DbSet<Atencion_Cliente> Atencion_Cliente { get; set; }
        public DbSet<Cita> Cita { get; set; }
        public DbSet<Clasificacion_Problema> Clasificacion_Problema { get; set; }
        public DbSet<Especialidad> Especialidad { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<Estado_Asistencia> Estado_Asistencia { get; set; }
        public DbSet<Medico> Medico { get; set; }
        public DbSet<Paciente> Paciente { get; set; }
        public DbSet<Paciente_Cita> Paciente_Cita { get; set; }
        public DbSet<Prioridad_Mejora> Prioridad_Mejora { get; set; }
        public DbSet<Rol_Permiso> Rol_Permiso { get; set; }
        public DbSet<Tipo_Servicio> Tipo_Servicio { get; set; }
        public DbSet<Tipo_Registro> Tipo_Registro { get; set; }
        public DbSet<Estado_Contabilidad> Estado_Contabilidad { get; set; }
        public DbSet<Tipo_Transaccion> Tipo_Transaccion { get; set; }
        public DbSet<Contabilidad> Contabilidad { get; set; }
        public DbSet<Servicio> Servicio { get; set; }
        public DbSet<Servicios_Brindados> Servicios_Brindados { get; set; }
        public DbSet<Descuento> Descuento { get; set; }
        public DbSet<Factura> Factura { get; set; }
        public DbSet<Reporte> Reporte { get; set; }
        public DbSet<Copago> Copago { get; set; }
        public DbSet<Respaldo> Respaldo { get; set; }
        public DbSet<Inventario> Inventario { get; set; }
        public DbSet<Avisos> Avisos { get; set; }
        public DbSet<Asistente_Linea> Asistente_Linea { get; set; }
        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<Auditoria_Alerta> Auditoria_Alerta { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Administrativa>().ToTable("Administrativa");
            modelBuilder.Entity<Atencion_Cliente>().ToTable("Atencion_Cliente");
            modelBuilder.Entity<Cita>().ToTable("Cita");
            modelBuilder.Entity<Clasificacion_Problema>().ToTable("Clasificacion_Problema");
            modelBuilder.Entity<Especialidad>().ToTable("Especialidad");
            modelBuilder.Entity<Estado>().ToTable("Estado");
            modelBuilder.Entity<Estado_Asistencia>().ToTable("Estado_Asistencia");
            modelBuilder.Entity<Medico>().ToTable("Medico");
            modelBuilder.Entity<Paciente>().ToTable("Paciente");
            modelBuilder.Entity<Paciente_Cita>().ToTable("Paciente_Cita");
            modelBuilder.Entity<Prioridad_Mejora>().ToTable("Prioridad_Mejora");
            modelBuilder.Entity<Rol_Permiso>().ToTable("Rol_Permiso");
            modelBuilder.Entity<Tipo_Servicio>().ToTable("Tipo_Servicio");
            modelBuilder.Entity<Tipo_Registro>().ToTable("Tipo_Registro");
            modelBuilder.Entity<Estado_Contabilidad>().ToTable("Estado_Contabilidad");
            modelBuilder.Entity<Tipo_Transaccion>().ToTable("Tipo_Transaccion");
            modelBuilder.Entity<Contabilidad>().ToTable("Contabilidad");
            modelBuilder.Entity<Servicio>().ToTable("Servicio ");
            modelBuilder.Entity<Servicios_Brindados>().ToTable("Servicios_Brindados");
            modelBuilder.Entity<Descuento>().ToTable("Descuento");
            modelBuilder.Entity<Factura>().ToTable("Factura");

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
    .HasRequired(r => r.Paciente)
    .WithMany()
    .HasForeignKey(r => r.Id_Paciente)
   .WillCascadeOnDelete(false);// Previene la cascada

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


            // Relación obligatoria entre Cita y Atencion_Cliente
            modelBuilder.Entity<Cita>()
                .HasRequired(c => c.Atencion_Cliente)
                .WithMany()
                .HasForeignKey(c => c.Id_Atencion_Cliente)
                .WillCascadeOnDelete(false);

            // Configuración para Factura y Descuento
            modelBuilder.Entity<Factura>()
    .HasRequired(f => f.Descuento_Aplicado)
    .WithMany()
    .HasForeignKey(f => f.Id_Descuento)
    .WillCascadeOnDelete(false);

            // Otras relaciones
            modelBuilder.Entity<Medico>()
                .HasRequired(m => m.Especialidad)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Medico>()
                .HasRequired(m => m.EstadoAsistencia)
                .WithMany()
                .WillCascadeOnDelete(false);

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

            modelBuilder.Entity<Paciente_Cita>()
                .HasRequired(pc => pc.Cita)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Paciente_Cita>()
                .HasRequired(pc => pc.Paciente)
                .WithMany()
                .WillCascadeOnDelete(false);

            // Configuración de tablas restantes
            modelBuilder.Entity<Copago>().ToTable("Copago");
            modelBuilder.Entity<Respaldo>().ToTable("Respaldo");
            modelBuilder.Entity<Busquedas_exportaciones>().ToTable("Busquedas_exportaciones");
            modelBuilder.Entity<Inventario>().ToTable("Inventario");
            modelBuilder.Entity<Avisos>().ToTable("Avisos");
            modelBuilder.Entity<Asistente_Linea>().ToTable("Asistente_Linea");
            modelBuilder.Entity<Empleado>().ToTable("Empleado ");
            modelBuilder.Entity<Auditoria_Alerta>().ToTable("Auditoria_Alerta");


        }
    }
}