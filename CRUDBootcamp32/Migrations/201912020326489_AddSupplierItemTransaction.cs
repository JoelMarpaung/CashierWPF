namespace CRUDBootcamp32.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSupplierItemTransaction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_M_Item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        price = c.Int(nullable: false),
                        stock = c.Int(nullable: false),
                        Supplier_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TB_M_Supplier", t => t.Supplier_Id)
                .Index(t => t.Supplier_Id);
            
            CreateTable(
                "dbo.TB_M_Supplier",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        email = c.String(),
                        createDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TB_M_Transaction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        totalPrice = c.Int(nullable: false),
                        dateTransaction = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TB_M_Transaction_Item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        quantity = c.Int(nullable: false),
                        price = c.Int(nullable: false),
                        Item_Id = c.Int(),
                        Transaction_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TB_M_Item", t => t.Item_Id)
                .ForeignKey("dbo.TB_M_Transaction", t => t.Transaction_Id)
                .Index(t => t.Item_Id)
                .Index(t => t.Transaction_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TB_M_Transaction_Item", "Transaction_Id", "dbo.TB_M_Transaction");
            DropForeignKey("dbo.TB_M_Transaction_Item", "Item_Id", "dbo.TB_M_Item");
            DropForeignKey("dbo.TB_M_Item", "Supplier_Id", "dbo.TB_M_Supplier");
            DropIndex("dbo.TB_M_Transaction_Item", new[] { "Transaction_Id" });
            DropIndex("dbo.TB_M_Transaction_Item", new[] { "Item_Id" });
            DropIndex("dbo.TB_M_Item", new[] { "Supplier_Id" });
            DropTable("dbo.TB_M_Transaction_Item");
            DropTable("dbo.TB_M_Transaction");
            DropTable("dbo.TB_M_Supplier");
            DropTable("dbo.TB_M_Item");
        }
    }
}
