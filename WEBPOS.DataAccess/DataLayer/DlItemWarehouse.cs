using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.Helpers;
using WEBPOS.DataAccess.Repository;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlItemWarehouse : BaseRepository<WEBPOSContext, DeItemWarehouse>
    {
        public DlItemWarehouse(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeItemWarehouse> ReadAll()
        {
            return Context.ItemWarehouses.ToList();
        }
        public IQueryable<DeItemWarehouse> ReadAllQueryable()
        {
            return Context.ItemWarehouses;
        }
        public IEnumerable<DeItemWarehouse> Read(DeItemWarehouse obj)
        {
            var data = Context.ItemWarehouses.ToList();

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
            var val = Context.ItemWarehouses.FirstOrDefault(x => x.ItemCode == obj.ItemCode && x.WarehouseCode == obj.WarehouseCode);
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
                Context.ItemWarehouses.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Articulo en almacen", obj.ItemCode + "|" + obj.WarehouseCode)
                };
                BlActivityLog.Save(activity);
            }
            Context.SaveChanges();
        }

        public void Delete(string itemCode, string warehouseCode)
        {
            var obj = Context.ItemWarehouses.FirstOrDefault(x=>x.ItemCode == itemCode && x.WarehouseCode == warehouseCode);
            if(obj != null)
            {
                Context.ItemWarehouses.Remove(obj);
                Context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Articulo en almacen", obj.ItemCode + "|" + obj.WarehouseCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
