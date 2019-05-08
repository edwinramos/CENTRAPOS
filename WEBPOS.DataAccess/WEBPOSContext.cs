using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WEBPOS.DataAccess.DataEntities;

namespace WEBPOS.DataAccess
{
    public class WEBPOSContext : DbContext
    {
        public WEBPOSContext() : base("WEBPOSContext")
        {
            Database.CommandTimeout = 180;
        }

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

        public override int SaveChanges()
        {
            this.ChangeTracker.DetectChanges();
            var added = this.ChangeTracker.Entries()
                        .Where(t => t.State == EntityState.Added)
                        .Select(t => t.Entity)
                        .ToArray();

            foreach (var entity in added)
            {
                if (entity is Base)
                {
                    var track = entity as Base;
                    var user = track.UpdateUser;
                    try { user = (string)HttpContext.Current.Session["UserCode"]; } catch { }
                    
                    track.LastUpdate = DateTime.Now;
                    track.UpdateUser = user;
                }
            }

            var modified = this.ChangeTracker.Entries()
                        .Where(t => t.State == EntityState.Modified)
                        .Select(t => t.Entity)
                        .ToArray();

            foreach (var entity in modified)
            {
                if (entity is Base)
                {
                    var track = entity as Base;
                    var user = track.UpdateUser;
                    try { user = (string)HttpContext.Current.Session["UserCode"]; } catch { }

                    track.LastUpdate = DateTime.Now;
                    track.UpdateUser = user;
                }
            }
            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
