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
    public class DlUserSellOrder
    {
        private CentraPosDBContext context = new CentraPosDBContext();
        public IEnumerable<DeUserSellOrder> ReadAll()
        {
            return context.UserSellOrders.ToList();
        }
        public IQueryable<DeUserSellOrder> ReadAllQueryable()
        {
            return context.UserSellOrders;
        }

        public DeUserSellOrder ReadByCode(string userCode, int sellOrderId)
        {
            return context.UserSellOrders.FirstOrDefault(x=>x.UserCode == userCode && x.SellOrderId == sellOrderId);
        }

        public IEnumerable<DeUserSellOrder> Read(DeUserSellOrder obj)
        {
            var data = context.UserSellOrders.ToList();

            if (!string.IsNullOrEmpty(obj.UserCode))
                data = data.Where(x => x.UserCode == obj.UserCode).ToList();

            if (obj.SellOrderId != 0)
                data = data.Where(x=>x.SellOrderId == obj.SellOrderId).ToList();

            return data;
        }

        public void Save(DeUserSellOrder obj)
        {
            var val = context.UserSellOrders.FirstOrDefault(x => x.UserCode == obj.UserCode && x.SellOrderId == obj.SellOrderId);
            if (val != null)
            {
                val.UserOrderState = obj.UserOrderState;
            }
            else
            {
                context.UserSellOrders.Add(obj);
            }
            context.SaveChanges();
        }

        public void Delete(string userCode, int sellOrderId)
        {
            var obj = context.UserSellOrders.FirstOrDefault(x => x.UserCode == userCode && x.SellOrderId == sellOrderId);
            if (obj != null)
            {
                context.UserSellOrders.Remove(obj);
                context.SaveChanges();

                //var activity = new DeActivityLog
                //{
                //    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Precio", obj.ItemCode + "|" + obj.UserSellOrderListCode)
                //};
                //BlActivityLog.Save(activity);
            }
        }
    }
}
