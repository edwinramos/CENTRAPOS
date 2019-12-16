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
    public class DlPriceList
    {
        private WEBPOSContext context = new WEBPOSContext();
        public IEnumerable<DePriceList> ReadAll()
        {
            return context.PriceLists.ToList();
        }
        public IQueryable<DePriceList> ReadAllQueryable()
        {
            return context.PriceLists;
        }
        public IEnumerable<DePriceList> Read(DePriceList obj)
        {
            var data = context.PriceLists.ToList();

            if (!string.IsNullOrEmpty(obj.PriceListCode))
                data = data.Where(x=>x.PriceListCode == obj.PriceListCode).ToList();

            if (!string.IsNullOrEmpty(obj.PriceListDescription))
                data = data.Where(x => x.PriceListDescription == obj.PriceListDescription).ToList();

            return data;
        }

        public void Save(DePriceList obj)
        {
            var val = context.PriceLists.FirstOrDefault(x => x.PriceListCode == obj.PriceListCode);
            if (val != null)
            {
                val.PriceListDescription = obj.PriceListDescription;

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.UPDATE), "Lista de Precio", obj.PriceListCode)
                };
                BlActivityLog.Save(activity);
            }
            else
            {
                context.PriceLists.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Lista de Precio", obj.PriceListCode)
                };
                BlActivityLog.Save(activity);
            }
            context.SaveChanges();
        }

        public void Delete(string priceListCode)
        {
            var obj = context.PriceLists.FirstOrDefault(x=>x.PriceListCode == priceListCode);
            if(obj != null)
            {
                context.PriceLists.Remove(obj);
                context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Lista de Precio", obj.PriceListCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
