using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlItemWarehouse
    {
        public static IEnumerable<DeItemWarehouse> ReadAll()
        {
            var dl = new DlItemWarehouse();
            return dl.ReadAll();
        }
        public static IQueryable<DeItemWarehouse> ReadAllQueryable()
        {
            var dl = new DlItemWarehouse();
            return dl.ReadAllQueryable();
        }
        public static IEnumerable<DeItemWarehouse> Read(DeItemWarehouse obj)
        {
            var dl = new DlItemWarehouse();
            return dl.Read(obj);
        }

        public static void Save(DeItemWarehouse obj)
        {
            var dl = new DlItemWarehouse();
            dl.Save(obj);
        }

        public static void Delete(DeItemWarehouse obj)
        {
            var dl = new DlItemWarehouse();
            dl.Delete(obj.ItemCode, obj.WarehouseCode);
        }
    }
}
