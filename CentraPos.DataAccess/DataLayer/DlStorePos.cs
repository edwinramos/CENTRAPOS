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
    public class DlStorePos
    {
        private CentraPosDBContext context = new CentraPosDBContext();
        public IEnumerable<DeStorePos> ReadAll()
        {
            return context.StorePoses.ToList();
        }
        public IQueryable<DeStorePos> ReadAllQueryable()
        {
            return context.StorePoses;
        }
        public IEnumerable<DeStorePos> Read(DeStorePos obj)
        {
            var data = context.StorePoses.ToList();

            if (!string.IsNullOrEmpty(obj.StoreCode))
                data = data.Where(x => x.StoreCode == obj.StoreCode).ToList();

            if (!string.IsNullOrEmpty(obj.StorePosCode))
                data = data.Where(x=>x.StorePosCode == obj.StorePosCode).ToList();

            return data;
        }

        public void Save(DeStorePos obj)
        {
            var val = context.StorePoses.FirstOrDefault(x => x.StorePosCode == obj.StorePosCode);
            if (val != null)
            {
                val.StorePosDescription = obj.StorePosDescription;
                val.DeviceId = obj.DeviceId;
                val.DeviceType = obj.DeviceType;

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.UPDATE), "Terminal", obj.StorePosCode)
                };
                BlActivityLog.Save(activity);
            }
            else
            {
                context.StorePoses.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Terminal", obj.StorePosCode)
                };
                BlActivityLog.Save(activity);
            }
            context.SaveChanges();
        }

        public void Delete(string storeCode, string storePosCode)
        {
            var obj = context.StorePoses.FirstOrDefault(x=> x.StoreCode == storeCode && x.StorePosCode == storePosCode);
            if(obj != null)
            {
                context.StorePoses.Remove(obj);
                context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Terminal", obj.StorePosCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
