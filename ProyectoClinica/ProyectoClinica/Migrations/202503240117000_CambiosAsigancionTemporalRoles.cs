namespace ProyectoClinica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiosAsigancionTemporalRoles : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AsignacionRolesTemporales", "Id", "dbo.AspNetUsers");
            AddColumn("dbo.AsignacionRolesTemporales", "Id_Usuario", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.AsignacionRolesTemporales", "Id_Departamento", c => c.Int(nullable: false));
            AddColumn("dbo.AsignacionRolesTemporales", "Motivo", c => c.String());
            AddColumn("dbo.AsignacionRolesTemporales", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AsignacionRolesTemporales", "Id_Usuario");
            CreateIndex("dbo.AsignacionRolesTemporales", "Id_Departamento");
            CreateIndex("dbo.AsignacionRolesTemporales", "ApplicationUser_Id");
            AddForeignKey("dbo.AsignacionRolesTemporales", "Id_Departamento", "dbo.Departamentos", "Id_Departamento", cascadeDelete: true);
            AddForeignKey("dbo.AsignacionRolesTemporales", "Id_Usuario", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AsignacionRolesTemporales", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AsignacionRolesTemporales", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AsignacionRolesTemporales", "Id_Usuario", "dbo.AspNetUsers");
            DropForeignKey("dbo.AsignacionRolesTemporales", "Id_Departamento", "dbo.Departamentos");
            DropIndex("dbo.AsignacionRolesTemporales", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AsignacionRolesTemporales", new[] { "Id_Departamento" });
            DropIndex("dbo.AsignacionRolesTemporales", new[] { "Id_Usuario" });
            DropColumn("dbo.AsignacionRolesTemporales", "ApplicationUser_Id");
            DropColumn("dbo.AsignacionRolesTemporales", "Motivo");
            DropColumn("dbo.AsignacionRolesTemporales", "Id_Departamento");
            DropColumn("dbo.AsignacionRolesTemporales", "Id_Usuario");
            AddForeignKey("dbo.AsignacionRolesTemporales", "Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
