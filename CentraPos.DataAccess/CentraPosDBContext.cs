using CentraPos.DataAccess.DataEntities;
using Microsoft.EntityFrameworkCore;
using System;

namespace CentraPos.DataAccess
{
    public class CentraPosDBContext : DbContext
    {
        public DbSet<DePriceList> PriceLists { get; set; }
        public DbSet<DeItem> Items { get; set; }        
        public DbSet<DePrice> Prices { get; set; }
        public DbSet<DeItemWarehouse> ItemWarehouses { get; set; }
        public DbSet<DeWarehouse> Warehouses { get; set; }
        public DbSet<DeTax> Taxes { get; set; }
        public DbSet<DeStore> Stores { get; set; }
        public DbSet<DePaymentType> PaymentTypes { get; set; }
        public DbSet<DeUnitMeasure> UnitMeasures { get; set; }
        public DbSet<DeDepartment> Departments { get; set; }
        public DbSet<DeBusinessPartner> BusinessPartners { get; set; }
        public DbSet<DeBusinessPartnerContact> BusinessPartnerContacts { get; set; }
        public DbSet<DeUser> Users { get; set; }
        public DbSet<DeSellTransactionHead> SellTransactionHeads { get; set; }
        public DbSet<DeSellTransactionDetail> SellTransactionDetails { get; set; }
        public DbSet<DeSellTransactionPayment> SellTransactionPayments { get; set; }
        public DbSet<DeStorePos> StorePoses { get; set; }
        public DbSet<DeActivityLog> ActivityLoges { get; set; }
        public DbSet<DeTable> Tables { get; set; }
        public DbSet<DeSellOrder> SellOrders { get; set; }
        public DbSet<DeSellOrderDetail> SellOrderDetails { get; set; }
        public DbSet<DePosClosureHead> PosClosureHeads { get; set; }
        public DbSet<DePosClosureDetail> PosClosureDetails { get; set; }
        public DbSet<DeUserMobileProfile> UserMobileProfiles { get; set; }
        public DbSet<DeUserSellOrder> UserSellOrders { get; set; }
        public DbSet<DeBusinessPartnerGroup> BusinessPartnerGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=WINDOWS-V1TELJK;Initial Catalog=CentraPosDB;Integrated Security=SSPI;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DePriceList>().HasKey(o => o.PriceListCode);
            modelBuilder.Entity<DeItem>().HasKey(o => o.ItemCode);
            modelBuilder.Entity<DePrice>().HasKey(o => new { o.ItemCode, o.PriceListCode });
            modelBuilder.Entity<DeItemWarehouse>().HasKey(o => new { o.ItemCode, o.WarehouseCode });
            modelBuilder.Entity<DeWarehouse>().HasKey(o => o.WarehouseCode);
            modelBuilder.Entity<DeTax>().HasKey(o => o.TaxCode);
            modelBuilder.Entity<DeStore>().HasKey(o => o.StoreCode);
            modelBuilder.Entity<DePaymentType>().HasKey(o => o.PaymentTypeCode);
            modelBuilder.Entity<DeUnitMeasure>().HasKey(o => o.UnitMeasureCode);
            modelBuilder.Entity<DeDepartment>().HasKey(o => o.DepartmentCode);
            modelBuilder.Entity<DeBusinessPartner>().HasKey(o => new { o.BusinessPartnerCode, o.BusinessPartnerType });
            modelBuilder.Entity<DeBusinessPartnerContact>().HasKey(o => new { o.BusinessPartnerCode, o.BusinessPartnerContactCode });
            modelBuilder.Entity<DeUser>().HasKey(o => o.UserCode);
            modelBuilder.Entity<DeSellTransactionHead>().HasKey(o => new { o.StoreCode, o.PosCode, o.TransactionDateTime, o.TransactionNumber });
            modelBuilder.Entity<DeSellTransactionDetail>().HasKey(o => new { o.StoreCode, o.PosCode, o.TransactionDateTime, o.TransactionNumber, o.RowNumber });
            modelBuilder.Entity<DeSellTransactionPayment>().HasKey(o => new { o.StoreCode, o.PosCode, o.TransactionDateTime, o.TransactionNumber, o.RowNumber });
            modelBuilder.Entity<DeStorePos>().HasKey(o => new { o.StoreCode, o.StorePosCode });
            modelBuilder.Entity<DeActivityLog>().HasKey(o => o.ID );
            modelBuilder.Entity<DeTable>().HasKey(o => o.ID );
            modelBuilder.Entity<DeSellOrder>().HasKey(o => o.SellOrderId );
            modelBuilder.Entity<DeSellOrderDetail>().HasKey(o => new { o.SellOrderId, o.LineNumber });
            modelBuilder.Entity<DePosClosureHead>().HasKey(o => o.PosClosureHeadId );
            modelBuilder.Entity<DePosClosureDetail>().HasKey(o => new { o.PosClosureHeadId, o.StoreCode, o.StorePosCode, o.TransactionNumber });
            modelBuilder.Entity<DeUserMobileProfile>().HasKey(o => new { o.StoreCode, o.UserCode });
            modelBuilder.Entity<DeUserSellOrder>().HasKey(o => new { o.UserCode, o.SellOrderId });
            modelBuilder.Entity<DeBusinessPartnerGroup>().HasKey(o => o.BusinessPartnerGroupCode );
        }
    }
}
