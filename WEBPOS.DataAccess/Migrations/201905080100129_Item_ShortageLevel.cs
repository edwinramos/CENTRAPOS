namespace WEBPOS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Item_ShortageLevel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.srItem", "ShotageLevel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.srItem", "ShotageLevel");
        }
    }
}
