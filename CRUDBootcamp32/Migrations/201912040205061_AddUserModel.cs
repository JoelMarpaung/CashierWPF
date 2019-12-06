namespace CRUDBootcamp32.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_M_User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        email = c.String(),
                        password = c.String(),
                        createDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Role_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TB_M_Role", t => t.Role_Id)
                .Index(t => t.Role_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TB_M_User", "Role_Id", "dbo.TB_M_Role");
            DropIndex("dbo.TB_M_User", new[] { "Role_Id" });
            DropTable("dbo.TB_M_User");
        }
    }
}
