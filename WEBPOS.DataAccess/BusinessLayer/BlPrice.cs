using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
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
        public static IEnumerable<DePrice> ReadAllQueryable()
        {
            var dl = new DlPrice();
            return dl.ReadAll();
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
