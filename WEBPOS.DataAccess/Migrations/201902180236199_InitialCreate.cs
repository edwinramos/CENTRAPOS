namespace WEBPOS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.srActivityLog",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ActivityMessage = c.String(),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.srBusinessPartnerContact",
                c => new
                    {
                        BusinessPartnerCode = c.String(nullable: false, maxLength: 128),
                        BusinessPartnerContactCode = c.String(nullable: false, maxLength: 128),
                        ContactName = c.String(),
                        ContactTitle = c.String(),
                        Telephone1 = c.String(),
                        Telephone2 = c.String(),
                        Email = c.String(),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => new { t.BusinessPartnerCode, t.BusinessPartnerContactCode });
            
            CreateTable(
                "dbo.srBusinessPartner",
                c => new
                    {
                        BusinessPartnerCode = c.String(nullable: false, maxLength: 128),
                        BusinessPartnerType = c.String(nullable: false, maxLength: 128),
                        BusinessPartnerDescription = c.String(),
                        RNC = c.String(),
                        PriceListCode = c.String(maxLength: 128),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => new { t.BusinessPartnerCode, t.BusinessPartnerType })
                .ForeignKey("dbo.srPriceList", t => t.PriceListCode)
                .Index(t => t.PriceListCode);
            
            CreateTable(
                "dbo.srPriceList",
                c => new
                    {
                        PriceListCode = c.String(nullable: false, maxLength: 128),
                        PriceListDescription = c.String(),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.PriceListCode);
            
            CreateTable(
                "dbo.srPrice",
                c => new
                    {
                        PriceListCode = c.String(nullable: false, maxLength: 128),
                        ItemCode = c.String(nullable: false, maxLength: 128),
                        SellPrice = c.Double(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => new { t.PriceListCode, t.ItemCode })
                .ForeignKey("dbo.srItem", t => t.ItemCode, cascadeDelete: true)
                .ForeignKey("dbo.srPriceList", t => t.PriceListCode, cascadeDelete: true)
                .Index(t => t.PriceListCode)
                .Index(t => t.ItemCode);
            
            CreateTable(
                "dbo.srItem",
                c => new
                    {
                        ItemCode = c.String(nullable: false, maxLength: 128),
                        ItemDescription = c.String(nullable: false),
                        TaxCode = c.String(maxLength: 128),
                        SupplierCode = c.String(),
                        NetWeight = c.Double(nullable: false),
                        UnitMeasureCode = c.String(maxLength: 128),
                        DepartmentCode = c.String(maxLength: 128),
                        Barcode = c.String(),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.ItemCode)
                .ForeignKey("dbo.srDepartment", t => t.DepartmentCode)
                .ForeignKey("dbo.srTax", t => t.TaxCode)
                .ForeignKey("dbo.srUnitMeasure", t => t.UnitMeasureCode)
                .Index(t => t.TaxCode)
                .Index(t => t.UnitMeasureCode)
                .Index(t => t.DepartmentCode);
            
            CreateTable(
                "dbo.srDepartment",
                c => new
                    {
                        DepartmentCode = c.String(nullable: false, maxLength: 128),
                        DepartmentDescription = c.String(),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.DepartmentCode);
            
            CreateTable(
                "dbo.srTax",
                c => new
                    {
                        TaxCode = c.String(nullable: false, maxLength: 128),
                        TaxDescription = c.String(nullable: false),
                        TaxPercent = c.Double(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.TaxCode);
            
            CreateTable(
                "dbo.srUnitMeasure",
                c => new
                    {
                        UnitMeasureCode = c.String(nullable: false, maxLength: 128),
                        UnitMeasureDescription = c.String(),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.UnitMeasureCode);
            
            CreateTable(
                "dbo.srItemWarehouse",
                c => new
                    {
                        WarehouseCode = c.String(nullable: false, maxLength: 128),
                        ItemCode = c.String(nullable: false, maxLength: 128),
                        QuantityOnHand = c.Double(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => new { t.WarehouseCode, t.ItemCode })
                .ForeignKey("dbo.srItem", t => t.ItemCode, cascadeDelete: true)
                .ForeignKey("dbo.srWarehouse", t => t.WarehouseCode, cascadeDelete: true)
                .Index(t => t.WarehouseCode)
                .Index(t => t.ItemCode);
            
            CreateTable(
                "dbo.srWarehouse",
                c => new
                    {
                        WarehouseCode = c.String(nullable: false, maxLength: 128),
                        WarehouseDescription = c.String(),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.WarehouseCode);
            
            CreateTable(
                "dbo.srPaymentType",
                c => new
                    {
                        PaymentTypeCode = c.String(nullable: false, maxLength: 128),
                        PaymentTypeDescription = c.String(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.PaymentTypeCode);
            
            CreateTable(
                "dbo.srSellOrderDetail",
                c => new
                    {
                        SellOrderId = c.Int(nullable: false),
                        LineNumber = c.Int(nullable: false),
                        IsClosed = c.Boolean(nullable: false),
                        ItemCode = c.String(nullable: false),
                        ItemDescription = c.String(nullable: false),
                        Barcode = c.String(),
                        ExternalCode = c.String(),
                        PriceBefDiscounts = c.Double(nullable: false),
                        DiscountValue = c.Double(nullable: false),
                        Quantity = c.Double(nullable: false),
                        Price = c.Double(nullable: false),
                        WarehouseCode = c.String(nullable: false),
                        VatCode = c.String(nullable: false),
                        VatPercent = c.Double(nullable: false),
                        VatValue = c.Double(nullable: false),
                        PriceAftVat = c.Double(nullable: false),
                        TotalRowValue = c.Double(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => new { t.SellOrderId, t.LineNumber })
                .ForeignKey("dbo.srSellOrder", t => t.SellOrderId, cascadeDelete: true)
                .Index(t => t.SellOrderId);
            
            CreateTable(
                "dbo.srSellOrder",
                c => new
                    {
                        SellOrderId = c.Int(nullable: false),
                        PriceListCode = c.String(),
                        DocDateTime = c.DateTime(nullable: false),
                        ClientCode = c.String(nullable: false),
                        ClientDescription = c.String(),
                        ExternalReference = c.String(),
                        VatSum = c.Double(nullable: false),
                        TotalDiscount = c.Double(nullable: false),
                        DocTotal = c.Double(nullable: false),
                        IsClosed = c.Boolean(nullable: false),
                        Comments = c.String(),
                        PaymentTypeCode = c.String(),
                        StoreCode = c.String(),
                        WarehouseCode = c.String(),
                        DocNetTotal = c.Double(nullable: false),
                        ClosedDateTime = c.DateTime(nullable: false),
                        StorePosCode = c.String(),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.SellOrderId);
            
            CreateTable(
                "dbo.srSellTransactionDetail",
                c => new
                    {
                        StoreCode = c.String(nullable: false, maxLength: 128),
                        PosCode = c.String(nullable: false, maxLength: 128),
                        TransactionDateTime = c.DateTime(nullable: false),
                        TransactionNumber = c.Double(nullable: false),
                        RowNumber = c.Double(nullable: false),
                        ItemCode = c.String(maxLength: 128),
                        ItemDescription = c.String(),
                        Barcode = c.String(),
                        TaxCode = c.String(),
                        TaxPercent = c.Double(nullable: false),
                        BasePrice = c.Double(nullable: false),
                        SellPrice = c.Double(nullable: false),
                        DiscountOnItem = c.Double(nullable: false),
                        Quantity = c.Double(nullable: false),
                        RowValue = c.Double(nullable: false),
                        PriceListCode = c.String(maxLength: 128),
                        TotalValue = c.Double(nullable: false),
                        IsPrinted = c.Boolean(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        StorePos_StoreCode = c.String(maxLength: 128),
                        StorePos_StorePosCode = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.StoreCode, t.PosCode, t.TransactionDateTime, t.TransactionNumber, t.RowNumber })
                .ForeignKey("dbo.srItem", t => t.ItemCode)
                .ForeignKey("dbo.srPriceList", t => t.PriceListCode)
                .ForeignKey("dbo.srStore", t => t.StoreCode, cascadeDelete: true)
                .ForeignKey("dbo.srStorePos", t => new { t.StorePos_StoreCode, t.StorePos_StorePosCode })
                .Index(t => t.StoreCode)
                .Index(t => t.ItemCode)
                .Index(t => t.PriceListCode)
                .Index(t => new { t.StorePos_StoreCode, t.StorePos_StorePosCode });
            
            CreateTable(
                "dbo.srStore",
                c => new
                    {
                        StoreCode = c.String(nullable: false, maxLength: 128),
                        StoreDescription = c.String(nullable: false),
                        PriceListCode = c.String(nullable: false, maxLength: 128),
                        WarehouseCode = c.String(nullable: false, maxLength: 128),
                        Telephone = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        RNC = c.String(),
                        NIF = c.String(),
                        NCFSequence1 = c.Int(nullable: false),
                        NCFSequence2 = c.Int(nullable: false),
                        SequenceDueDate = c.DateTime(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.StoreCode)
                .ForeignKey("dbo.srPriceList", t => t.PriceListCode, cascadeDelete: true)
                .ForeignKey("dbo.srWarehouse", t => t.WarehouseCode, cascadeDelete: true)
                .Index(t => t.PriceListCode)
                .Index(t => t.WarehouseCode);
            
            CreateTable(
                "dbo.srStorePos",
                c => new
                    {
                        StoreCode = c.String(nullable: false, maxLength: 128),
                        StorePosCode = c.String(nullable: false, maxLength: 128),
                        StorePosDescription = c.String(nullable: false),
                        DeviceId = c.String(),
                        DeviceType = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => new { t.StoreCode, t.StorePosCode })
                .ForeignKey("dbo.srStore", t => t.StoreCode, cascadeDelete: true)
                .Index(t => t.StoreCode);
            
            CreateTable(
                "dbo.srSellTransactionHead",
                c => new
                    {
                        StoreCode = c.String(nullable: false, maxLength: 128),
                        PosCode = c.String(nullable: false, maxLength: 128),
                        TransactionDateTime = c.DateTime(nullable: false),
                        TransactionNumber = c.Double(nullable: false),
                        CustomerCode = c.String(),
                        PriceListCode = c.String(),
                        SellOrderId = c.Int(nullable: false),
                        NCF = c.String(),
                        TotalValue = c.Double(nullable: false),
                        TotalDiscount = c.Double(nullable: false),
                        IsPrinted = c.Boolean(nullable: false),
                        DocType = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => new { t.StoreCode, t.PosCode, t.TransactionDateTime, t.TransactionNumber })
                .ForeignKey("dbo.srStore", t => t.StoreCode, cascadeDelete: true)
                .Index(t => t.StoreCode);
            
            CreateTable(
                "dbo.srSellTransactionPayment",
                c => new
                    {
                        StoreCode = c.String(nullable: false, maxLength: 128),
                        PosCode = c.String(nullable: false, maxLength: 128),
                        TransactionDateTime = c.DateTime(nullable: false),
                        TransactionNumber = c.Double(nullable: false),
                        RowNumber = c.Long(nullable: false),
                        PaymentTypeCode = c.String(),
                        PaymentValue = c.Double(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => new { t.StoreCode, t.PosCode, t.TransactionDateTime, t.TransactionNumber, t.RowNumber })
                .ForeignKey("dbo.srSellTransactionHead", t => new { t.StoreCode, t.PosCode, t.TransactionDateTime, t.TransactionNumber }, cascadeDelete: true)
                .Index(t => new { t.StoreCode, t.PosCode, t.TransactionDateTime, t.TransactionNumber });
            
            CreateTable(
                "dbo.srTable",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        KeyFixed = c.String(nullable: false),
                        KeyVariable = c.String(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.srUser",
                c => new
                    {
                        UserCode = c.String(nullable: false, maxLength: 128),
                        UserType = c.Int(nullable: false),
                        Password = c.String(nullable: false),
                        Name = c.String(),
                        LastName = c.String(),
                        Gender = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.UserCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.srSellTransactionPayment", new[] { "StoreCode", "PosCode", "TransactionDateTime", "TransactionNumber" }, "dbo.srSellTransactionHead");
            DropForeignKey("dbo.srSellTransactionHead", "StoreCode", "dbo.srStore");
            DropForeignKey("dbo.srSellTransactionDetail", new[] { "StorePos_StoreCode", "StorePos_StorePosCode" }, "dbo.srStorePos");
            DropForeignKey("dbo.srStorePos", "StoreCode", "dbo.srStore");
            DropForeignKey("dbo.srSellTransactionDetail", "StoreCode", "dbo.srStore");
            DropForeignKey("dbo.srStore", "WarehouseCode", "dbo.srWarehouse");
            DropForeignKey("dbo.srStore", "PriceListCode", "dbo.srPriceList");
            DropForeignKey("dbo.srSellTransactionDetail", "PriceListCode", "dbo.srPriceList");
            DropForeignKey("dbo.srSellTransactionDetail", "ItemCode", "dbo.srItem");
            DropForeignKey("dbo.srSellOrderDetail", "SellOrderId", "dbo.srSellOrder");
            DropForeignKey("dbo.srItemWarehouse", "WarehouseCode", "dbo.srWarehouse");
            DropForeignKey("dbo.srItemWarehouse", "ItemCode", "dbo.srItem");
            DropForeignKey("dbo.srBusinessPartner", "PriceListCode", "dbo.srPriceList");
            DropForeignKey("dbo.srPrice", "PriceListCode", "dbo.srPriceList");
            DropForeignKey("dbo.srPrice", "ItemCode", "dbo.srItem");
            DropForeignKey("dbo.srItem", "UnitMeasureCode", "dbo.srUnitMeasure");
            DropForeignKey("dbo.srItem", "TaxCode", "dbo.srTax");
            DropForeignKey("dbo.srItem", "DepartmentCode", "dbo.srDepartment");
            DropIndex("dbo.srSellTransactionPayment", new[] { "StoreCode", "PosCode", "TransactionDateTime", "TransactionNumber" });
            DropIndex("dbo.srSellTransactionHead", new[] { "StoreCode" });
            DropIndex("dbo.srStorePos", new[] { "StoreCode" });
            DropIndex("dbo.srStore", new[] { "WarehouseCode" });
            DropIndex("dbo.srStore", new[] { "PriceListCode" });
            DropIndex("dbo.srSellTransactionDetail", new[] { "StorePos_StoreCode", "StorePos_StorePosCode" });
            DropIndex("dbo.srSellTransactionDetail", new[] { "PriceListCode" });
            DropIndex("dbo.srSellTransactionDetail", new[] { "ItemCode" });
            DropIndex("dbo.srSellTransactionDetail", new[] { "StoreCode" });
            DropIndex("dbo.srSellOrderDetail", new[] { "SellOrderId" });
            DropIndex("dbo.srItemWarehouse", new[] { "ItemCode" });
            DropIndex("dbo.srItemWarehouse", new[] { "WarehouseCode" });
            DropIndex("dbo.srItem", new[] { "DepartmentCode" });
            DropIndex("dbo.srItem", new[] { "UnitMeasureCode" });
            DropIndex("dbo.srItem", new[] { "TaxCode" });
            DropIndex("dbo.srPrice", new[] { "ItemCode" });
            DropIndex("dbo.srPrice", new[] { "PriceListCode" });
            DropIndex("dbo.srBusinessPartner", new[] { "PriceListCode" });
            DropTable("dbo.srUser");
            DropTable("dbo.srTable");
            DropTable("dbo.srSellTransactionPayment");
            DropTable("dbo.srSellTransactionHead");
            DropTable("dbo.srStorePos");
            DropTable("dbo.srStore");
            DropTable("dbo.srSellTransactionDetail");
            DropTable("dbo.srSellOrder");
            DropTable("dbo.srSellOrderDetail");
            DropTable("dbo.srPaymentType");
            DropTable("dbo.srWarehouse");
            DropTable("dbo.srItemWarehouse");
            DropTable("dbo.srUnitMeasure");
            DropTable("dbo.srTax");
            DropTable("dbo.srDepartment");
            DropTable("dbo.srItem");
            DropTable("dbo.srPrice");
            DropTable("dbo.srPriceList");
            DropTable("dbo.srBusinessPartner");
            DropTable("dbo.srBusinessPartnerContact");
            DropTable("dbo.srActivityLog");
        }
    }
}
