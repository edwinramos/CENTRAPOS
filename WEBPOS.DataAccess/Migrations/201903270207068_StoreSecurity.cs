namespace WEBPOS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreSecurity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.srStore", "MaxDiscAmount", c => c.Double(nullable: false));
            AddColumn("dbo.srStore", "MaxDiscPercent", c => c.Double(nullable: false));
            AddColumn("dbo.srStore", "EnableEditing", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.srStore", "EnableEditing");
            DropColumn("dbo.srStore", "MaxDiscPercent");
            DropColumn("dbo.srStore", "MaxDiscAmount");
        }
    }
}
