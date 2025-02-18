namespace ProyectoClinica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiosNotaMedico : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Nota_Medico",
                c => new
                    {
                        Id_Nota_Medico = c.Int(nullable: false, identity: true),
                        Observacion = c.String(maxLength: 255),
                        Recomendacion = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id_Nota_Medico);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Nota_Medico");
        }
    }
}
