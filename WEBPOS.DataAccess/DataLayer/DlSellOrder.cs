using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.Helpers;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlSellOrder
    {
        private WEBPOSContext context = new WEBPOSContext();
        public IEnumerable<DeSellOrder> ReadAll()
        {
            return context.SellOrders.ToList();
        }
        public IQueryable<DeSellOrder> ReadAllQueryable()
        {
            return context.SellOrders;
        }
        public IQueryable<DeSellOrder> ReadByGroupCode(string groupCode)
        {
            var queryString = $@"SELECT *
FROM srSellOrder A INNER JOIN srBusinessPartner B
ON A.ClientCode = B.BusinessPartnerCode
JOIN srUserSellOrder C
ON A.SellOrderId != C.SellOrderId
WHERE A.IsClosed = 0 AND B.BusinessPartnerGroupCode = '{groupCode}'";

            return context.Database.SqlQuery<DeSellOrder>(queryString).AsQueryable();
        }
        public IEnumerable<DeSellOrder> Read(DeSellOrder obj)
        {
            var data = context.SellOrders.ToList();
            if (!string.IsNullOrEmpty(obj.StoreCode))
                data = data.Where(x => x.StoreCode == obj.StoreCode).ToList();

            if (obj.SellOrderId != 0)
                data = data.Where(x => x.SellOrderId == obj.SellOrderId).ToList();

            return data;
        }

        public void Save(DeSellOrder obj)
        {
            var old = context.SellOrders.FirstOrDefault(x => x.SellOrderId == obj.SellOrderId);
            if (old == null)
            {
                context.SellOrders.Add(obj);

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Orden", obj.SellOrderId)
                };
                BlActivityLog.Save(activity);
            }
            else
            {
                old.PriceListCode = obj.PriceListCode;
                old.PaymentTypeCode = obj.PaymentTypeCode;
                old.IsClosed = obj.IsClosed;
                old.ExternalReference = obj.ExternalReference;
                old.ClientCode = obj.ClientCode;
                old.ClientDescription = obj.ClientDescription;
                old.ClosedDateTime = obj.ClosedDateTime;
                old.Comments = obj.Comments;
                old.DocDateTime = obj.DocDateTime;
                old.DocNetTotal = obj.DocNetTotal;
                old.DocTotal = obj.DocTotal;
                old.StoreCode = obj.StoreCode;
                old.TotalDiscount = obj.TotalDiscount;
                old.VatSum = obj.VatSum;
                old.WarehouseCode = obj.WarehouseCode;
                old.StorePosCode = obj.StorePosCode;

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.UPDATE), "Orden", obj.SellOrderId)
                };

                BlActivityLog.Save(activity);
            }

            context.SaveChanges();
        }

        public void Delete(int sellOrderId)
        {
            var obj = context.SellOrders.FirstOrDefault(x => x.SellOrderId == sellOrderId);
            if (obj != null)
            {
                context.SellOrders.Remove(obj);
                context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Orden", obj.SellOrderId)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
