namespace WEBPOS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserMobileProfile_Update : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.srUserMobileProfile");
            AddPrimaryKey("dbo.srUserMobileProfile", new[] { "StoreCode", "UserCode" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.srUserMobileProfile");
            AddPrimaryKey("dbo.srUserMobileProfile", new[] { "StoreCode", "UserCode", "MobileProfileType" });
        }
    }
}
