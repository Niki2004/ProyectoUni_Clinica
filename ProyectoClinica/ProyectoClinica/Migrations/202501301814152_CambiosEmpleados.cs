namespace ProyectoClinica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiosEmpleados : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Empleado", "Nombre", c => c.String(maxLength: 255));
            AddColumn("dbo.Empleado", "Apellido", c => c.String(maxLength: 255));
            AddColumn("dbo.Empleado", "Cedula", c => c.String(maxLength: 255));
            AddColumn("dbo.Empleado", "Correo", c => c.String(maxLength: 255));
            AddColumn("dbo.Empleado", "FechaInicio", c => c.DateTime(nullable: false));
            AddColumn("dbo.Empleado", "FechaFin", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Empleado", "FechaFin");
            DropColumn("dbo.Empleado", "FechaInicio");
            DropColumn("dbo.Empleado", "Correo");
            DropColumn("dbo.Empleado", "Cedula");
            DropColumn("dbo.Empleado", "Apellido");
            DropColumn("dbo.Empleado", "Nombre");
        }
    }
}
