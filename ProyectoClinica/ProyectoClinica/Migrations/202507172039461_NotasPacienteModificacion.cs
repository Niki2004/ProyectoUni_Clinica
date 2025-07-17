namespace ProyectoClinica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotasPacienteModificacion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Nota_Paciente", "Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Nota_Paciente", "Id");
            AddForeignKey("dbo.Nota_Paciente", "Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Nota_Paciente", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Nota_Paciente", new[] { "Id" });
            DropColumn("dbo.Nota_Paciente", "Id");
        }
    }
}
