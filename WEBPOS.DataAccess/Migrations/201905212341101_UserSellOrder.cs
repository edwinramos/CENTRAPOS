namespace WEBPOS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserSellOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.srUserSellOrder",
                c => new
                    {
                        UserCode = c.String(nullable: false, maxLength: 128),
                        SellOrderId = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => new { t.UserCode, t.SellOrderId });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.srUserSellOrder");
        }
    }
}
