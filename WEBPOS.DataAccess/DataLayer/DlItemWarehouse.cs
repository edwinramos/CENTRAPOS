using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.Helpers;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlItemWarehouse
    {
        private WEBPOSContext context = new WEBPOSContext();
        public IEnumerable<DeItemWarehouse> ReadAll()
        {
            return context.ItemWarehouses.ToList();
        }
        public IQueryable<DeItemWarehouse> ReadAllQueryable()
        {
            return context.ItemWarehouses;
        }
        public IEnumerable<DeItemWarehouse> Read(DeItemWarehouse obj)
        {
            var data = context.ItemWarehouses.ToList();

            if (!string.IsNullOrEmpty(obj.WarehouseCode))
                data = data.Where(x=>x.WarehouseCode == obj.WarehouseCode).ToList();

            if (!string.IsNullOrEmpty(obj.ItemCode))
                data = data.Where(x => x.ItemCode == obj.ItemCode).ToList();

            if (!data.All(x => x.ItemCode == obj.ItemCode))
                return new List<DeItemWarehouse>();

            return data;
        }

        public void Save(DeItemWarehouse obj)
        {
            var val = context.ItemWarehouses.FirstOrDefault(x => x.ItemCode == obj.ItemCode && x.WarehouseCode == obj.WarehouseCode);
            if (val != null)
            {
                val.QuantityOnHand = obj.QuantityOnHand;

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.UPDATE), "Articulo en almacen", obj.ItemCode + "|" + obj.WarehouseCode)
                };
                BlActivityLog.Save(activity);
            }
            else
            {
                context.ItemWarehouses.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Articulo en almacen", obj.ItemCode + "|" + obj.WarehouseCode)
                };
                BlActivityLog.Save(activity);
            }
            context.SaveChanges();
        }

        public void Delete(string itemCode, string warehouseCode)
        {
            var obj = context.ItemWarehouses.FirstOrDefault(x=>x.ItemCode == itemCode && x.WarehouseCode == warehouseCode);
            if(obj != null)
            {
                context.ItemWarehouses.Remove(obj);
                context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Articulo en almacen", obj.ItemCode + "|" + obj.WarehouseCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
