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
    public class DlPrice : BaseRepository<WEBPOSContext, DePrice>
    {
        public DlPrice(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DePrice> ReadAll()
        {
            return Context.Prices.ToList();
        }
        public IQueryable<DePrice> ReadAllQueryable()
        {
            return Context.Prices;
        }

        public DePrice ReadByCode(string itemCode, string priceListCode)
        {
            return Context.Prices.FirstOrDefault(x=>x.ItemCode == itemCode && x.PriceListCode == priceListCode);
        }

        public IEnumerable<DePrice> Read(DePrice obj)
        {
            var data = Context.Prices.ToList();

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
            var val = Context.Prices.FirstOrDefault(x => x.ItemCode == obj.ItemCode && x.PriceListCode == obj.PriceListCode);
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
                Context.Prices.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Precio", obj.ItemCode + "|" + obj.PriceListCode)
                };
                BlActivityLog.Save(activity);
            }
            Context.SaveChanges();
        }

        public void Delete(string itemCode, string priceListCode)
        {
            var obj = Context.Prices.FirstOrDefault(x=> x.ItemCode == itemCode && x.PriceListCode == priceListCode);
            if(obj != null)
            {
                Context.Prices.Remove(obj);
                Context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Precio", obj.ItemCode + "|" + obj.PriceListCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
