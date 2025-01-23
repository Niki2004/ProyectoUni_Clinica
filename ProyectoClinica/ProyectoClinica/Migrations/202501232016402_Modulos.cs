namespace ProyectoClinica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modulos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrativa",
                c => new
                    {
                        Id_Admin = c.Int(nullable: false, identity: true),
                        Id_Estado = c.Int(nullable: false),
                        Notif_Intentos_Fallidos = c.Boolean(nullable: false),
                        Notif_Acceso_Datos_Sensibles = c.Boolean(nullable: false),
                        Ultimos_Intentos_Login = c.String(),
                        Log_Eliminacion = c.String(),
                        Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id_Admin)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Estado", t => t.Id_Estado, cascadeDelete: true)
                .Index(t => t.Id_Estado)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(),
                        Apellido = c.String(),
                        Edad_Paciente = c.Int(nullable: false),
                        Direccion = c.String(),
                        Cedula = c.String(),
                        Imagen = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Medico",
                c => new
                    {
                        Id_Medico = c.Int(nullable: false, identity: true),
                        Id_Especialidad = c.Int(nullable: false),
                        Id_Estado_Asistencia = c.Int(nullable: false),
                        Nota_medico = c.String(maxLength: 255),
                        Horario_inicio = c.Time(nullable: false, precision: 7),
                        Horario_fin = c.Time(nullable: false, precision: 7),
                        Filtro_historial = c.String(maxLength: 50),
                        Notificacion_enviada = c.String(maxLength: 255),
                        Total_Procedimiento = c.Int(),
                        Motivo_cancelacion = c.String(maxLength: 255),
                        Observaciones_Pacientes = c.String(maxLength: 225),
                        Receta_aprobada = c.String(maxLength: 255),
                        Detalle_receta = c.String(maxLength: 255),
                        Motivo_modificacion = c.String(maxLength: 255),
                        Fecha_modificacion = c.DateTime(nullable: false),
                        Fecha_creacion = c.DateTime(nullable: false),
                        Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id_Medico)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Especialidad", t => t.Id_Especialidad, cascadeDelete: true)
                .ForeignKey("dbo.Estado_Asistencia", t => t.Id_Estado_Asistencia, cascadeDelete: true)
                .Index(t => t.Id_Especialidad)
                .Index(t => t.Id_Estado_Asistencia)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Cita",
                c => new
                    {
                        Id_Cita = c.Int(nullable: false, identity: true),
                        Id_Medico = c.Int(nullable: false),
                        Id_Atencion_Cliente = c.Int(nullable: false),
                        Hora_cita = c.Time(nullable: false, precision: 7),
                        Duracion_Tratamiento = c.String(maxLength: 255),
                        Total_Citas = c.Int(nullable: false),
                        Cantidad_Pacientes = c.Int(nullable: false),
                        Fecha_Cita = c.DateTime(nullable: false),
                        Modalidad = c.String(maxLength: 50),
                        Fecha_solicitud = c.DateTime(nullable: false),
                        Fecha_notificacion = c.DateTime(nullable: false),
                        Hora_notificacion = c.Time(nullable: false, precision: 7),
                        Tipo_Tratamiento = c.String(maxLength: 255),
                        Tipo_Consulta = c.String(maxLength: 255),
                        Receta_solicitada = c.String(maxLength: 255),
                        Citas_Asistidas = c.Int(nullable: false),
                        Citas_No_Asistidas = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Cita)
                .ForeignKey("dbo.Atencion_Cliente", t => t.Id_Atencion_Cliente, cascadeDelete: true)
                .ForeignKey("dbo.Medico", t => t.Id_Medico, cascadeDelete: true)
                .Index(t => t.Id_Medico)
                .Index(t => t.Id_Atencion_Cliente);
            
            CreateTable(
                "dbo.Atencion_Cliente",
                c => new
                    {
                        Id_Atencion_Cliente = c.Int(nullable: false, identity: true),
                        Id_Clasificacion = c.Int(nullable: false),
                        Id_Prioridad = c.Int(nullable: false),
                        Id_Tipo_Servicio = c.Int(nullable: false),
                        Promedio_servicio = c.Int(nullable: false),
                        Salud_Evaluada = c.String(maxLength: 225),
                        Comentarios_Paciente = c.String(maxLength: 225),
                        Porcentaje_Pacientes_Satisfechos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Fechas_Comentario = c.DateTime(nullable: false),
                        Frecuencia_Comentarios = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id_Atencion_Cliente)
                .ForeignKey("dbo.Clasificacion_Problema", t => t.Id_Clasificacion, cascadeDelete: true)
                .ForeignKey("dbo.Prioridad_Mejora", t => t.Id_Prioridad, cascadeDelete: true)
                .ForeignKey("dbo.Tipo_Servicio", t => t.Id_Tipo_Servicio, cascadeDelete: true)
                .Index(t => t.Id_Clasificacion)
                .Index(t => t.Id_Prioridad)
                .Index(t => t.Id_Tipo_Servicio);
            
            CreateTable(
                "dbo.Clasificacion_Problema",
                c => new
                    {
                        Id_Clasificacion = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id_Clasificacion);
            
            CreateTable(
                "dbo.Prioridad_Mejora",
                c => new
                    {
                        Id_Prioridad = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id_Prioridad);
            
            CreateTable(
                "dbo.Tipo_Servicio",
                c => new
                    {
                        Id_Tipo_Servicio = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id_Tipo_Servicio);
            
            CreateTable(
                "dbo.Paciente_Cita",
                c => new
                    {
                        Id_Paciente_Cita = c.Int(nullable: false, identity: true),
                        Id_Paciente = c.Int(nullable: false),
                        Id_Cita = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Paciente_Cita)
                .ForeignKey("dbo.Cita", t => t.Id_Cita, cascadeDelete: true)
                .ForeignKey("dbo.Paciente", t => t.Id_Paciente, cascadeDelete: true)
                .Index(t => t.Id_Paciente)
                .Index(t => t.Id_Cita);
            
            CreateTable(
                "dbo.Paciente",
                c => new
                    {
                        Id_Paciente = c.Int(nullable: false, identity: true),
                        Contacto_emergencia = c.String(maxLength: 255),
                        Nota_paciente = c.String(),
                        Sintomas = c.String(maxLength: 255),
                        Nombre_Complicacion = c.String(maxLength: 255),
                        Formato_imagen = c.String(maxLength: 50),
                        Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id_Paciente)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Especialidad",
                c => new
                    {
                        Id_Especialidad = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 255),
                        Descripcion = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id_Especialidad);
            
            CreateTable(
                "dbo.Estado_Asistencia",
                c => new
                    {
                        Id_Estado_Asistencia = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id_Estado_Asistencia);
            
            CreateTable(
                "dbo.Rol_Permiso",
                c => new
                    {
                        Id_Rol = c.Int(nullable: false, identity: true),
                        Rol_Usuario = c.String(nullable: false, maxLength: 100),
                        Permisos = c.String(maxLength: 255),
                        Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id_Rol)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Estado",
                c => new
                    {
                        Id_Estado = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id_Estado);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Descripcion = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Administrativa", "Id_Estado", "dbo.Estado");
            DropForeignKey("dbo.Administrativa", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rol_Permiso", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Medico", "Id_Estado_Asistencia", "dbo.Estado_Asistencia");
            DropForeignKey("dbo.Medico", "Id_Especialidad", "dbo.Especialidad");
            DropForeignKey("dbo.Paciente_Cita", "Id_Paciente", "dbo.Paciente");
            DropForeignKey("dbo.Paciente", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Paciente_Cita", "Id_Cita", "dbo.Cita");
            DropForeignKey("dbo.Cita", "Id_Medico", "dbo.Medico");
            DropForeignKey("dbo.Atencion_Cliente", "Id_Tipo_Servicio", "dbo.Tipo_Servicio");
            DropForeignKey("dbo.Atencion_Cliente", "Id_Prioridad", "dbo.Prioridad_Mejora");
            DropForeignKey("dbo.Atencion_Cliente", "Id_Clasificacion", "dbo.Clasificacion_Problema");
            DropForeignKey("dbo.Cita", "Id_Atencion_Cliente", "dbo.Atencion_Cliente");
            DropForeignKey("dbo.Medico", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Rol_Permiso", new[] { "Id" });
            DropIndex("dbo.Paciente", new[] { "Id" });
            DropIndex("dbo.Paciente_Cita", new[] { "Id_Cita" });
            DropIndex("dbo.Paciente_Cita", new[] { "Id_Paciente" });
            DropIndex("dbo.Atencion_Cliente", new[] { "Id_Tipo_Servicio" });
            DropIndex("dbo.Atencion_Cliente", new[] { "Id_Prioridad" });
            DropIndex("dbo.Atencion_Cliente", new[] { "Id_Clasificacion" });
            DropIndex("dbo.Cita", new[] { "Id_Atencion_Cliente" });
            DropIndex("dbo.Cita", new[] { "Id_Medico" });
            DropIndex("dbo.Medico", new[] { "Id" });
            DropIndex("dbo.Medico", new[] { "Id_Estado_Asistencia" });
            DropIndex("dbo.Medico", new[] { "Id_Especialidad" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Administrativa", new[] { "Id" });
            DropIndex("dbo.Administrativa", new[] { "Id_Estado" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Estado");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Rol_Permiso");
            DropTable("dbo.Estado_Asistencia");
            DropTable("dbo.Especialidad");
            DropTable("dbo.Paciente");
            DropTable("dbo.Paciente_Cita");
            DropTable("dbo.Tipo_Servicio");
            DropTable("dbo.Prioridad_Mejora");
            DropTable("dbo.Clasificacion_Problema");
            DropTable("dbo.Atencion_Cliente");
            DropTable("dbo.Cita");
            DropTable("dbo.Medico");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Administrativa");
        }
    }
}
