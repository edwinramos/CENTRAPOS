using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CentraPos.DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "srActivityLog",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    ActivityMessage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srActivityLog", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "srBusinessPartnerContact",
                columns: table => new
                {
                    BusinessPartnerCode = table.Column<string>(nullable: false),
                    BusinessPartnerContactCode = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    ContactName = table.Column<string>(nullable: true),
                    ContactTitle = table.Column<string>(nullable: true),
                    Telephone1 = table.Column<string>(nullable: true),
                    Telephone2 = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srBusinessPartnerContact", x => new { x.BusinessPartnerCode, x.BusinessPartnerContactCode });
                });

            migrationBuilder.CreateTable(
                name: "srBusinessPartnerGroup",
                columns: table => new
                {
                    BusinessPartnerGroupCode = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    BusinessPartnerGroupDescription = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srBusinessPartnerGroup", x => x.BusinessPartnerGroupCode);
                });

            migrationBuilder.CreateTable(
                name: "srDepartment",
                columns: table => new
                {
                    DepartmentCode = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    DepartmentDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srDepartment", x => x.DepartmentCode);
                });

            migrationBuilder.CreateTable(
                name: "srPaymentType",
                columns: table => new
                {
                    PaymentTypeCode = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    PaymentTypeDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srPaymentType", x => x.PaymentTypeCode);
                });

            migrationBuilder.CreateTable(
                name: "srPosClosureDetail",
                columns: table => new
                {
                    PosClosureHeadId = table.Column<int>(nullable: false),
                    StoreCode = table.Column<string>(nullable: false),
                    StorePosCode = table.Column<string>(nullable: false),
                    TransactionNumber = table.Column<double>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    TransactionDateTime = table.Column<DateTime>(nullable: false),
                    NCF = table.Column<string>(nullable: true),
                    TotalValue = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srPosClosureDetail", x => new { x.PosClosureHeadId, x.StoreCode, x.StorePosCode, x.TransactionNumber });
                });

            migrationBuilder.CreateTable(
                name: "srPosClosureHead",
                columns: table => new
                {
                    PosClosureHeadId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    StoreCode = table.Column<string>(nullable: false),
                    StorePosCode = table.Column<string>(nullable: false),
                    UserCode = table.Column<string>(nullable: true),
                    BeginAmount = table.Column<double>(nullable: false),
                    Total = table.Column<double>(nullable: false),
                    StartDateTime = table.Column<DateTime>(nullable: false),
                    EndDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srPosClosureHead", x => x.PosClosureHeadId);
                });

            migrationBuilder.CreateTable(
                name: "srPriceList",
                columns: table => new
                {
                    PriceListCode = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    PriceListDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srPriceList", x => x.PriceListCode);
                });

            migrationBuilder.CreateTable(
                name: "srSellOrder",
                columns: table => new
                {
                    SellOrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    PriceListCode = table.Column<string>(nullable: true),
                    DocDateTime = table.Column<DateTime>(nullable: false),
                    ClientCode = table.Column<string>(nullable: false),
                    ClientDescription = table.Column<string>(nullable: true),
                    ExternalReference = table.Column<string>(nullable: true),
                    VatSum = table.Column<double>(nullable: false),
                    TotalDiscount = table.Column<double>(nullable: false),
                    DocTotal = table.Column<double>(nullable: false),
                    IsClosed = table.Column<bool>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    PaymentTypeCode = table.Column<string>(nullable: true),
                    StoreCode = table.Column<string>(nullable: true),
                    WarehouseCode = table.Column<string>(nullable: true),
                    DocNetTotal = table.Column<double>(nullable: false),
                    ClosedDateTime = table.Column<DateTime>(nullable: false),
                    StorePosCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srSellOrder", x => x.SellOrderId);
                });

            migrationBuilder.CreateTable(
                name: "srTable",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    KeyFixed = table.Column<string>(nullable: true),
                    KeyVariable = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srTable", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "srTax",
                columns: table => new
                {
                    TaxCode = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    TaxDescription = table.Column<string>(nullable: false),
                    TaxPercent = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srTax", x => x.TaxCode);
                });

            migrationBuilder.CreateTable(
                name: "srUnitMeasure",
                columns: table => new
                {
                    UnitMeasureCode = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UnitMeasureDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srUnitMeasure", x => x.UnitMeasureCode);
                });

            migrationBuilder.CreateTable(
                name: "srUser",
                columns: table => new
                {
                    UserCode = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UserType = table.Column<int>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    IsEditing = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srUser", x => x.UserCode);
                });

            migrationBuilder.CreateTable(
                name: "srUserSellOrder",
                columns: table => new
                {
                    UserCode = table.Column<string>(nullable: false),
                    SellOrderId = table.Column<int>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    UserOrderState = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srUserSellOrder", x => new { x.UserCode, x.SellOrderId });
                });

            migrationBuilder.CreateTable(
                name: "srWarehouse",
                columns: table => new
                {
                    WarehouseCode = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    WarehouseDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srWarehouse", x => x.WarehouseCode);
                });

            migrationBuilder.CreateTable(
                name: "srBusinessPartner",
                columns: table => new
                {
                    BusinessPartnerCode = table.Column<string>(nullable: false),
                    BusinessPartnerType = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    BusinessPartnerDescription = table.Column<string>(nullable: true),
                    RNC = table.Column<string>(nullable: true),
                    PriceListCode = table.Column<string>(nullable: true),
                    BusinessPartnerGroupCode = table.Column<string>(nullable: true),
                    IsVoided = table.Column<bool>(nullable: false),
                    PriceListCode1 = table.Column<string>(nullable: true),
                    BusinessPartnerGroupCode1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srBusinessPartner", x => new { x.BusinessPartnerCode, x.BusinessPartnerType });
                    table.ForeignKey(
                        name: "FK_srBusinessPartner_srBusinessPartnerGroup_BusinessPartnerGroupCode1",
                        column: x => x.BusinessPartnerGroupCode1,
                        principalTable: "srBusinessPartnerGroup",
                        principalColumn: "BusinessPartnerGroupCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_srBusinessPartner_srPriceList_PriceListCode1",
                        column: x => x.PriceListCode1,
                        principalTable: "srPriceList",
                        principalColumn: "PriceListCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "srSellOrderDetail",
                columns: table => new
                {
                    SellOrderId = table.Column<int>(nullable: false),
                    LineNumber = table.Column<int>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    IsClosed = table.Column<bool>(nullable: false),
                    ItemCode = table.Column<string>(nullable: false),
                    ItemDescription = table.Column<string>(nullable: false),
                    Barcode = table.Column<string>(nullable: true),
                    ExternalCode = table.Column<string>(nullable: true),
                    PriceBefDiscounts = table.Column<double>(nullable: false),
                    DiscountValue = table.Column<double>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    WarehouseCode = table.Column<string>(nullable: false),
                    VatCode = table.Column<string>(nullable: false),
                    VatPercent = table.Column<double>(nullable: false),
                    VatValue = table.Column<double>(nullable: false),
                    PriceAftVat = table.Column<double>(nullable: false),
                    TotalRowValue = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srSellOrderDetail", x => new { x.SellOrderId, x.LineNumber });
                    table.ForeignKey(
                        name: "FK_srSellOrderDetail_srSellOrder_SellOrderId",
                        column: x => x.SellOrderId,
                        principalTable: "srSellOrder",
                        principalColumn: "SellOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "srItem",
                columns: table => new
                {
                    ItemCode = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    ItemDescription = table.Column<string>(nullable: false),
                    TaxCode = table.Column<string>(nullable: true),
                    SupplierCode = table.Column<string>(nullable: true),
                    NetWeight = table.Column<double>(nullable: false),
                    UnitMeasureCode = table.Column<string>(nullable: true),
                    DepartmentCode = table.Column<string>(nullable: true),
                    Barcode = table.Column<string>(nullable: true),
                    IsVoided = table.Column<bool>(nullable: false),
                    ShortageLevel = table.Column<int>(nullable: false),
                    TaxCode1 = table.Column<string>(nullable: true),
                    DepartmentCode1 = table.Column<string>(nullable: true),
                    UnitMeasureCode1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srItem", x => x.ItemCode);
                    table.ForeignKey(
                        name: "FK_srItem_srDepartment_DepartmentCode1",
                        column: x => x.DepartmentCode1,
                        principalTable: "srDepartment",
                        principalColumn: "DepartmentCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_srItem_srTax_TaxCode1",
                        column: x => x.TaxCode1,
                        principalTable: "srTax",
                        principalColumn: "TaxCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_srItem_srUnitMeasure_UnitMeasureCode1",
                        column: x => x.UnitMeasureCode1,
                        principalTable: "srUnitMeasure",
                        principalColumn: "UnitMeasureCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "srUserMobileProfile",
                columns: table => new
                {
                    StoreCode = table.Column<string>(nullable: false),
                    UserCode = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    MobileProfileType = table.Column<int>(nullable: false),
                    Param1 = table.Column<string>(nullable: true),
                    Param2 = table.Column<string>(nullable: true),
                    UserCode1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srUserMobileProfile", x => new { x.StoreCode, x.UserCode });
                    table.ForeignKey(
                        name: "FK_srUserMobileProfile_srUser_UserCode1",
                        column: x => x.UserCode1,
                        principalTable: "srUser",
                        principalColumn: "UserCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "srStore",
                columns: table => new
                {
                    StoreCode = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    StoreDescription = table.Column<string>(nullable: false),
                    PriceListCode = table.Column<string>(nullable: false),
                    WarehouseCode = table.Column<string>(nullable: false),
                    Telephone = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    RNC = table.Column<string>(nullable: true),
                    NIF = table.Column<string>(nullable: true),
                    NCFSequence1 = table.Column<int>(nullable: false),
                    NCFSequence2 = table.Column<int>(nullable: false),
                    MaxDiscAmount = table.Column<double>(nullable: false),
                    MaxDiscPercent = table.Column<double>(nullable: false),
                    SequenceDueDate = table.Column<DateTime>(nullable: false),
                    PriceListCode1 = table.Column<string>(nullable: true),
                    WarehouseCode1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srStore", x => x.StoreCode);
                    table.ForeignKey(
                        name: "FK_srStore_srPriceList_PriceListCode1",
                        column: x => x.PriceListCode1,
                        principalTable: "srPriceList",
                        principalColumn: "PriceListCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_srStore_srWarehouse_WarehouseCode1",
                        column: x => x.WarehouseCode1,
                        principalTable: "srWarehouse",
                        principalColumn: "WarehouseCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "srItemWarehouse",
                columns: table => new
                {
                    WarehouseCode = table.Column<string>(nullable: false),
                    ItemCode = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    QuantityOnHand = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srItemWarehouse", x => new { x.ItemCode, x.WarehouseCode });
                    table.ForeignKey(
                        name: "FK_srItemWarehouse_srItem_ItemCode",
                        column: x => x.ItemCode,
                        principalTable: "srItem",
                        principalColumn: "ItemCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_srItemWarehouse_srWarehouse_WarehouseCode",
                        column: x => x.WarehouseCode,
                        principalTable: "srWarehouse",
                        principalColumn: "WarehouseCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "srPrice",
                columns: table => new
                {
                    PriceListCode = table.Column<string>(nullable: false),
                    ItemCode = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    SellPrice = table.Column<double>(nullable: false),
                    ItemCode1 = table.Column<string>(nullable: true),
                    PriceListCode1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srPrice", x => new { x.ItemCode, x.PriceListCode });
                    table.ForeignKey(
                        name: "FK_srPrice_srItem_ItemCode1",
                        column: x => x.ItemCode1,
                        principalTable: "srItem",
                        principalColumn: "ItemCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_srPrice_srPriceList_PriceListCode1",
                        column: x => x.PriceListCode1,
                        principalTable: "srPriceList",
                        principalColumn: "PriceListCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "srSellTransactionHead",
                columns: table => new
                {
                    StoreCode = table.Column<string>(nullable: false),
                    PosCode = table.Column<string>(nullable: false),
                    TransactionDateTime = table.Column<DateTime>(nullable: false),
                    TransactionNumber = table.Column<double>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    CustomerCode = table.Column<string>(nullable: true),
                    PriceListCode = table.Column<string>(nullable: true),
                    SellOrderId = table.Column<int>(nullable: false),
                    NCF = table.Column<string>(nullable: true),
                    TotalValue = table.Column<double>(nullable: false),
                    TotalDiscount = table.Column<double>(nullable: false),
                    IsPrinted = table.Column<bool>(nullable: false),
                    DocType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srSellTransactionHead", x => new { x.StoreCode, x.PosCode, x.TransactionDateTime, x.TransactionNumber });
                    table.ForeignKey(
                        name: "FK_srSellTransactionHead_srStore_StoreCode",
                        column: x => x.StoreCode,
                        principalTable: "srStore",
                        principalColumn: "StoreCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "srStorePos",
                columns: table => new
                {
                    StoreCode = table.Column<string>(nullable: false),
                    StorePosCode = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    StorePosDescription = table.Column<string>(nullable: false),
                    DeviceId = table.Column<string>(nullable: true),
                    DeviceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srStorePos", x => new { x.StoreCode, x.StorePosCode });
                    table.ForeignKey(
                        name: "FK_srStorePos_srStore_StoreCode",
                        column: x => x.StoreCode,
                        principalTable: "srStore",
                        principalColumn: "StoreCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "srSellTransactionPayment",
                columns: table => new
                {
                    StoreCode = table.Column<string>(nullable: false),
                    PosCode = table.Column<string>(nullable: false),
                    TransactionDateTime = table.Column<DateTime>(nullable: false),
                    TransactionNumber = table.Column<double>(nullable: false),
                    RowNumber = table.Column<long>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    PaymentTypeCode = table.Column<string>(nullable: true),
                    PaymentValue = table.Column<double>(nullable: false),
                    SellTransactionHeadStoreCode = table.Column<string>(nullable: true),
                    SellTransactionHeadPosCode = table.Column<string>(nullable: true),
                    SellTransactionHeadTransactionDateTime = table.Column<DateTime>(nullable: true),
                    SellTransactionHeadTransactionNumber = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srSellTransactionPayment", x => new { x.StoreCode, x.PosCode, x.TransactionDateTime, x.TransactionNumber, x.RowNumber });
                    table.ForeignKey(
                        name: "FK_srSellTransactionPayment_srSellTransactionHead_SellTransactionHeadStoreCode_SellTransactionHeadPosCode_SellTransactionHeadTr~",
                        columns: x => new { x.SellTransactionHeadStoreCode, x.SellTransactionHeadPosCode, x.SellTransactionHeadTransactionDateTime, x.SellTransactionHeadTransactionNumber },
                        principalTable: "srSellTransactionHead",
                        principalColumns: new[] { "StoreCode", "PosCode", "TransactionDateTime", "TransactionNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "srSellTransactionDetail",
                columns: table => new
                {
                    StoreCode = table.Column<string>(nullable: false),
                    PosCode = table.Column<string>(nullable: false),
                    TransactionDateTime = table.Column<DateTime>(nullable: false),
                    TransactionNumber = table.Column<double>(nullable: false),
                    RowNumber = table.Column<double>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUser = table.Column<string>(nullable: true),
                    ItemCode = table.Column<string>(nullable: true),
                    ItemDescription = table.Column<string>(nullable: true),
                    Barcode = table.Column<string>(nullable: true),
                    TaxCode = table.Column<string>(nullable: true),
                    TaxPercent = table.Column<double>(nullable: false),
                    BasePrice = table.Column<double>(nullable: false),
                    SellPrice = table.Column<double>(nullable: false),
                    DiscountOnItem = table.Column<double>(nullable: false),
                    DiscountType = table.Column<int>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    RowValue = table.Column<double>(nullable: false),
                    PriceListCode = table.Column<string>(nullable: true),
                    TotalValue = table.Column<double>(nullable: false),
                    IsPrinted = table.Column<bool>(nullable: false),
                    ItemCode1 = table.Column<string>(nullable: true),
                    PriceListCode1 = table.Column<string>(nullable: true),
                    StorePosStoreCode = table.Column<string>(nullable: true),
                    StorePosCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_srSellTransactionDetail", x => new { x.StoreCode, x.PosCode, x.TransactionDateTime, x.TransactionNumber, x.RowNumber });
                    table.ForeignKey(
                        name: "FK_srSellTransactionDetail_srItem_ItemCode1",
                        column: x => x.ItemCode1,
                        principalTable: "srItem",
                        principalColumn: "ItemCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_srSellTransactionDetail_srPriceList_PriceListCode1",
                        column: x => x.PriceListCode1,
                        principalTable: "srPriceList",
                        principalColumn: "PriceListCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_srSellTransactionDetail_srStore_StoreCode",
                        column: x => x.StoreCode,
                        principalTable: "srStore",
                        principalColumn: "StoreCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_srSellTransactionDetail_srStorePos_StorePosStoreCode_StorePosCode",
                        columns: x => new { x.StorePosStoreCode, x.StorePosCode },
                        principalTable: "srStorePos",
                        principalColumns: new[] { "StoreCode", "StorePosCode" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_srBusinessPartner_BusinessPartnerGroupCode1",
                table: "srBusinessPartner",
                column: "BusinessPartnerGroupCode1");

            migrationBuilder.CreateIndex(
                name: "IX_srBusinessPartner_PriceListCode1",
                table: "srBusinessPartner",
                column: "PriceListCode1");

            migrationBuilder.CreateIndex(
                name: "IX_srItem_DepartmentCode1",
                table: "srItem",
                column: "DepartmentCode1");

            migrationBuilder.CreateIndex(
                name: "IX_srItem_TaxCode1",
                table: "srItem",
                column: "TaxCode1");

            migrationBuilder.CreateIndex(
                name: "IX_srItem_UnitMeasureCode1",
                table: "srItem",
                column: "UnitMeasureCode1");

            migrationBuilder.CreateIndex(
                name: "IX_srItemWarehouse_WarehouseCode",
                table: "srItemWarehouse",
                column: "WarehouseCode");

            migrationBuilder.CreateIndex(
                name: "IX_srPrice_ItemCode1",
                table: "srPrice",
                column: "ItemCode1");

            migrationBuilder.CreateIndex(
                name: "IX_srPrice_PriceListCode1",
                table: "srPrice",
                column: "PriceListCode1");

            migrationBuilder.CreateIndex(
                name: "IX_srSellTransactionDetail_ItemCode1",
                table: "srSellTransactionDetail",
                column: "ItemCode1");

            migrationBuilder.CreateIndex(
                name: "IX_srSellTransactionDetail_PriceListCode1",
                table: "srSellTransactionDetail",
                column: "PriceListCode1");

            migrationBuilder.CreateIndex(
                name: "IX_srSellTransactionDetail_StorePosStoreCode_StorePosCode",
                table: "srSellTransactionDetail",
                columns: new[] { "StorePosStoreCode", "StorePosCode" });

            migrationBuilder.CreateIndex(
                name: "IX_srSellTransactionPayment_SellTransactionHeadStoreCode_SellTransactionHeadPosCode_SellTransactionHeadTransactionDateTime_Sell~",
                table: "srSellTransactionPayment",
                columns: new[] { "SellTransactionHeadStoreCode", "SellTransactionHeadPosCode", "SellTransactionHeadTransactionDateTime", "SellTransactionHeadTransactionNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_srStore_PriceListCode1",
                table: "srStore",
                column: "PriceListCode1");

            migrationBuilder.CreateIndex(
                name: "IX_srStore_WarehouseCode1",
                table: "srStore",
                column: "WarehouseCode1");

            migrationBuilder.CreateIndex(
                name: "IX_srUserMobileProfile_UserCode1",
                table: "srUserMobileProfile",
                column: "UserCode1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "srActivityLog");

            migrationBuilder.DropTable(
                name: "srBusinessPartner");

            migrationBuilder.DropTable(
                name: "srBusinessPartnerContact");

            migrationBuilder.DropTable(
                name: "srItemWarehouse");

            migrationBuilder.DropTable(
                name: "srPaymentType");

            migrationBuilder.DropTable(
                name: "srPosClosureDetail");

            migrationBuilder.DropTable(
                name: "srPosClosureHead");

            migrationBuilder.DropTable(
                name: "srPrice");

            migrationBuilder.DropTable(
                name: "srSellOrderDetail");

            migrationBuilder.DropTable(
                name: "srSellTransactionDetail");

            migrationBuilder.DropTable(
                name: "srSellTransactionPayment");

            migrationBuilder.DropTable(
                name: "srTable");

            migrationBuilder.DropTable(
                name: "srUserMobileProfile");

            migrationBuilder.DropTable(
                name: "srUserSellOrder");

            migrationBuilder.DropTable(
                name: "srBusinessPartnerGroup");

            migrationBuilder.DropTable(
                name: "srSellOrder");

            migrationBuilder.DropTable(
                name: "srItem");

            migrationBuilder.DropTable(
                name: "srStorePos");

            migrationBuilder.DropTable(
                name: "srSellTransactionHead");

            migrationBuilder.DropTable(
                name: "srUser");

            migrationBuilder.DropTable(
                name: "srDepartment");

            migrationBuilder.DropTable(
                name: "srTax");

            migrationBuilder.DropTable(
                name: "srUnitMeasure");

            migrationBuilder.DropTable(
                name: "srStore");

            migrationBuilder.DropTable(
                name: "srPriceList");

            migrationBuilder.DropTable(
                name: "srWarehouse");
        }
    }
}
