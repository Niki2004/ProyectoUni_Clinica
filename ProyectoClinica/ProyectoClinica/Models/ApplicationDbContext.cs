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

        }
    }
}