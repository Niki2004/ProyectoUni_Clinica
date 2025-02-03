namespace ProyectoClinica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actualizacion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Factura", "Id_ServicioBrindado", "dbo.Servicios_Brindados");
            DropIndex("dbo.Factura", new[] { "Id_ServicioBrindado" });
            RenameColumn(table: "dbo.Factura", name: "Id_ServicioBrindado", newName: "Servicios_Brindados_Id_ServicioBrindado");
            CreateTable(
                "dbo.Metodo_Pago",
                c => new
                    {
                        Id_MetodoPago = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id_MetodoPago);
            
            CreateTable(
                "dbo.Metodo_Pago_Utilizado",
                c => new
                    {
                        Id_MetodoPagoUtilizado = c.Int(nullable: false, identity: true),
                        Id_Factura = c.Int(nullable: false),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Id_MetodoPago = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_MetodoPagoUtilizado)
                .ForeignKey("dbo.Factura", t => t.Id_Factura, cascadeDelete: true)
                .ForeignKey("dbo.Metodo_Pago", t => t.Id_MetodoPago, cascadeDelete: true)
                .Index(t => t.Id_Factura)
                .Index(t => t.Id_MetodoPago);
            
            AddColumn("dbo.Factura", "Metodo_Pago_Utilizado_Id_MetodoPagoUtilizado", c => c.Int());
            AddColumn("dbo.Servicios_Brindados", "Id_Factura", c => c.Int(nullable: false));
            AlterColumn("dbo.Factura", "Servicios_Brindados_Id_ServicioBrindado", c => c.Int());
            CreateIndex("dbo.Factura", "Metodo_Pago_Utilizado_Id_MetodoPagoUtilizado");
            CreateIndex("dbo.Factura", "Servicios_Brindados_Id_ServicioBrindado");
            CreateIndex("dbo.Servicios_Brindados", "Id_Factura");
            AddForeignKey("dbo.Factura", "Metodo_Pago_Utilizado_Id_MetodoPagoUtilizado", "dbo.Metodo_Pago_Utilizado", "Id_MetodoPagoUtilizado");
            AddForeignKey("dbo.Servicios_Brindados", "Id_Factura", "dbo.Factura", "Id_Factura", cascadeDelete: true);
            AddForeignKey("dbo.Factura", "Servicios_Brindados_Id_ServicioBrindado", "dbo.Servicios_Brindados", "Id_ServicioBrindado");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Factura", "Servicios_Brindados_Id_ServicioBrindado", "dbo.Servicios_Brindados");
            DropForeignKey("dbo.Servicios_Brindados", "Id_Factura", "dbo.Factura");
            DropForeignKey("dbo.Metodo_Pago_Utilizado", "Id_MetodoPago", "dbo.Metodo_Pago");
            DropForeignKey("dbo.Factura", "Metodo_Pago_Utilizado_Id_MetodoPagoUtilizado", "dbo.Metodo_Pago_Utilizado");
            DropForeignKey("dbo.Metodo_Pago_Utilizado", "Id_Factura", "dbo.Factura");
            DropIndex("dbo.Servicios_Brindados", new[] { "Id_Factura" });
            DropIndex("dbo.Metodo_Pago_Utilizado", new[] { "Id_MetodoPago" });
            DropIndex("dbo.Metodo_Pago_Utilizado", new[] { "Id_Factura" });
            DropIndex("dbo.Factura", new[] { "Servicios_Brindados_Id_ServicioBrindado" });
            DropIndex("dbo.Factura", new[] { "Metodo_Pago_Utilizado_Id_MetodoPagoUtilizado" });
            AlterColumn("dbo.Factura", "Servicios_Brindados_Id_ServicioBrindado", c => c.Int(nullable: false));
            DropColumn("dbo.Servicios_Brindados", "Id_Factura");
            DropColumn("dbo.Factura", "Metodo_Pago_Utilizado_Id_MetodoPagoUtilizado");
            DropTable("dbo.Metodo_Pago_Utilizado");
            DropTable("dbo.Metodo_Pago");
            RenameColumn(table: "dbo.Factura", name: "Servicios_Brindados_Id_ServicioBrindado", newName: "Id_ServicioBrindado");
            CreateIndex("dbo.Factura", "Id_ServicioBrindado");
            AddForeignKey("dbo.Factura", "Id_ServicioBrindado", "dbo.Servicios_Brindados", "Id_ServicioBrindado", cascadeDelete: true);
        }
    }
}
