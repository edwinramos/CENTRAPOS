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
    public class DlUserSellOrder : BaseRepository<WEBPOSContext, DeUserSellOrder>
    {
        public DlUserSellOrder(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeUserSellOrder> ReadAll()
        {
            return Context.UserSellOrders.ToList();
        }
        public IQueryable<DeUserSellOrder> ReadAllQueryable()
        {
            return Context.UserSellOrders;
        }

        public DeUserSellOrder ReadByCode(string userCode, int sellOrderId)
        {
            return Context.UserSellOrders.FirstOrDefault(x=>x.UserCode == userCode && x.SellOrderId == sellOrderId);
        }

        public IEnumerable<DeUserSellOrder> Read(DeUserSellOrder obj)
        {
            var data = Context.UserSellOrders.ToList();

            if (!string.IsNullOrEmpty(obj.UserCode))
                data = data.Where(x => x.UserCode == obj.UserCode).ToList();

            if (obj.SellOrderId != 0)
                data = data.Where(x=>x.SellOrderId == obj.SellOrderId).ToList();

            return data;
        }

        public void Save(DeUserSellOrder obj)
        {
            var val = Context.UserSellOrders.FirstOrDefault(x => x.UserCode == obj.UserCode && x.SellOrderId == obj.SellOrderId);
            if (val != null)
            {
                val.UserOrderState = obj.UserOrderState;
            }
            else
            {
                Context.UserSellOrders.Add(obj);
            }
            Context.SaveChanges();
        }

        public void Delete(string userCode, int sellOrderId)
        {
            var obj = Context.UserSellOrders.FirstOrDefault(x => x.UserCode == userCode && x.SellOrderId == sellOrderId);
            if (obj != null)
            {
                Context.UserSellOrders.Remove(obj);
                Context.SaveChanges();

                //var activity = new DeActivityLog
                //{
                //    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Precio", obj.ItemCode + "|" + obj.UserSellOrderListCode)
                //};
                //BlActivityLog.Save(activity);
            }
        }
    }
}
