namespace ProyectoClinica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiosMedico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Medico", "Imagen", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Medico", "Imagen");
        }
    }
}
