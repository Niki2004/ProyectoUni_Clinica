namespace ProyectoClinica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFactura : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Factura", "Descuento_Aplicado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Factura", "Descuento");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Factura", "Descuento", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Factura", "Descuento_Aplicado");
        }
    }
}
