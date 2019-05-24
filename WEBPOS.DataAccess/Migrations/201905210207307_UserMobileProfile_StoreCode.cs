namespace WEBPOS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserMobileProfile_StoreCode : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.srUserMobileProfile");
            AlterColumn("dbo.srUserMobileProfile", "StoreCode", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.srUserMobileProfile", new[] { "StoreCode", "UserCode", "MobileProfileType" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.srUserMobileProfile");
            AlterColumn("dbo.srUserMobileProfile", "StoreCode", c => c.String());
            AddPrimaryKey("dbo.srUserMobileProfile", new[] { "UserCode", "MobileProfileType" });
        }
    }
}
