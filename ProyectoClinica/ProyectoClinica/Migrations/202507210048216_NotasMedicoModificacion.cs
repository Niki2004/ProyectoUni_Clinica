namespace ProyectoClinica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotasMedicoModificacion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Nota_Medico", "Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Nota_Medico", "Id");
            AddForeignKey("dbo.Nota_Medico", "Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Nota_Medico", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Nota_Medico", new[] { "Id" });
            DropColumn("dbo.Nota_Medico", "Id");
        }
    }
}
