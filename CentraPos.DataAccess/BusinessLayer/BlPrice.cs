using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentraPos.DataAccess.DataEntities;
using CentraPos.DataAccess.DataLayer;

namespace CentraPos.DataAccess.BusinessLayer
{
    public static class BlPrice
    {
        public static IEnumerable<DePrice> ReadAll()
        {
            var dl = new DlPrice();
            return dl.ReadAll();
        }
        public static DePrice ReadByCode(string itemCode, string priceListCode)
        {
            var dl = new DlPrice();
            return dl.ReadByCode(itemCode, priceListCode);
        }
        public static IQueryable<DePrice> ReadAllQueryable()
        {
            var dl = new DlPrice();
            return dl.ReadAllQueryable();
        }
        public static IEnumerable<DePrice> Read(DePrice obj)
        {
            var dl = new DlPrice();
            return dl.Read(obj);
        }

        public static void Save(DePrice obj)
        {
            var dl = new DlPrice();
            dl.Save(obj);
        }

        public static void Delete(DePrice obj)
        {
            var dl = new DlPrice();
            dl.Delete(obj.ItemCode, obj.PriceListCode);
        }
    }
}
