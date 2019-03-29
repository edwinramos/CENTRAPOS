namespace WEBPOS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SistemLocks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.srUser", "IsEditing", c => c.Boolean(nullable: false));
            DropColumn("dbo.srStore", "EnableEditing");
        }
        
        public override void Down()
        {
            AddColumn("dbo.srStore", "EnableEditing", c => c.Boolean(nullable: false));
            DropColumn("dbo.srUser", "IsEditing");
        }
    }
}
