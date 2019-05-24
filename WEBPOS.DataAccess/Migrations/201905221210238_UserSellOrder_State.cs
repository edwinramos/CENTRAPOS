namespace WEBPOS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserSellOrder_State : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.srUserSellOrder", "UserOrderState", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.srUserSellOrder", "UserOrderState");
        }
    }
}
