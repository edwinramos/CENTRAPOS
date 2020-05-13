using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentraPos.DataAccess.BusinessLayer;
using CentraPos.DataAccess.DataEntities;
using CentraPos.DataAccess.Helpers;

namespace CentraPos.DataAccess.DataLayer
{
    public class DlWarehouse
    {
        private CentraPosDBContext context = new CentraPosDBContext();
        public IEnumerable<DeWarehouse> ReadAll()
        {
            return context.Warehouses.ToList();
        }
        public IQueryable<DeWarehouse> ReadAllQueryable()
        {
            return context.Warehouses;
        }
        public IEnumerable<DeWarehouse> Read(DeWarehouse obj)
        {
            var data = context.Warehouses.ToList();

            if (!string.IsNullOrEmpty(obj.WarehouseCode))
                data = data.Where(x=>x.WarehouseCode == obj.WarehouseCode).ToList();

            if (!string.IsNullOrEmpty(obj.WarehouseDescription))
                data = data.Where(x => x.WarehouseDescription == obj.WarehouseDescription).ToList();

            return data;
        }

        public void Save(DeWarehouse obj)
        {
            var val = context.Warehouses.FirstOrDefault(x => x.WarehouseCode == obj.WarehouseCode);
            if (val != null)
            {
                val.WarehouseDescription = obj.WarehouseDescription;
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.UPDATE), "Almacen", obj.WarehouseCode)
                };
                BlActivityLog.Save(activity);
            }
            else
            {
                context.Warehouses.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Almacen", obj.WarehouseCode)
                };
                BlActivityLog.Save(activity);
            }
            context.SaveChanges();
        }

        public void Delete(string WarehouseCode)
        {
            var obj = context.Warehouses.FirstOrDefault(x=>x.WarehouseCode == WarehouseCode);
            if(obj != null)
            {
                context.Warehouses.Remove(obj);
                context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Almacen", obj.WarehouseCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
