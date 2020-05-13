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
    public class DlWarehouse : BaseRepository<WEBPOSContext, DeWarehouse>
    {
        public DlWarehouse(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeWarehouse> ReadAll()
        {
            return Context.Warehouses.ToList();
        }
        public IQueryable<DeWarehouse> ReadAllQueryable()
        {
            return Context.Warehouses;
        }
        public IEnumerable<DeWarehouse> Read(DeWarehouse obj)
        {
            var data = Context.Warehouses.ToList();

            if (!string.IsNullOrEmpty(obj.WarehouseCode))
                data = data.Where(x=>x.WarehouseCode == obj.WarehouseCode).ToList();

            if (!string.IsNullOrEmpty(obj.WarehouseDescription))
                data = data.Where(x => x.WarehouseDescription == obj.WarehouseDescription).ToList();

            return data;
        }

        public void Save(DeWarehouse obj)
        {
            var val = Context.Warehouses.FirstOrDefault(x => x.WarehouseCode == obj.WarehouseCode);
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
                Context.Warehouses.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Almacen", obj.WarehouseCode)
                };
                BlActivityLog.Save(activity);
            }
            Context.SaveChanges();
        }

        public void Delete(string WarehouseCode)
        {
            var obj = Context.Warehouses.FirstOrDefault(x=>x.WarehouseCode == WarehouseCode);
            if(obj != null)
            {
                Context.Warehouses.Remove(obj);
                Context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Almacen", obj.WarehouseCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
