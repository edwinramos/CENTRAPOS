namespace WEBPOS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SellTransactionDetail_Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.srSellTransactionDetail", "DiscountType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.srSellTransactionDetail", "DiscountType");
        }
    }
}
