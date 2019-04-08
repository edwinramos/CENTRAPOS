namespace WEBPOS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class srTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.srTable", "Value", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.srTable", "Value");
        }
    }
}
