namespace ProyectoClinica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nuevaConfi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Historial_Aplicaciones",
                c => new
                    {
                        Id_Historial = c.Int(nullable: false, identity: true),
                        Tipo_Evento = c.String(nullable: false, maxLength: 50),
                        Descripcion = c.String(nullable: false, maxLength: 500),
                        Fecha_Hora = c.DateTime(nullable: false),
                        Rol_Anterior = c.String(maxLength: 50),
                        Rol_Nuevo = c.String(maxLength: 50),
                        Area_Accedida = c.String(maxLength: 100),
                        Motivo_Bloqueo = c.String(maxLength: 500),
                        Exitoso = c.Boolean(nullable: false),
                        Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id_Historial)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Historial_Aplicaciones", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Historial_Aplicaciones", new[] { "Id" });
            DropTable("dbo.Historial_Aplicaciones");
        }
    }
}
