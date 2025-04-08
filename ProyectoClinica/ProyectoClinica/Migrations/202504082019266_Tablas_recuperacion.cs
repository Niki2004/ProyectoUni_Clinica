namespace ProyectoClinica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tablas_recuperacion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Recuperacion_Contra",
                c => new
                    {
                        Id_Recuperacion = c.Int(nullable: false, identity: true),
                        Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id_Recuperacion)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Recuperacion_Historial",
                c => new
                    {
                        Id_Historial = c.Int(nullable: false, identity: true),
                        Recuperacion = c.String(nullable: false, maxLength: 100),
                        Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id_Historial)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recuperacion_Historial", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Recuperacion_Contra", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Recuperacion_Historial", new[] { "Id" });
            DropIndex("dbo.Recuperacion_Contra", new[] { "Id" });
            DropTable("dbo.Recuperacion_Historial");
            DropTable("dbo.Recuperacion_Contra");
        }
    }
}
