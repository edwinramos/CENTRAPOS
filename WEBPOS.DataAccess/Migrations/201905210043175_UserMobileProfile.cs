namespace WEBPOS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserMobileProfile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.srUserMobileProfile",
                c => new
                    {
                        UserCode = c.String(nullable: false, maxLength: 128),
                        MobileProfileType = c.Int(nullable: false),
                        StoreCode = c.String(),
                        Param1 = c.String(),
                        Param2 = c.String(),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => new { t.UserCode, t.MobileProfileType })
                .ForeignKey("dbo.srUser", t => t.UserCode, cascadeDelete: true)
                .Index(t => t.UserCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.srUserMobileProfile", "UserCode", "dbo.srUser");
            DropIndex("dbo.srUserMobileProfile", new[] { "UserCode" });
            DropTable("dbo.srUserMobileProfile");
        }
    }
}
