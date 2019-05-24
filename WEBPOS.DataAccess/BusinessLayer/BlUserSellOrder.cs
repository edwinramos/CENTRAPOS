using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlUserSellOrder
    {
        public static IEnumerable<DeUserSellOrder> ReadAll()
        {
            var dl = new DlUserSellOrder();
            return dl.ReadAll();
        }
        public static DeUserSellOrder ReadByCode(string userCode, int sellOrderId)
        {
            var dl = new DlUserSellOrder();
            return dl.ReadByCode(userCode, sellOrderId);
        }
        public static IEnumerable<DeUserSellOrder> ReadAllQueryable()
        {
            var dl = new DlUserSellOrder();
            return dl.ReadAll();
        }
        public static IEnumerable<DeUserSellOrder> Read(DeUserSellOrder obj)
        {
            var dl = new DlUserSellOrder();
            return dl.Read(obj);
        }

        public static void Save(DeUserSellOrder obj)
        {
            var dl = new DlUserSellOrder();
            dl.Save(obj);
        }

        public static void Delete(DeUserSellOrder obj)
        {
            var dl = new DlUserSellOrder();
            dl.Delete(obj.UserCode, obj.SellOrderId);
        }
    }
}
