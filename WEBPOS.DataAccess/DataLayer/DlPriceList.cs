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
    public class DlPriceList : BaseRepository<WEBPOSContext, DePriceList>
    {
        public DlPriceList(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DePriceList> ReadAll()
        {
            return Context.PriceLists.ToList();
        }
        public IQueryable<DePriceList> ReadAllQueryable()
        {
            return Context.PriceLists;
        }
        public IEnumerable<DePriceList> Read(DePriceList obj)
        {
            var data = Context.PriceLists.ToList();

            if (!string.IsNullOrEmpty(obj.PriceListCode))
                data = data.Where(x=>x.PriceListCode == obj.PriceListCode).ToList();

            if (!string.IsNullOrEmpty(obj.PriceListDescription))
                data = data.Where(x => x.PriceListDescription == obj.PriceListDescription).ToList();

            return data;
        }

        public void Save(DePriceList obj)
        {
            var val = Context.PriceLists.FirstOrDefault(x => x.PriceListCode == obj.PriceListCode);
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
                Context.PriceLists.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Lista de Precio", obj.PriceListCode)
                };
                BlActivityLog.Save(activity);
            }
            Context.SaveChanges();
        }

        public void Delete(string priceListCode)
        {
            var obj = Context.PriceLists.FirstOrDefault(x=>x.PriceListCode == priceListCode);
            if(obj != null)
            {
                Context.PriceLists.Remove(obj);
                Context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Lista de Precio", obj.PriceListCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
