namespace ProyectoClinica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class plantillas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProgramacionInforme",
                c => new
                    {
                        Id_ProgramacionInforme = c.Int(nullable: false, identity: true),
                        Periodo = c.String(),
                        Destinatario = c.String(),
                        FechaHora = c.DateTime(nullable: false),
                        Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id_ProgramacionInforme)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.PlantillaInforme",
                c => new
                    {
                        Id_PlantillaInforme = c.Int(nullable: false, identity: true),
                        NombrePlantilla = c.String(),
                        CamposSeleccionados = c.String(),
                        FechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id_PlantillaInforme);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProgramacionInforme", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.ProgramacionInforme", new[] { "Id" });
            DropTable("dbo.PlantillaInforme");
            DropTable("dbo.ProgramacionInforme");
        }
    }
}
