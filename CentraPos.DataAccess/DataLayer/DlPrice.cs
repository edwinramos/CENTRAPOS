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
    public class DlPrice
    {
        private CentraPosDBContext context = new CentraPosDBContext();
        public IEnumerable<DePrice> ReadAll()
        {
            return context.Prices.ToList();
        }
        public IQueryable<DePrice> ReadAllQueryable()
        {
            return context.Prices;
        }

        public DePrice ReadByCode(string itemCode, string priceListCode)
        {
            return context.Prices.FirstOrDefault(x=>x.ItemCode == itemCode && x.PriceListCode == priceListCode);
        }

        public IEnumerable<DePrice> Read(DePrice obj)
        {
            var data = context.Prices.ToList();

            if (!string.IsNullOrEmpty(obj.ItemCode))
                data = data.Where(x => x.ItemCode == obj.ItemCode).ToList();

            if (!string.IsNullOrEmpty(obj.PriceListCode))
                data = data.Where(x=>x.PriceListCode == obj.PriceListCode).ToList();

            if (!data.All(x=>x.ItemCode == obj.ItemCode))
                return new List<DePrice>();

            return data;
        }

        public void Save(DePrice obj)
        {
            var val = context.Prices.FirstOrDefault(x => x.ItemCode == obj.ItemCode && x.PriceListCode == obj.PriceListCode);
            if (val != null)
            {
                val.SellPrice = obj.SellPrice;

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.UPDATE), "Precio", obj.ItemCode + "|" + obj.PriceListCode)
                };
                BlActivityLog.Save(activity);
            }
            else
            {
                context.Prices.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Precio", obj.ItemCode + "|" + obj.PriceListCode)
                };
                BlActivityLog.Save(activity);
            }
            context.SaveChanges();
        }

        public void Delete(string itemCode, string priceListCode)
        {
            var obj = context.Prices.FirstOrDefault(x=> x.ItemCode == itemCode && x.PriceListCode == priceListCode);
            if(obj != null)
            {
                context.Prices.Remove(obj);
                context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Precio", obj.ItemCode + "|" + obj.PriceListCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
