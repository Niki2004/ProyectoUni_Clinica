namespace ProyectoClinica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CopagoUpdate : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Copago", name: "Id", newName: "ApplicationUser_Id");
            RenameIndex(table: "dbo.Copago", name: "IX_Id", newName: "IX_ApplicationUser_Id");
            AddColumn("dbo.Copago", "cedula", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Copago", "cedula");
            RenameIndex(table: "dbo.Copago", name: "IX_ApplicationUser_Id", newName: "IX_Id");
            RenameColumn(table: "dbo.Copago", name: "ApplicationUser_Id", newName: "Id");
        }
    }
}
