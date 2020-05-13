using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.BusinessLayer;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.Helpers;
using WEBPOS.DataAccess.Models;
using WEBPOS.DataAccess.Repository;

namespace WEBPOS.DataAccess.DataLayer
{
    public class DlSellOrder : BaseRepository<WEBPOSContext, DeSellOrder>
    {
        public DlSellOrder(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeSellOrder> ReadAll()
        {
            return Context.SellOrders.ToList();
        }
        public IEnumerable<DeSellOrder> ReadAllQueryableCustom(string filters)
        {
            var queryString = $@"SELECT *
FROM srSellOrder
WHERE {filters}";
            return Context.Database.SqlQuery<DeSellOrder>(queryString);
        }
        public IQueryable<DeSellOrder> ReadAllQueryable()
        {
            return Context.SellOrders;
        }
        public IQueryable<SoldQuantityModel> GetSoldQtyByDate(string userCode, DateTime dateFrom, DateTime dateTo)
        {
            var queryString = $@"SELECT '{userCode}' UserCode, '{dateFrom.ToString("yyyy-MM-dd")}' FromDate, '{dateTo.ToString("yyyy-MM-dd")}' ToDate, SUM(B.Quantity) Quantity, B.ItemDescription
FROM srSellOrder A inner join srSellOrderDetail B
ON A.SellOrderId = B.SellOrderId
where A.UpdateUser = '{userCode}' AND A.DocDateTime BETWEEN '{dateFrom.ToString("yyyy-MM-dd")}' AND '{dateTo.ToString("yyyy-MM-dd")}'
GROUP BY B.ItemDescription";

            return Context.Database.SqlQuery<SoldQuantityModel>(queryString).AsQueryable();
        }
        public IQueryable<DeSellOrder> ReadByGroupCode(string groupCode)
        {
            var queryString = $@"SELECT *
FROM srSellOrder A INNER JOIN srBusinessPartner B
ON A.ClientCode = B.BusinessPartnerCode
WHERE A.IsClosed = 0 AND B.BusinessPartnerGroupCode = '{groupCode}' AND A.SellOrderId NOT IN (select SellOrderId from srUserSellOrder)";

            return Context.Database.SqlQuery<DeSellOrder>(queryString).AsQueryable();
        }
        public IEnumerable<DeSellOrder> Read(DeSellOrder obj)
        {
            var data = Context.SellOrders.ToList();
            if (!string.IsNullOrEmpty(obj.StoreCode))
                data = data.Where(x => x.StoreCode == obj.StoreCode).ToList();

            if (obj.SellOrderId != 0)
                data = data.Where(x => x.SellOrderId == obj.SellOrderId).ToList();

            return data;
        }

        public void Save(DeSellOrder obj)
        {
            var old = Context.SellOrders.FirstOrDefault(x => x.SellOrderId == obj.SellOrderId);
            if (old == null)
            {
                Context.SellOrders.Add(obj);

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

            Context.SaveChanges();
        }

        public void Delete(int sellOrderId)
        {
            var obj = Context.SellOrders.FirstOrDefault(x => x.SellOrderId == sellOrderId);
            if (obj != null)
            {
                Context.SellOrders.Remove(obj);
                Context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Orden", obj.SellOrderId)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
