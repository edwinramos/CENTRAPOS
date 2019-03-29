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
    public class DlStore
    {
        private WEBPOSContext context = new WEBPOSContext();
        public IEnumerable<DeStore> ReadAll()
        {
            return context.Stores.ToList();
        }
        public IEnumerable<DeStore> ReadAllQueryable()
        {
            return context.Stores;
        }
        public DeStore ReadByCode(string StoreCode)
        {
            return context.Stores.FirstOrDefault(x => x.StoreCode == StoreCode);
        }
        public IEnumerable<DeStore> Read(DeStore obj)
        {
            var data = context.Stores.ToList();

            if (!string.IsNullOrEmpty(obj.StoreCode))
                data = data.Where(x=>x.StoreCode == obj.StoreCode).ToList();

            if (!string.IsNullOrEmpty(obj.StoreDescription))
                data = data.Where(x => x.StoreDescription == obj.StoreDescription).ToList();

            return data;
        }

        public void Save(DeStore obj)
        {
            var val = context.Stores.FirstOrDefault(x => x.StoreCode == obj.StoreCode);
            if (val != null)
            {
                val.StoreDescription = obj.StoreDescription;
                val.PriceListCode = obj.PriceListCode;
                val.WarehouseCode = obj.WarehouseCode;
                val.RNC = obj.RNC;
                val.NIF = obj.NIF;
                val.NCFSequence1 = obj.NCFSequence1;
                val.NCFSequence2 = obj.NCFSequence2;
                val.Telephone = obj.Telephone;
                val.City = obj.City;
                val.Address = obj.Address;
                val.SequenceDueDate = obj.SequenceDueDate;
                val.MaxDiscAmount = obj.MaxDiscAmount;
                val.MaxDiscPercent = obj.MaxDiscPercent;

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.UPDATE), "Tienda", obj.StoreCode)
                };
                BlActivityLog.Save(activity);
            }
            else
            {
                context.Stores.Add(obj);

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Tienda", obj.StoreCode)
                };
                BlActivityLog.Save(activity);
            }
            context.SaveChanges();
        }

        public void Delete(string StoreCode)
        {
            var obj = context.Stores.FirstOrDefault(x=>x.StoreCode == StoreCode);
            if(obj != null)
            {
                foreach (var store in Read(new DeStore { StoreCode = StoreCode }))
                {
                    context.Stores.Remove(store);
                }

                context.Stores.Remove(obj);
                context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Tienda", obj.StoreCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
