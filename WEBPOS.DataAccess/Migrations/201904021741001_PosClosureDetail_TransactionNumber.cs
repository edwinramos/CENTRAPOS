namespace WEBPOS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PosClosureDetail_TransactionNumber : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.srPosClosureDetail");
            AddPrimaryKey("dbo.srPosClosureDetail", new[] { "PosClosureHeadId", "StoreCode", "StorePosCode", "TransactionNumber" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.srPosClosureDetail");
            AddPrimaryKey("dbo.srPosClosureDetail", new[] { "PosClosureHeadId", "StoreCode", "StorePosCode" });
        }
    }
}
