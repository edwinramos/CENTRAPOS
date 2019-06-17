namespace WEBPOS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BusinessPartnerGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.srBusinessPartnerGroup",
                c => new
                    {
                        BusinessPartnerGroupCode = c.String(nullable: false, maxLength: 128),
                        BusinessPartnerGroupDescription = c.String(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.BusinessPartnerGroupCode);
            
            AddColumn("dbo.srBusinessPartner", "BusinessPartnerGroupCode", c => c.String(maxLength: 128));
            AddColumn("dbo.srItem", "IsVoided", c => c.Boolean(nullable: false));
            CreateIndex("dbo.srBusinessPartner", "BusinessPartnerGroupCode");
            AddForeignKey("dbo.srBusinessPartner", "BusinessPartnerGroupCode", "dbo.srBusinessPartnerGroup", "BusinessPartnerGroupCode");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.srBusinessPartner", "BusinessPartnerGroupCode", "dbo.srBusinessPartnerGroup");
            DropIndex("dbo.srBusinessPartner", new[] { "BusinessPartnerGroupCode" });
            DropColumn("dbo.srItem", "IsVoided");
            DropColumn("dbo.srBusinessPartner", "BusinessPartnerGroupCode");
            DropTable("dbo.srBusinessPartnerGroup");
        }
    }
}
