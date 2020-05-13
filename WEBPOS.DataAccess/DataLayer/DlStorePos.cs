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
    public class DlStorePos : BaseRepository<WEBPOSContext, DeStorePos>
    {
        public DlStorePos(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeStorePos> ReadAll()
        {
            return Context.StorePoses.ToList();
        }
        public IQueryable<DeStorePos> ReadAllQueryable()
        {
            return Context.StorePoses;
        }
        public IEnumerable<DeStorePos> Read(DeStorePos obj)
        {
            var data = Context.StorePoses.ToList();

            if (!string.IsNullOrEmpty(obj.StoreCode))
                data = data.Where(x => x.StoreCode == obj.StoreCode).ToList();

            if (!string.IsNullOrEmpty(obj.StorePosCode))
                data = data.Where(x=>x.StorePosCode == obj.StorePosCode).ToList();

            return data;
        }

        public void Save(DeStorePos obj)
        {
            var val = Context.StorePoses.FirstOrDefault(x => x.StorePosCode == obj.StorePosCode);
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
                Context.StorePoses.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Terminal", obj.StorePosCode)
                };
                BlActivityLog.Save(activity);
            }
            Context.SaveChanges();
        }

        public void Delete(string storeCode, string storePosCode)
        {
            var obj = Context.StorePoses.FirstOrDefault(x=> x.StoreCode == storeCode && x.StorePosCode == storePosCode);
            if(obj != null)
            {
                Context.StorePoses.Remove(obj);
                Context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Terminal", obj.StorePosCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
