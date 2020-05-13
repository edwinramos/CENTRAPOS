using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.Helpers;
using WEBPOS.DataAccess.Models;
using WEBPOS.DataAccess.Repository;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlItem : BaseRepository<WEBPOSContext, DeItem>
    {
        public DlItem(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeItem> ReadAll()
        {
            return Context.Items.ToList();
        }
        public IQueryable<DeItem> ReadAllQueryable()
        {
            var queryString = $@"SELECT * FROM srItem";
            return Context.Items;
        }
        public IEnumerable<ItemSearchResult> ReadSearch(string param, string plCode, string whCode)
        {
            var queryString = $@"SELECT A.ItemCode, A.ItemDescription, ISNULL(C.QuantityOnHand, 0) AvailableQty, ISNULL(B.SellPrice, 0) Price
FROM srItem A
INNER JOIN srPrice B ON A.ItemCode = B.ItemCode
INNER JOIN srItemWarehouse C ON A.ItemCode = C.ItemCode
WHERE (A.ItemCode like '%{param}%' OR A.ItemDescription like '%{param}%') AND (B.PriceListCode = '{whCode}' AND C.WarehouseCode = '{plCode}')";
            return Context.Database.SqlQuery<ItemSearchResult>(queryString);
        }
        public DeItem ReadByCode(string itemCode)
        {
            return Context.Items.FirstOrDefault(x => x.ItemCode == itemCode);
        }
        public IEnumerable<DeItem> Read(DeItem obj)
        {
            var data = Context.Items.ToList();

            if (!string.IsNullOrEmpty(obj.ItemCode))
                data = data.Where(x=>x.ItemCode == obj.ItemCode).ToList();

            if (!string.IsNullOrEmpty(obj.ItemDescription))
                data = data.Where(x => x.ItemDescription == obj.ItemDescription).ToList();

            if (!string.IsNullOrEmpty(obj.Barcode))
                data = data.Where(x => x.Barcode == obj.Barcode).ToList();

            return data;
        }

        public void Save(DeItem obj)
        {
            var val = Context.Items.FirstOrDefault(x => x.ItemCode == obj.ItemCode);
            if (val != null)
            {
                val.ItemDescription = obj.ItemDescription;
                val.Barcode = obj.Barcode;
                val.DepartmentCode = obj.DepartmentCode;
                val.SupplierCode = obj.SupplierCode;
                val.TaxCode = obj.TaxCode;
                val.UnitMeasureCode = obj.UnitMeasureCode;

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.UPDATE), "Articulo", obj.ItemCode)
                };
                BlActivityLog.Save(activity);
            }
            else
            {
                Context.Items.Add(obj);

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Articulo", obj.ItemCode)
                };
                BlActivityLog.Save(activity);
            }
            Context.SaveChanges();
        }

        public void Delete(string itemCode)
        {
            var obj = Context.Items.FirstOrDefault(x=>x.ItemCode == itemCode);
            if(obj != null)
            {
                foreach (var price in new DlPrice().Read(new DePrice { ItemCode = itemCode }))
                {
                    Context.Prices.Remove(price);
                }
                Context.SaveChanges();

                foreach (var warehouse in new DlItemWarehouse().Read(new DeItemWarehouse { ItemCode = itemCode }))
                {
                    Context.ItemWarehouses.Remove(warehouse);
                }
                Context.SaveChanges();

                Context.Items.Remove(obj);
                Context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Articulo", obj.ItemCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
