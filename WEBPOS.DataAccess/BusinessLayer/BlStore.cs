using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlStore
    {
        public static IEnumerable<DeStore> ReadAll()
        {
            var dl = new DlStore();
            return dl.ReadAll();
        }
        public static IQueryable<DeStore> ReadAllQueryable()
        {
            var dl = new DlStore();
            return dl.ReadAllQueryable();
        }
        public static DeStore ReadByCode(string StoreCode)
        {
            var dl = new DlStore();
            return dl.ReadByCode(StoreCode);
        }
        public static IEnumerable<DeStore> Read(DeStore obj)
        {
            var dl = new DlStore();
            return dl.Read(obj);
        }

        public static void Save(DeStore obj)
        {
            var dl = new DlStore();
            dl.Save(obj);
        }

        public static void Delete(DeStore obj)
        {
            var dl = new DlStore();
            dl.Delete(obj.StoreCode);
        }

        public static string GetNextStoreCode()
        {
            var count = ReadAll().Count() + 1;
            string str = count.ToString().PadLeft(7, '0');

            while (ReadAllQueryable().Any(x => x.StoreCode == str))
            {
                count++;
                str = count.ToString().PadRight(7, '0');
            }

            return str;
        }
    }
}
