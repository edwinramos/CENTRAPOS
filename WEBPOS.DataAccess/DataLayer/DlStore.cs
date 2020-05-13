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
    public class DlStore : BaseRepository<WEBPOSContext, DeStore>
    {
        public DlStore(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeStore> ReadAll()
        {
            return Context.Stores.ToList();
        }
        public IQueryable<DeStore> ReadAllQueryable()
        {
            return Context.Stores;
        }
        public DeStore ReadByCode(string StoreCode)
        {
            return Context.Stores.FirstOrDefault(x => x.StoreCode == StoreCode);
        }
        public IEnumerable<DeStore> Read(DeStore obj)
        {
            var data = Context.Stores.ToList();

            if (!string.IsNullOrEmpty(obj.StoreCode))
                data = data.Where(x=>x.StoreCode == obj.StoreCode).ToList();

            if (!string.IsNullOrEmpty(obj.StoreDescription))
                data = data.Where(x => x.StoreDescription == obj.StoreDescription).ToList();

            return data;
        }

        public void Save(DeStore obj)
        {
            var val = Context.Stores.FirstOrDefault(x => x.StoreCode == obj.StoreCode);
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
                Context.Stores.Add(obj);

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Tienda", obj.StoreCode)
                };
                BlActivityLog.Save(activity);
            }
            Context.SaveChanges();
        }

        public void Delete(string StoreCode)
        {
            var obj = Context.Stores.FirstOrDefault(x=>x.StoreCode == StoreCode);
            if(obj != null)
            {
                foreach (var store in Read(new DeStore { StoreCode = StoreCode }))
                {
                    Context.Stores.Remove(store);
                }

                Context.Stores.Remove(obj);
                Context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Tienda", obj.StoreCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
