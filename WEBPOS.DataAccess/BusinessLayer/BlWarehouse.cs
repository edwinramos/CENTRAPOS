using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlWarehouse
    {
        public static IEnumerable<DeWarehouse> ReadAll()
        {
            var dl = new DlWarehouse();
            return dl.ReadAll();
        }
        public static IEnumerable<DeWarehouse> ReadAllQueryable()
        {
            var dl = new DlWarehouse();
            return dl.ReadAll();
        }
        public static IEnumerable<DeWarehouse> Read(DeWarehouse obj)
        {
            var dl = new DlWarehouse();
            return dl.Read(obj);
        }

        public static void Save(DeWarehouse obj)
        {
            var dl = new DlWarehouse();
            dl.Save(obj);
        }

        public static void Delete(DeWarehouse obj)
        {
            var dl = new DlWarehouse();
            dl.Delete(obj.WarehouseCode);
        }
    }
}
