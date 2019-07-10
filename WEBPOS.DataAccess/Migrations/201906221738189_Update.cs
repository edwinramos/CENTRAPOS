namespace WEBPOS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.srBusinessPartner", "IsVoided", c => c.Boolean(nullable: false));
            AddColumn("dbo.srItem", "ShortageLevel", c => c.Int(nullable: false));
            DropColumn("dbo.srItem", "ShotageLevel");
        }
        
        public override void Down()
        {
            AddColumn("dbo.srItem", "ShotageLevel", c => c.Int(nullable: false));
            DropColumn("dbo.srItem", "ShortageLevel");
            DropColumn("dbo.srBusinessPartner", "IsVoided");
        }
    }
}
