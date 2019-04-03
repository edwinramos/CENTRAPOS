namespace WEBPOS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PosClosure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.srPosClosureDetail",
                c => new
                    {
                        PosClosureHeadId = c.Int(nullable: false),
                        StoreCode = c.String(nullable: false, maxLength: 128),
                        StorePosCode = c.String(nullable: false, maxLength: 128),
                        TransactionNumber = c.Double(nullable: false),
                        TransactionDateTime = c.DateTime(nullable: false),
                        NCF = c.String(),
                        TotalValue = c.Double(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => new { t.PosClosureHeadId, t.StoreCode, t.StorePosCode });
            
            CreateTable(
                "dbo.srPosClosureHead",
                c => new
                    {
                        PosClosureHeadId = c.Int(nullable: false, identity: true),
                        StoreCode = c.String(nullable: false),
                        StorePosCode = c.String(nullable: false),
                        UserCode = c.String(),
                        BeginAmount = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.PosClosureHeadId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.srPosClosureHead");
            DropTable("dbo.srPosClosureDetail");
        }
    }
}
