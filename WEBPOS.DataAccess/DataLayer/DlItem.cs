using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.Helpers;
using WEBPOS.DataAccess.Models;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlItem
    {
        private WEBPOSContext context = new WEBPOSContext();
        public IEnumerable<DeItem> ReadAll()
        {
            return context.Items.ToList();
        }
        public IQueryable<DeItem> ReadAllQueryable()
        {
            var queryString = $@"SELECT * FROM srItem";
            return context.Items;
        }
        public IEnumerable<ItemSearchResult> ReadSearch(string param, string plCode, string whCode)
        {
            var queryString = $@"SELECT A.ItemCode, A.ItemDescription, ISNULL(C.QuantityOnHand, 0) AvailableQty, ISNULL(B.SellPrice, 0) Price
FROM srItem A
INNER JOIN srPrice B ON A.ItemCode = B.ItemCode
INNER JOIN srItemWarehouse C ON A.ItemCode = C.ItemCode
WHERE (A.ItemCode like '%{param}%' OR A.ItemDescription like '%{param}%') AND (B.PriceListCode = '{whCode}' AND C.WarehouseCode = '{plCode}')";
            return context.Database.SqlQuery<ItemSearchResult>(queryString);
        }
        public DeItem ReadByCode(string itemCode)
        {
            return context.Items.FirstOrDefault(x => x.ItemCode == itemCode);
        }
        public IEnumerable<DeItem> Read(DeItem obj)
        {
            var data = context.Items.ToList();

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
            var val = context.Items.FirstOrDefault(x => x.ItemCode == obj.ItemCode);
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
                context.Items.Add(obj);

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Articulo", obj.ItemCode)
                };
                BlActivityLog.Save(activity);
            }
            context.SaveChanges();
        }

        public void Delete(string itemCode)
        {
            var obj = context.Items.FirstOrDefault(x=>x.ItemCode == itemCode);
            if(obj != null)
            {
                foreach (var price in new DlPrice().Read(new DePrice { ItemCode = itemCode }))
                {
                    context.Prices.Remove(price);
                }
                context.SaveChanges();

                foreach (var warehouse in new DlItemWarehouse().Read(new DeItemWarehouse { ItemCode = itemCode }))
                {
                    context.ItemWarehouses.Remove(warehouse);
                }
                context.SaveChanges();

                context.Items.Remove(obj);
                context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Articulo", obj.ItemCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
