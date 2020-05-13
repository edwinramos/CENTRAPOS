using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentraPos.DataAccess.DataEntities;
using CentraPos.DataAccess.DataLayer;
using CentraPos.DataAccess.Models;

namespace CentraPos.DataAccess.BusinessLayer
{
    public static class BlItem
    {
        public static IEnumerable<DeItem> ReadAll()
        {
            var dl = new DlItem();
            return dl.ReadAll();
        }
        public static IQueryable<DeItem> ReadAllQueryable()
        {
            var dl = new DlItem();
            return dl.ReadAllQueryable();
        }
        public static IEnumerable<ItemSearchResult> ReadSearch(string param, string priceListCode, string warehouseCode)
        {
            var dl = new DlItem();
            return dl.ReadSearch(param, priceListCode, warehouseCode);
        }
        public static DeItem ReadByCode(string itemCode)
        {
            var dl = new DlItem();
            return dl.ReadByCode(itemCode);
        }
        public static IEnumerable<DeItem> Read(DeItem obj)
        {
            var dl = new DlItem();
            return dl.Read(obj);
        }

        public static void Save(DeItem obj)
        {
            var dl = new DlItem();
            dl.Save(obj);
        }

        public static void Delete(DeItem obj)
        {
            var dl = new DlItem();
            dl.Delete(obj.ItemCode);
        }

        public static string GetNextItemCode()
        {
            var count = ReadAll().Count() + 1;
            string str = count.ToString().PadLeft(7, '0');

            while (ReadAllQueryable().Any(x => x.ItemCode == str))
            {
                count++;
                str = count.ToString().PadRight(7, '0');
            }

            return str;
        }
    }
}
