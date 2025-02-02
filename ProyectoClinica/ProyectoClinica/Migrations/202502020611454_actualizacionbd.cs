namespace ProyectoClinica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actualizacionbd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Metodo_Pago_Utilizado", "Monto", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Metodo_Pago_Utilizado", "Monto");
        }
    }
}
