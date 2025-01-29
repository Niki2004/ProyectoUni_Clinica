namespace ProyectoClinica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cambios : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Medico", "Nombre", c => c.String(maxLength: 255));
            AddColumn("dbo.Paciente", "Nombre", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Paciente", "Nombre");
            DropColumn("dbo.Medico", "Nombre");
        }
    }
}
