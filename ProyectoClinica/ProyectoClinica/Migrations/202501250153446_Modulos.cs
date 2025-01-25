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
                "dbo.Copago",
                c => new
                    {
                        Id_Copago = c.Int(nullable: false, identity: true),
                        Porcentaje = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tipo = c.String(),
                        Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id_Copago)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Empleado",
                c => new
                    {
                        Id_Empleado = c.Int(nullable: false, identity: true),
                        Id_Medico = c.Int(nullable: false),
                        Id_Estado = c.Int(nullable: false),
                        Id_Usuario = c.String(nullable: false, maxLength: 128),
                        Comentarios = c.String(maxLength: 255),
                        Historial_cambios = c.String(maxLength: 255),
                        Jornada = c.String(maxLength: 50),
                        Notificaciones = c.String(maxLength: 255),
                        Evaluaciones = c.String(maxLength: 255),
                        Fecha_vencimiento_contrato = c.DateTime(nullable: false),
                        Administrador_modificacion = c.String(maxLength: 255),
                        Fecha_registro = c.DateTime(nullable: false),
                        documentos = c.String(maxLength: 255),
                        Fecha_actualizacion = c.DateTime(nullable: false),
                        Fecha_proxima_evaluacion = c.DateTime(nullable: false),
                        Historial_capacitaciones = c.String(maxLength: 255),
                        Departamento = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id_Empleado)
                .ForeignKey("dbo.AspNetUsers", t => t.Id_Usuario, cascadeDelete: true)
                .ForeignKey("dbo.Estado", t => t.Id_Estado, cascadeDelete: true)
                .ForeignKey("dbo.Medico", t => t.Id_Medico, cascadeDelete: true)
                .Index(t => t.Id_Medico)
                .Index(t => t.Id_Estado)
                .Index(t => t.Id_Usuario);
            
            CreateTable(
                "dbo.Estado",
                c => new
                    {
                        Id_Estado = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id_Estado);
            
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
                        Total_Procedimiento = c.Int(nullable: false),
                        Motivo_cancelacion = c.String(maxLength: 255),
                        Observaciones_Pacientes = c.String(maxLength: 225),
                        Receta_aprobada = c.String(maxLength: 255),
                        Detalle_receta = c.String(maxLength: 255),
                        Motivo_modificacion = c.String(maxLength: 255),
                        Fecha_modificacion = c.DateTime(nullable: false),
                        Fecha_creacion = c.DateTime(nullable: false),
                        Id = c.String(maxLength: 128),
                        Especialidad_Id_Especialidad = c.Int(),
                        Estado_Asistencia_Id_Estado_Asistencia = c.Int(),
                    })
                .PrimaryKey(t => t.Id_Medico)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Especialidad", t => t.Especialidad_Id_Especialidad)
                .ForeignKey("dbo.Especialidad", t => t.Id_Especialidad)
                .ForeignKey("dbo.Estado_Asistencia", t => t.Estado_Asistencia_Id_Estado_Asistencia)
                .ForeignKey("dbo.Estado_Asistencia", t => t.Id_Estado_Asistencia)
                .Index(t => t.Id_Especialidad)
                .Index(t => t.Id_Estado_Asistencia)
                .Index(t => t.Id)
                .Index(t => t.Especialidad_Id_Especialidad)
                .Index(t => t.Estado_Asistencia_Id_Estado_Asistencia);
            
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
                        Atencion_Cliente_Id_Atencion_Cliente = c.Int(),
                    })
                .PrimaryKey(t => t.Id_Cita)
                .ForeignKey("dbo.Atencion_Cliente", t => t.Atencion_Cliente_Id_Atencion_Cliente)
                .ForeignKey("dbo.Atencion_Cliente", t => t.Id_Atencion_Cliente)
                .ForeignKey("dbo.Medico", t => t.Id_Medico, cascadeDelete: true)
                .Index(t => t.Id_Medico)
                .Index(t => t.Id_Atencion_Cliente)
                .Index(t => t.Atencion_Cliente_Id_Atencion_Cliente);
            
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
                        Paciente_Id_Paciente = c.Int(),
                        Cita_Id_Cita = c.Int(),
                    })
                .PrimaryKey(t => t.Id_Paciente_Cita)
                .ForeignKey("dbo.Cita", t => t.Id_Cita)
                .ForeignKey("dbo.Paciente", t => t.Paciente_Id_Paciente)
                .ForeignKey("dbo.Paciente", t => t.Id_Paciente)
                .ForeignKey("dbo.Cita", t => t.Cita_Id_Cita)
                .Index(t => t.Id_Paciente)
                .Index(t => t.Id_Cita)
                .Index(t => t.Paciente_Id_Paciente)
                .Index(t => t.Cita_Id_Cita);
            
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
                "dbo.Reportes",
                c => new
                    {
                        Id_Reporte = c.Int(nullable: false, identity: true),
                        Id_Cita = c.Int(nullable: false),
                        Id_Paciente = c.Int(nullable: false),
                        Id_Factura = c.Int(nullable: false),
                        Id_Contabilidad = c.Int(nullable: false),
                        Id_Medico = c.Int(nullable: false),
                        Tipo_Reporte = c.String(),
                        Promedio_Complicaciones = c.Int(nullable: false),
                        Costo_Promedio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Fecha_Reporte = c.DateTime(nullable: false),
                        Total_Complicaciones = c.Int(nullable: false),
                        Rango_Edad = c.String(),
                        Intervalo_Datos = c.String(),
                        Dia_Semana = c.String(),
                        Horario = c.String(),
                        Porcentaje_Exito = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Id = c.String(nullable: false, maxLength: 128),
                        Contabilidad_ID_Contabilidad = c.Int(),
                        Factura_Id_Factura = c.Int(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id_Reporte)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Cita", t => t.Id_Cita)
                .ForeignKey("dbo.Contabilidad", t => t.Contabilidad_ID_Contabilidad)
                .ForeignKey("dbo.Contabilidad", t => t.Id_Contabilidad)
                .ForeignKey("dbo.Factura", t => t.Factura_Id_Factura)
                .ForeignKey("dbo.Factura", t => t.Id_Factura)
                .ForeignKey("dbo.Medico", t => t.Id_Medico)
                .ForeignKey("dbo.Paciente", t => t.Id_Paciente)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.Id_Cita)
                .Index(t => t.Id_Paciente)
                .Index(t => t.Id_Factura)
                .Index(t => t.Id_Contabilidad)
                .Index(t => t.Id_Medico)
                .Index(t => t.Id)
                .Index(t => t.Contabilidad_ID_Contabilidad)
                .Index(t => t.Factura_Id_Factura)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Contabilidad",
                c => new
                    {
                        ID_Contabilidad = c.Int(nullable: false, identity: true),
                        Id_Tipo_Registro = c.Int(nullable: false),
                        Id_Estado_Contabilidad = c.Int(nullable: false),
                        Id_Tipo_Transaccion = c.Int(nullable: false),
                        ClienteProveedor = c.String(),
                        Fecha_Registro = c.DateTime(nullable: false),
                        Fecha_Vencimiento = c.DateTime(nullable: false),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Monto_Anticipo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Impuesto_Aplicado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descuento_Aplicado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Comentarios = c.String(),
                        Fecha_Cierre = c.DateTime(nullable: false),
                        Ingresos_Totales = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total_Pagos_Pendientes = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total_Sueldos = c.Int(nullable: false),
                        Observaciones_Ingresos = c.String(),
                        Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID_Contabilidad)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Estado_Contabilidad", t => t.Id_Estado_Contabilidad)
                .ForeignKey("dbo.Tipo_Registro", t => t.Id_Tipo_Registro)
                .ForeignKey("dbo.Tipo_Transaccion", t => t.Id_Tipo_Transaccion)
                .Index(t => t.Id_Tipo_Registro)
                .Index(t => t.Id_Estado_Contabilidad)
                .Index(t => t.Id_Tipo_Transaccion)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Estado_Contabilidad",
                c => new
                    {
                        Id_Estado_Contabilidad = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 255),
                        Descripcion = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id_Estado_Contabilidad);
            
            CreateTable(
                "dbo.Tipo_Registro",
                c => new
                    {
                        Id_Tipo_Registro = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 255),
                        Descripcion = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id_Tipo_Registro);
            
            CreateTable(
                "dbo.Tipo_Transaccion",
                c => new
                    {
                        Id_Tipo_Transaccion = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 255),
                        Descripcion = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id_Tipo_Transaccion);
            
            CreateTable(
                "dbo.Factura",
                c => new
                    {
                        Id_Factura = c.Int(nullable: false, identity: true),
                        Id_ServicioBrindado = c.Int(nullable: false),
                        Id_Descuento = c.Int(nullable: false),
                        NumeroRecibo = c.String(),
                        FechaHora = c.DateTime(nullable: false),
                        MetodoPago = c.String(),
                        CedulaCliente = c.String(),
                        NombreCliente = c.String(),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Impuesto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPagado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descuento_Id_Descuento = c.Int(),
                    })
                .PrimaryKey(t => t.Id_Factura)
                .ForeignKey("dbo.Descuento", t => t.Descuento_Id_Descuento)
                .ForeignKey("dbo.Descuento", t => t.Id_Descuento)
                .ForeignKey("dbo.Servicios_Brindados", t => t.Id_ServicioBrindado, cascadeDelete: true)
                .Index(t => t.Id_ServicioBrindado)
                .Index(t => t.Id_Descuento)
                .Index(t => t.Descuento_Id_Descuento);
            
            CreateTable(
                "dbo.Descuento",
                c => new
                    {
                        Id_Descuento = c.Int(nullable: false, identity: true),
                        Nombre_Descuento = c.String(),
                        Codigo_Descuento = c.String(),
                        Porcentaje_Descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Limite_Usos = c.Int(nullable: false),
                        Compania_Afiliada = c.String(),
                        Activo = c.Boolean(nullable: false),
                        Fecha_Creacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Descuento);
            
            CreateTable(
                "dbo.Servicios_Brindados",
                c => new
                    {
                        Id_ServicioBrindado = c.Int(nullable: false, identity: true),
                        Id_Servicio = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_ServicioBrindado)
                .ForeignKey("dbo.Servicio", t => t.Id_Servicio, cascadeDelete: true)
                .Index(t => t.Id_Servicio);
            
            CreateTable(
                "dbo.Servicio",
                c => new
                    {
                        Id_Servicio = c.Int(nullable: false, identity: true),
                        Nombre_Servicio = c.String(),
                        Precio_Servicio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Especialidad = c.String(),
                    })
                .PrimaryKey(t => t.Id_Servicio);
            
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
                "dbo.Asistente_Linea",
                c => new
                    {
                        Id_asistente = c.Int(nullable: false, identity: true),
                        Id_Cita = c.Int(nullable: false),
                        Resumen_Asistencia = c.String(),
                        Detalle = c.String(),
                    })
                .PrimaryKey(t => t.Id_asistente)
                .ForeignKey("dbo.Cita", t => t.Id_Cita, cascadeDelete: true)
                .Index(t => t.Id_Cita);
            
            CreateTable(
                "dbo.Auditoria_Alerta",
                c => new
                    {
                        Id_Auditoria = c.Int(nullable: false, identity: true),
                        Id_Paciente = c.Int(nullable: false),
                        Alerta_Respaldo_Fallido = c.String(maxLength: 255),
                        Usuario_Modificacion = c.String(maxLength: 100),
                        Fecha_Modificacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Auditoria)
                .ForeignKey("dbo.Paciente", t => t.Id_Paciente, cascadeDelete: true)
                .Index(t => t.Id_Paciente);
            
            CreateTable(
                "dbo.Avisos",
                c => new
                    {
                        Id_Aviso = c.Int(nullable: false, identity: true),
                        Id_Articulo = c.Int(nullable: false),
                        DiasAnticipacion = c.Int(nullable: false),
                        HorasAnticipacion = c.Int(nullable: false),
                        Categorias = c.String(),
                        NotificacionPush = c.Boolean(nullable: false),
                        NotificacionEmail = c.Boolean(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Aviso)
                .ForeignKey("dbo.Inventario", t => t.Id_Articulo, cascadeDelete: true)
                .Index(t => t.Id_Articulo);
            
            CreateTable(
                "dbo.Inventario",
                c => new
                    {
                        Id_Articulo = c.Int(nullable: false, identity: true),
                        Id_Estado = c.Int(nullable: false),
                        NombreArticulo = c.String(),
                        Marca = c.String(),
                        Cantidad = c.Int(nullable: false),
                        PrecioUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FechaCaducidad = c.DateTime(nullable: false),
                        TipoArticulo = c.String(),
                        FechaIngreso = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Articulo)
                .ForeignKey("dbo.Estado", t => t.Id_Estado, cascadeDelete: true)
                .Index(t => t.Id_Estado);
            
            CreateTable(
                "dbo.Respaldo",
                c => new
                    {
                        Id_Respaldo = c.Int(nullable: false, identity: true),
                        Id_Paciente = c.Int(nullable: false),
                        Respaldo_configurado = c.String(),
                        Fecha_respaldo = c.DateTime(nullable: false),
                        Respaldo_exitoso = c.String(),
                    })
                .PrimaryKey(t => t.Id_Respaldo)
                .ForeignKey("dbo.Paciente", t => t.Id_Paciente, cascadeDelete: true)
                .Index(t => t.Id_Paciente);
            
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
            
            CreateTable(
                "dbo.Busquedas_exportaciones",
                c => new
                    {
                        Id_Busqueda = c.Int(nullable: false, identity: true),
                        Id_Paciente = c.Int(nullable: false),
                        Criterios_Busqueda = c.String(),
                        Estado_Busqueda = c.String(),
                        Exportado_PDF = c.String(),
                        Fecha_Exportacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Busqueda)
                .ForeignKey("dbo.Paciente", t => t.Id_Paciente, cascadeDelete: true)
                .Index(t => t.Id_Paciente);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Busquedas_exportaciones", "Id_Paciente", "dbo.Paciente");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Respaldo", "Id_Paciente", "dbo.Paciente");
            DropForeignKey("dbo.Inventario", "Id_Estado", "dbo.Estado");
            DropForeignKey("dbo.Avisos", "Id_Articulo", "dbo.Inventario");
            DropForeignKey("dbo.Auditoria_Alerta", "Id_Paciente", "dbo.Paciente");
            DropForeignKey("dbo.Asistente_Linea", "Id_Cita", "dbo.Cita");
            DropForeignKey("dbo.Administrativa", "Id_Estado", "dbo.Estado");
            DropForeignKey("dbo.Administrativa", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rol_Permiso", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reportes", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Empleado", "Id_Medico", "dbo.Medico");
            DropForeignKey("dbo.Reportes", "Id_Paciente", "dbo.Paciente");
            DropForeignKey("dbo.Reportes", "Id_Medico", "dbo.Medico");
            DropForeignKey("dbo.Reportes", "Id_Factura", "dbo.Factura");
            DropForeignKey("dbo.Servicios_Brindados", "Id_Servicio", "dbo.Servicio");
            DropForeignKey("dbo.Factura", "Id_ServicioBrindado", "dbo.Servicios_Brindados");
            DropForeignKey("dbo.Reportes", "Factura_Id_Factura", "dbo.Factura");
            DropForeignKey("dbo.Factura", "Id_Descuento", "dbo.Descuento");
            DropForeignKey("dbo.Factura", "Descuento_Id_Descuento", "dbo.Descuento");
            DropForeignKey("dbo.Reportes", "Id_Contabilidad", "dbo.Contabilidad");
            DropForeignKey("dbo.Contabilidad", "Id_Tipo_Transaccion", "dbo.Tipo_Transaccion");
            DropForeignKey("dbo.Contabilidad", "Id_Tipo_Registro", "dbo.Tipo_Registro");
            DropForeignKey("dbo.Reportes", "Contabilidad_ID_Contabilidad", "dbo.Contabilidad");
            DropForeignKey("dbo.Contabilidad", "Id_Estado_Contabilidad", "dbo.Estado_Contabilidad");
            DropForeignKey("dbo.Contabilidad", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reportes", "Id_Cita", "dbo.Cita");
            DropForeignKey("dbo.Reportes", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Medico", "Id_Estado_Asistencia", "dbo.Estado_Asistencia");
            DropForeignKey("dbo.Medico", "Estado_Asistencia_Id_Estado_Asistencia", "dbo.Estado_Asistencia");
            DropForeignKey("dbo.Medico", "Id_Especialidad", "dbo.Especialidad");
            DropForeignKey("dbo.Medico", "Especialidad_Id_Especialidad", "dbo.Especialidad");
            DropForeignKey("dbo.Paciente_Cita", "Cita_Id_Cita", "dbo.Cita");
            DropForeignKey("dbo.Paciente_Cita", "Id_Paciente", "dbo.Paciente");
            DropForeignKey("dbo.Paciente_Cita", "Paciente_Id_Paciente", "dbo.Paciente");
            DropForeignKey("dbo.Paciente", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Paciente_Cita", "Id_Cita", "dbo.Cita");
            DropForeignKey("dbo.Cita", "Id_Medico", "dbo.Medico");
            DropForeignKey("dbo.Cita", "Id_Atencion_Cliente", "dbo.Atencion_Cliente");
            DropForeignKey("dbo.Atencion_Cliente", "Id_Tipo_Servicio", "dbo.Tipo_Servicio");
            DropForeignKey("dbo.Atencion_Cliente", "Id_Prioridad", "dbo.Prioridad_Mejora");
            DropForeignKey("dbo.Atencion_Cliente", "Id_Clasificacion", "dbo.Clasificacion_Problema");
            DropForeignKey("dbo.Cita", "Atencion_Cliente_Id_Atencion_Cliente", "dbo.Atencion_Cliente");
            DropForeignKey("dbo.Medico", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Empleado", "Id_Estado", "dbo.Estado");
            DropForeignKey("dbo.Empleado", "Id_Usuario", "dbo.AspNetUsers");
            DropForeignKey("dbo.Copago", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Busquedas_exportaciones", new[] { "Id_Paciente" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Respaldo", new[] { "Id_Paciente" });
            DropIndex("dbo.Inventario", new[] { "Id_Estado" });
            DropIndex("dbo.Avisos", new[] { "Id_Articulo" });
            DropIndex("dbo.Auditoria_Alerta", new[] { "Id_Paciente" });
            DropIndex("dbo.Asistente_Linea", new[] { "Id_Cita" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Rol_Permiso", new[] { "Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Servicios_Brindados", new[] { "Id_Servicio" });
            DropIndex("dbo.Factura", new[] { "Descuento_Id_Descuento" });
            DropIndex("dbo.Factura", new[] { "Id_Descuento" });
            DropIndex("dbo.Factura", new[] { "Id_ServicioBrindado" });
            DropIndex("dbo.Contabilidad", new[] { "Id" });
            DropIndex("dbo.Contabilidad", new[] { "Id_Tipo_Transaccion" });
            DropIndex("dbo.Contabilidad", new[] { "Id_Estado_Contabilidad" });
            DropIndex("dbo.Contabilidad", new[] { "Id_Tipo_Registro" });
            DropIndex("dbo.Reportes", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Reportes", new[] { "Factura_Id_Factura" });
            DropIndex("dbo.Reportes", new[] { "Contabilidad_ID_Contabilidad" });
            DropIndex("dbo.Reportes", new[] { "Id" });
            DropIndex("dbo.Reportes", new[] { "Id_Medico" });
            DropIndex("dbo.Reportes", new[] { "Id_Contabilidad" });
            DropIndex("dbo.Reportes", new[] { "Id_Factura" });
            DropIndex("dbo.Reportes", new[] { "Id_Paciente" });
            DropIndex("dbo.Reportes", new[] { "Id_Cita" });
            DropIndex("dbo.Paciente", new[] { "Id" });
            DropIndex("dbo.Paciente_Cita", new[] { "Cita_Id_Cita" });
            DropIndex("dbo.Paciente_Cita", new[] { "Paciente_Id_Paciente" });
            DropIndex("dbo.Paciente_Cita", new[] { "Id_Cita" });
            DropIndex("dbo.Paciente_Cita", new[] { "Id_Paciente" });
            DropIndex("dbo.Atencion_Cliente", new[] { "Id_Tipo_Servicio" });
            DropIndex("dbo.Atencion_Cliente", new[] { "Id_Prioridad" });
            DropIndex("dbo.Atencion_Cliente", new[] { "Id_Clasificacion" });
            DropIndex("dbo.Cita", new[] { "Atencion_Cliente_Id_Atencion_Cliente" });
            DropIndex("dbo.Cita", new[] { "Id_Atencion_Cliente" });
            DropIndex("dbo.Cita", new[] { "Id_Medico" });
            DropIndex("dbo.Medico", new[] { "Estado_Asistencia_Id_Estado_Asistencia" });
            DropIndex("dbo.Medico", new[] { "Especialidad_Id_Especialidad" });
            DropIndex("dbo.Medico", new[] { "Id" });
            DropIndex("dbo.Medico", new[] { "Id_Estado_Asistencia" });
            DropIndex("dbo.Medico", new[] { "Id_Especialidad" });
            DropIndex("dbo.Empleado", new[] { "Id_Usuario" });
            DropIndex("dbo.Empleado", new[] { "Id_Estado" });
            DropIndex("dbo.Empleado", new[] { "Id_Medico" });
            DropIndex("dbo.Copago", new[] { "Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Administrativa", new[] { "Id" });
            DropIndex("dbo.Administrativa", new[] { "Id_Estado" });
            DropTable("dbo.Busquedas_exportaciones");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Respaldo");
            DropTable("dbo.Inventario");
            DropTable("dbo.Avisos");
            DropTable("dbo.Auditoria_Alerta");
            DropTable("dbo.Asistente_Linea");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Rol_Permiso");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Servicio");
            DropTable("dbo.Servicios_Brindados");
            DropTable("dbo.Descuento");
            DropTable("dbo.Factura");
            DropTable("dbo.Tipo_Transaccion");
            DropTable("dbo.Tipo_Registro");
            DropTable("dbo.Estado_Contabilidad");
            DropTable("dbo.Contabilidad");
            DropTable("dbo.Reportes");
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
            DropTable("dbo.Estado");
            DropTable("dbo.Empleado");
            DropTable("dbo.Copago");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Administrativa");
        }
    }
}
