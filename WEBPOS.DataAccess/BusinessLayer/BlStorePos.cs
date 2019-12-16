using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlStorePos
    {
        public static IEnumerable<DeStorePos> ReadAll()
        {
            var dl = new DlStorePos();
            return dl.ReadAll();
        }
        public static IQueryable<DeStorePos> ReadAllQueryable()
        {
            var dl = new DlStorePos();
            return dl.ReadAllQueryable();
        }
        public static IEnumerable<DeStorePos> Read(DeStorePos obj)
        {
            var dl = new DlStorePos();
            return dl.Read(obj);
        }

        public static void Save(DeStorePos obj)
        {
            var dl = new DlStorePos();
            dl.Save(obj);
        }

        public static void Delete(DeStorePos obj)
        {
            var dl = new DlStorePos();
            dl.Delete(obj.StoreCode, obj.StorePosCode);
        }

        public static string GetNextStorePosCode(string storeCode)
        {
            var list = ReadAllQueryable().Where(x => x.StoreCode == storeCode);
            var count = list.Count() + 1;
            string str = count.ToString().PadLeft(7, '0');

            while (list.Any(x => x.StorePosCode == str))
            {
                count++;
                str = count.ToString().PadRight(7, '0');
            }

            return str;
        }
    }
}
