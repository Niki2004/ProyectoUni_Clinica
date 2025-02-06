namespace ProyectoClinica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmpleadosTablas : DbMigration
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
                        Id_Cita = c.Int(nullable: false),
                        Id_receta = c.Int(nullable: false),
                        Especialidad = c.String(nullable: false),
                        Horario_fin = c.Time(nullable: false, precision: 7),
                        Nombre = c.String(maxLength: 255),
                        Horario_inicio = c.Time(nullable: false, precision: 7),
                        Fecha_creacion = c.DateTime(nullable: false),
                        Id = c.String(maxLength: 128),
                        Estado_Asistencia_Id_Estado_Asistencia = c.Int(),
                    })
                .PrimaryKey(t => t.Id_Medico)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Receta", t => t.Id_receta, cascadeDelete: true)
                .ForeignKey("dbo.Estado_Asistencia", t => t.Estado_Asistencia_Id_Estado_Asistencia)
                .Index(t => t.Id_receta)
                .Index(t => t.Id)
                .Index(t => t.Estado_Asistencia_Id_Estado_Asistencia);
            
            CreateTable(
                "dbo.Cita",
                c => new
                    {
                        Id_Cita = c.Int(nullable: false, identity: true),
                        Id_Medico = c.Int(nullable: false),
                        Nombre_Paciente = c.String(maxLength: 255),
                        Estado_Asistencia = c.String(maxLength: 255),
                        Hora_cita = c.Time(nullable: false, precision: 7),
                        Descripcion_Complicaciones = c.String(maxLength: 255),
                        Sintomas = c.String(maxLength: 255),
                        Fecha_Cita = c.DateTime(nullable: false),
                        Modalidad = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id_Cita)
                .ForeignKey("dbo.Medico", t => t.Id_Medico, cascadeDelete: true)
                .Index(t => t.Id_Medico);
            
            CreateTable(
                "dbo.Receta",
                c => new
                    {
                        Id_receta = c.Int(nullable: false, identity: true),
                        Fecha_Creacion = c.DateTime(nullable: false),
                        Nombre_Receta = c.String(maxLength: 255),
                        Observaciones_Pacientes = c.String(maxLength: 255),
                        Duracion_Tratamiento = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id_receta);
            
            CreateTable(
                "dbo.Reportes",
                c => new
                    {
                        Id_Reporte = c.Int(nullable: false, identity: true),
                        Id_Empleado = c.Int(nullable: false),
                        Id_Cita = c.Int(nullable: false),
                        Id_Contabilidad = c.Int(nullable: false),
                        Id_SReceta = c.Int(nullable: false),
                        Id_Atencion_Cliente = c.Int(nullable: false),
                        Id_Medico = c.Int(nullable: false),
                        Id_Factura = c.Int(nullable: false),
                        Fecha_Reporte = c.DateTime(nullable: false),
                        Id = c.String(nullable: false, maxLength: 128),
                        Contabilidad_Id_Contabilidad = c.Int(),
                        Factura_Id_Factura = c.Int(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id_Reporte)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Atencion_Cliente", t => t.Id_Atencion_Cliente, cascadeDelete: true)
                .ForeignKey("dbo.Cita", t => t.Id_Cita)
                .ForeignKey("dbo.Contabilidad", t => t.Contabilidad_Id_Contabilidad)
                .ForeignKey("dbo.Contabilidad", t => t.Id_Contabilidad)
                .ForeignKey("dbo.Empleado", t => t.Id_Empleado, cascadeDelete: true)
                .ForeignKey("dbo.Factura", t => t.Factura_Id_Factura)
                .ForeignKey("dbo.Factura", t => t.Id_Factura)
                .ForeignKey("dbo.Medico", t => t.Id_Medico)
                .ForeignKey("dbo.Solicitud_Receta", t => t.Id_SReceta, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.Id_Empleado)
                .Index(t => t.Id_Cita)
                .Index(t => t.Id_Contabilidad)
                .Index(t => t.Id_SReceta)
                .Index(t => t.Id_Atencion_Cliente)
                .Index(t => t.Id_Medico)
                .Index(t => t.Id_Factura)
                .Index(t => t.Id)
                .Index(t => t.Contabilidad_Id_Contabilidad)
                .Index(t => t.Factura_Id_Factura)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Atencion_Cliente",
                c => new
                    {
                        Id_Atencion_Cliente = c.Int(nullable: false, identity: true),
                        Salud_Evaluada = c.String(maxLength: 225),
                        Comentarios_Paciente = c.String(maxLength: 225),
                        Prioridad_Mejora = c.String(maxLength: 225),
                        Fechas_Comentario = c.DateTime(nullable: false),
                        Tipo_Servicio = c.String(maxLength: 225),
                        Clasificacion_Problema = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Atencion_Cliente);
            
            CreateTable(
                "dbo.Contabilidad",
                c => new
                    {
                        Id_Contabilidad = c.Int(nullable: false, identity: true),
                        Id_Tipo_Registro = c.Int(nullable: false),
                        Id_Estado_Contabilidad = c.Int(nullable: false),
                        Id_Tipo_Transaccion = c.Int(nullable: false),
                        ClienteProveedor = c.String(),
                        Conta_pago = c.String(),
                        Estatus_pago = c.String(),
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
                .PrimaryKey(t => t.Id_Contabilidad)
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
                "dbo.Empleado",
                c => new
                    {
                        Id_Empleado = c.Int(nullable: false, identity: true),
                        Id_Estado = c.Int(nullable: false),
                        Comentarios = c.String(maxLength: 255),
                        Nombre = c.String(maxLength: 255),
                        Apellido = c.String(maxLength: 255),
                        Cedula = c.String(maxLength: 255),
                        Correo = c.String(maxLength: 255),
                        Jornada = c.String(maxLength: 50),
                        Fecha_registro = c.DateTime(nullable: false),
                        Departamento = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id_Empleado)
                .ForeignKey("dbo.Estado", t => t.Id_Estado, cascadeDelete: true)
                .Index(t => t.Id_Estado);
            
            CreateTable(
                "dbo.Documento",
                c => new
                    {
                        Id_Documento = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Informacion = c.String(),
                        Id_Empleado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Documento)
                .ForeignKey("dbo.Empleado", t => t.Id_Empleado, cascadeDelete: true)
                .Index(t => t.Id_Empleado);
            
            CreateTable(
                "dbo.Estado",
                c => new
                    {
                        Id_Estado = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id_Estado);
            
            CreateTable(
                "dbo.Evaluacion",
                c => new
                    {
                        Id_Evaluacion = c.Int(nullable: false, identity: true),
                        Fecha_Evaluacion = c.DateTime(nullable: false),
                        Calificacion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Comentarios = c.String(),
                        Id_Empleado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Evaluacion)
                .ForeignKey("dbo.Empleado", t => t.Id_Empleado, cascadeDelete: true)
                .Index(t => t.Id_Empleado);
            
            CreateTable(
                "dbo.NotificacionEmpleado",
                c => new
                    {
                        Id_Notificacion = c.Int(nullable: false, identity: true),
                        Id_Empleado = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id_Notificacion)
                .ForeignKey("dbo.Empleado", t => t.Id_Empleado, cascadeDelete: true)
                .Index(t => t.Id_Empleado);
            
            CreateTable(
                "dbo.RolAsignacion",
                c => new
                    {
                        Id_Rol = c.Int(nullable: false, identity: true),
                        Id_Empleado = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id_Rol)
                .ForeignKey("dbo.Empleado", t => t.Id_Empleado, cascadeDelete: true)
                .Index(t => t.Id_Empleado);
            
            CreateTable(
                "dbo.Factura",
                c => new
                    {
                        Id_Factura = c.Int(nullable: false, identity: true),
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
                        Metodo_Pago_Utilizado_Id_MetodoPagoUtilizado = c.Int(),
                        Servicios_Brindados_Id_ServicioBrindado = c.Int(),
                    })
                .PrimaryKey(t => t.Id_Factura)
                .ForeignKey("dbo.Descuento", t => t.Descuento_Id_Descuento)
                .ForeignKey("dbo.Descuento", t => t.Id_Descuento)
                .ForeignKey("dbo.Metodo_Pago_Utilizado", t => t.Metodo_Pago_Utilizado_Id_MetodoPagoUtilizado)
                .ForeignKey("dbo.Servicios_Brindados", t => t.Servicios_Brindados_Id_ServicioBrindado)
                .Index(t => t.Id_Descuento)
                .Index(t => t.Descuento_Id_Descuento)
                .Index(t => t.Metodo_Pago_Utilizado_Id_MetodoPagoUtilizado)
                .Index(t => t.Servicios_Brindados_Id_ServicioBrindado);
            
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
                "dbo.Solicitud_Receta",
                c => new
                    {
                        Id_SReceta = c.Int(nullable: false, identity: true),
                        receta_solicitada = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id_SReceta);
            
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
                "dbo.Asientos_Contables",
                c => new
                    {
                        Id_Asiento = c.Int(nullable: false, identity: true),
                        Numero_Asiento = c.String(),
                        Fecha_Asiento = c.DateTime(nullable: false),
                        Tipo_Asiento = c.String(),
                        Cuenta_Contable = c.String(),
                        Descripcion_Cuenta = c.String(),
                        Tipo_Movimiento = c.String(),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Concepto = c.String(),
                        Referencia = c.String(),
                        Centros_Costos = c.String(),
                        Periodo_Contable = c.String(),
                        Estado = c.String(),
                        Observaciones = c.String(),
                        Usuario_Registro = c.String(),
                        Fecha_Registro = c.DateTime(nullable: false),
                        Usuario_Modificacion = c.String(),
                        Fecha_Modificacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Asiento);
            
            CreateTable(
                "dbo.Auditoria_Alerta",
                c => new
                    {
                        Id_Auditoria = c.Int(nullable: false, identity: true),
                        Alerta_Respaldo_Fallido = c.String(maxLength: 255),
                        Usuario_Modificacion = c.String(maxLength: 100),
                        Fecha_Modificacion = c.DateTime(nullable: false),
                        Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id_Auditoria)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
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
                "dbo.Bancos",
                c => new
                    {
                        Id_Banco = c.Int(nullable: false, identity: true),
                        Id_Diario = c.Int(nullable: false),
                        Nombre_Banco = c.String(),
                        Codigo_Banco = c.String(),
                    })
                .PrimaryKey(t => t.Id_Banco)
                .ForeignKey("dbo.Diarios_Contables", t => t.Id_Diario, cascadeDelete: true)
                .Index(t => t.Id_Diario);
            
            CreateTable(
                "dbo.Diarios_Contables",
                c => new
                    {
                        Id_Diario = c.Int(nullable: false, identity: true),
                        Id_Tipo_Registro = c.Int(nullable: false),
                        Codigo_Diario = c.String(),
                        Descripcion = c.String(),
                        Activo = c.Boolean(nullable: false),
                        Fecha_Creacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Diario)
                .ForeignKey("dbo.Tipo_Registro", t => t.Id_Tipo_Registro, cascadeDelete: true)
                .Index(t => t.Id_Tipo_Registro);
            
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
                        Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id_Busqueda)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Caja_Chica",
                c => new
                    {
                        Id_Caja_Chica = c.Int(nullable: false, identity: true),
                        Id_Contabilidad = c.Int(nullable: false),
                        Numero_Comprobante = c.String(),
                        Fecha_Movimiento = c.DateTime(nullable: false),
                        Tipo_Movimiento = c.String(),
                        Concepto = c.String(),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Saldo_Anterior = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Saldo_Actual = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Beneficiario = c.String(),
                        Categoria_Gasto = c.String(),
                        Numero_Factura = c.String(),
                        Estado = c.String(),
                        Observaciones = c.String(),
                        Usuario_Registro = c.String(),
                        Fecha_Registro = c.DateTime(nullable: false),
                        Usuario_Modificacion = c.String(),
                        Fecha_Modificacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Caja_Chica)
                .ForeignKey("dbo.Contabilidad", t => t.Id_Contabilidad, cascadeDelete: true)
                .Index(t => t.Id_Contabilidad);
            
            CreateTable(
                "dbo.Conciliaciones_Bancarias",
                c => new
                    {
                        Id_Conciliacion = c.Int(nullable: false, identity: true),
                        Id_Banco = c.Int(nullable: false),
                        Id_Diario = c.Int(nullable: false),
                        Id_Tipo_Registro = c.Int(nullable: false),
                        Fecha_Conciliacion = c.DateTime(nullable: false),
                        Saldo_Contable = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Saldo_Banco = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id_Conciliacion)
                .ForeignKey("dbo.Bancos", t => t.Id_Banco, cascadeDelete: true)
                .ForeignKey("dbo.Diarios_Contables", t => t.Id_Diario)
                .ForeignKey("dbo.Tipo_Registro", t => t.Id_Tipo_Registro)
                .Index(t => t.Id_Banco)
                .Index(t => t.Id_Diario)
                .Index(t => t.Id_Tipo_Registro);
            
            CreateTable(
                "dbo.Estado_Asistencia",
                c => new
                    {
                        Id_Estado_Asistencia = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id_Estado_Asistencia);
            
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
                "dbo.Metodo_Pago",
                c => new
                    {
                        Id_MetodoPago = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id_MetodoPago);
            
            CreateTable(
                "dbo.Metodo_Pago_Utilizado",
                c => new
                    {
                        Id_MetodoPagoUtilizado = c.Int(nullable: false, identity: true),
                        Id_Factura = c.Int(nullable: false),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Id_MetodoPago = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_MetodoPagoUtilizado)
                .ForeignKey("dbo.Factura", t => t.Id_Factura, cascadeDelete: true)
                .ForeignKey("dbo.Metodo_Pago", t => t.Id_MetodoPago, cascadeDelete: true)
                .Index(t => t.Id_Factura)
                .Index(t => t.Id_MetodoPago);
            
            CreateTable(
                "dbo.Modificacion_Receta",
                c => new
                    {
                        Id_Modificacion_receta = c.Int(nullable: false, identity: true),
                        Fecha_Modificacion = c.DateTime(nullable: false),
                        motivo_modificacion = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id_Modificacion_receta);
            
            CreateTable(
                "dbo.Nota_Paciente",
                c => new
                    {
                        Id_Nota_Paciente = c.Int(nullable: false, identity: true),
                        Nota_Del_Paciente = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id_Nota_Paciente);
            
            CreateTable(
                "dbo.Notificacion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mensaje = c.String(),
                        Fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pagos",
                c => new
                    {
                        Id_Pago = c.Int(nullable: false, identity: true),
                        Id_Banco = c.Int(nullable: false),
                        Numero_Referencia = c.String(),
                        Fecha_Pago = c.DateTime(nullable: false),
                        Tipo_Pago = c.String(),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Beneficiario = c.String(),
                        Cuenta_Beneficiario = c.String(),
                        Estado_Pago = c.String(),
                        Descripcion = c.String(),
                        Observaciones = c.String(),
                        Usuario_Registro = c.String(),
                        Fecha_Registro = c.DateTime(nullable: false),
                        Usuario_Modificacion = c.String(),
                        Fecha_Modificacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Pago)
                .ForeignKey("dbo.Bancos", t => t.Id_Banco, cascadeDelete: true)
                .Index(t => t.Id_Banco);
            
            CreateTable(
                "dbo.Respaldo",
                c => new
                    {
                        Id_Respaldo = c.Int(nullable: false, identity: true),
                        Id_Paciente = c.Int(nullable: false),
                        Respaldo_configurado = c.String(),
                        Fecha_respaldo = c.DateTime(nullable: false),
                        Respaldo_exitoso = c.String(),
                        Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id_Respaldo)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
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
                "dbo.Servicios_Brindados",
                c => new
                    {
                        Id_ServicioBrindado = c.Int(nullable: false, identity: true),
                        Id_Factura = c.Int(nullable: false),
                        Id_Servicio = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_ServicioBrindado)
                .ForeignKey("dbo.Factura", t => t.Id_Factura, cascadeDelete: true)
                .ForeignKey("dbo.Servicio", t => t.Id_Servicio, cascadeDelete: true)
                .Index(t => t.Id_Factura)
                .Index(t => t.Id_Servicio);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Servicios_Brindados", "Id_Servicio", "dbo.Servicio");
            DropForeignKey("dbo.Factura", "Servicios_Brindados_Id_ServicioBrindado", "dbo.Servicios_Brindados");
            DropForeignKey("dbo.Servicios_Brindados", "Id_Factura", "dbo.Factura");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Respaldo", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Pagos", "Id_Banco", "dbo.Bancos");
            DropForeignKey("dbo.Metodo_Pago_Utilizado", "Id_MetodoPago", "dbo.Metodo_Pago");
            DropForeignKey("dbo.Factura", "Metodo_Pago_Utilizado_Id_MetodoPagoUtilizado", "dbo.Metodo_Pago_Utilizado");
            DropForeignKey("dbo.Metodo_Pago_Utilizado", "Id_Factura", "dbo.Factura");
            DropForeignKey("dbo.Medico", "Estado_Asistencia_Id_Estado_Asistencia", "dbo.Estado_Asistencia");
            DropForeignKey("dbo.Conciliaciones_Bancarias", "Id_Tipo_Registro", "dbo.Tipo_Registro");
            DropForeignKey("dbo.Conciliaciones_Bancarias", "Id_Diario", "dbo.Diarios_Contables");
            DropForeignKey("dbo.Conciliaciones_Bancarias", "Id_Banco", "dbo.Bancos");
            DropForeignKey("dbo.Caja_Chica", "Id_Contabilidad", "dbo.Contabilidad");
            DropForeignKey("dbo.Busquedas_exportaciones", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Bancos", "Id_Diario", "dbo.Diarios_Contables");
            DropForeignKey("dbo.Diarios_Contables", "Id_Tipo_Registro", "dbo.Tipo_Registro");
            DropForeignKey("dbo.Inventario", "Id_Estado", "dbo.Estado");
            DropForeignKey("dbo.Avisos", "Id_Articulo", "dbo.Inventario");
            DropForeignKey("dbo.Auditoria_Alerta", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Administrativa", "Id_Estado", "dbo.Estado");
            DropForeignKey("dbo.Administrativa", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Rol_Permiso", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reportes", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reportes", "Id_SReceta", "dbo.Solicitud_Receta");
            DropForeignKey("dbo.Reportes", "Id_Medico", "dbo.Medico");
            DropForeignKey("dbo.Reportes", "Id_Factura", "dbo.Factura");
            DropForeignKey("dbo.Reportes", "Factura_Id_Factura", "dbo.Factura");
            DropForeignKey("dbo.Factura", "Id_Descuento", "dbo.Descuento");
            DropForeignKey("dbo.Factura", "Descuento_Id_Descuento", "dbo.Descuento");
            DropForeignKey("dbo.Reportes", "Id_Empleado", "dbo.Empleado");
            DropForeignKey("dbo.RolAsignacion", "Id_Empleado", "dbo.Empleado");
            DropForeignKey("dbo.NotificacionEmpleado", "Id_Empleado", "dbo.Empleado");
            DropForeignKey("dbo.Evaluacion", "Id_Empleado", "dbo.Empleado");
            DropForeignKey("dbo.Empleado", "Id_Estado", "dbo.Estado");
            DropForeignKey("dbo.Documento", "Id_Empleado", "dbo.Empleado");
            DropForeignKey("dbo.Reportes", "Id_Contabilidad", "dbo.Contabilidad");
            DropForeignKey("dbo.Contabilidad", "Id_Tipo_Transaccion", "dbo.Tipo_Transaccion");
            DropForeignKey("dbo.Contabilidad", "Id_Tipo_Registro", "dbo.Tipo_Registro");
            DropForeignKey("dbo.Reportes", "Contabilidad_Id_Contabilidad", "dbo.Contabilidad");
            DropForeignKey("dbo.Contabilidad", "Id_Estado_Contabilidad", "dbo.Estado_Contabilidad");
            DropForeignKey("dbo.Contabilidad", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reportes", "Id_Cita", "dbo.Cita");
            DropForeignKey("dbo.Reportes", "Id_Atencion_Cliente", "dbo.Atencion_Cliente");
            DropForeignKey("dbo.Reportes", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Medico", "Id_receta", "dbo.Receta");
            DropForeignKey("dbo.Cita", "Id_Medico", "dbo.Medico");
            DropForeignKey("dbo.Medico", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Copago", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Servicios_Brindados", new[] { "Id_Servicio" });
            DropIndex("dbo.Servicios_Brindados", new[] { "Id_Factura" });
            DropIndex("dbo.Respaldo", new[] { "Id" });
            DropIndex("dbo.Pagos", new[] { "Id_Banco" });
            DropIndex("dbo.Metodo_Pago_Utilizado", new[] { "Id_MetodoPago" });
            DropIndex("dbo.Metodo_Pago_Utilizado", new[] { "Id_Factura" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Conciliaciones_Bancarias", new[] { "Id_Tipo_Registro" });
            DropIndex("dbo.Conciliaciones_Bancarias", new[] { "Id_Diario" });
            DropIndex("dbo.Conciliaciones_Bancarias", new[] { "Id_Banco" });
            DropIndex("dbo.Caja_Chica", new[] { "Id_Contabilidad" });
            DropIndex("dbo.Busquedas_exportaciones", new[] { "Id" });
            DropIndex("dbo.Diarios_Contables", new[] { "Id_Tipo_Registro" });
            DropIndex("dbo.Bancos", new[] { "Id_Diario" });
            DropIndex("dbo.Inventario", new[] { "Id_Estado" });
            DropIndex("dbo.Avisos", new[] { "Id_Articulo" });
            DropIndex("dbo.Auditoria_Alerta", new[] { "Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Rol_Permiso", new[] { "Id" });
            DropIndex("dbo.Factura", new[] { "Servicios_Brindados_Id_ServicioBrindado" });
            DropIndex("dbo.Factura", new[] { "Metodo_Pago_Utilizado_Id_MetodoPagoUtilizado" });
            DropIndex("dbo.Factura", new[] { "Descuento_Id_Descuento" });
            DropIndex("dbo.Factura", new[] { "Id_Descuento" });
            DropIndex("dbo.RolAsignacion", new[] { "Id_Empleado" });
            DropIndex("dbo.NotificacionEmpleado", new[] { "Id_Empleado" });
            DropIndex("dbo.Evaluacion", new[] { "Id_Empleado" });
            DropIndex("dbo.Documento", new[] { "Id_Empleado" });
            DropIndex("dbo.Empleado", new[] { "Id_Estado" });
            DropIndex("dbo.Contabilidad", new[] { "Id" });
            DropIndex("dbo.Contabilidad", new[] { "Id_Tipo_Transaccion" });
            DropIndex("dbo.Contabilidad", new[] { "Id_Estado_Contabilidad" });
            DropIndex("dbo.Contabilidad", new[] { "Id_Tipo_Registro" });
            DropIndex("dbo.Reportes", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Reportes", new[] { "Factura_Id_Factura" });
            DropIndex("dbo.Reportes", new[] { "Contabilidad_Id_Contabilidad" });
            DropIndex("dbo.Reportes", new[] { "Id" });
            DropIndex("dbo.Reportes", new[] { "Id_Factura" });
            DropIndex("dbo.Reportes", new[] { "Id_Medico" });
            DropIndex("dbo.Reportes", new[] { "Id_Atencion_Cliente" });
            DropIndex("dbo.Reportes", new[] { "Id_SReceta" });
            DropIndex("dbo.Reportes", new[] { "Id_Contabilidad" });
            DropIndex("dbo.Reportes", new[] { "Id_Cita" });
            DropIndex("dbo.Reportes", new[] { "Id_Empleado" });
            DropIndex("dbo.Cita", new[] { "Id_Medico" });
            DropIndex("dbo.Medico", new[] { "Estado_Asistencia_Id_Estado_Asistencia" });
            DropIndex("dbo.Medico", new[] { "Id" });
            DropIndex("dbo.Medico", new[] { "Id_receta" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Copago", new[] { "Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Administrativa", new[] { "Id" });
            DropIndex("dbo.Administrativa", new[] { "Id_Estado" });
            DropTable("dbo.Servicios_Brindados");
            DropTable("dbo.Servicio");
            DropTable("dbo.Respaldo");
            DropTable("dbo.Pagos");
            DropTable("dbo.Notificacion");
            DropTable("dbo.Nota_Paciente");
            DropTable("dbo.Modificacion_Receta");
            DropTable("dbo.Metodo_Pago_Utilizado");
            DropTable("dbo.Metodo_Pago");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Estado_Asistencia");
            DropTable("dbo.Conciliaciones_Bancarias");
            DropTable("dbo.Caja_Chica");
            DropTable("dbo.Busquedas_exportaciones");
            DropTable("dbo.Diarios_Contables");
            DropTable("dbo.Bancos");
            DropTable("dbo.Inventario");
            DropTable("dbo.Avisos");
            DropTable("dbo.Auditoria_Alerta");
            DropTable("dbo.Asientos_Contables");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Rol_Permiso");
            DropTable("dbo.Solicitud_Receta");
            DropTable("dbo.Descuento");
            DropTable("dbo.Factura");
            DropTable("dbo.RolAsignacion");
            DropTable("dbo.NotificacionEmpleado");
            DropTable("dbo.Evaluacion");
            DropTable("dbo.Estado");
            DropTable("dbo.Documento");
            DropTable("dbo.Empleado");
            DropTable("dbo.Tipo_Transaccion");
            DropTable("dbo.Tipo_Registro");
            DropTable("dbo.Estado_Contabilidad");
            DropTable("dbo.Contabilidad");
            DropTable("dbo.Atencion_Cliente");
            DropTable("dbo.Reportes");
            DropTable("dbo.Receta");
            DropTable("dbo.Cita");
            DropTable("dbo.Medico");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Copago");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Administrativa");
        }
    }
}
