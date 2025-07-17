namespace ProyectoClinica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CitaModificacion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cita", "Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Cita", "Id");
            AddForeignKey("dbo.Cita", "Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cita", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Cita", new[] { "Id" });
            DropColumn("dbo.Cita", "Id");
        }
    }
}
