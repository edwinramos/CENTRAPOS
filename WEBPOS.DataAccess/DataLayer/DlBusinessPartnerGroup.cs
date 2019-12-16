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
    public class DlBusinessPartnerGroup
    {
        private WEBPOSContext context = new WEBPOSContext();
        public IEnumerable<DeBusinessPartnerGroup> ReadAll()
        {
            return context.BusinessPartnerGroups.ToList();
        }
        public IQueryable<DeBusinessPartnerGroup> ReadAllQueryable()
        {
            return context.BusinessPartnerGroups;
        }
        public IEnumerable<DeBusinessPartnerGroup> Read(DeBusinessPartnerGroup obj)
        {
            var data = context.BusinessPartnerGroups.ToList();

            if (!string.IsNullOrEmpty(obj.BusinessPartnerGroupCode))
                data = data.Where(x=>x.BusinessPartnerGroupCode == obj.BusinessPartnerGroupCode).ToList();

            if (!string.IsNullOrEmpty(obj.BusinessPartnerGroupDescription))
                data = data.Where(x => x.BusinessPartnerGroupDescription == obj.BusinessPartnerGroupDescription).ToList();

            return data;
        }

        public void Save(DeBusinessPartnerGroup obj)
        {
            var val = context.BusinessPartnerGroups.FirstOrDefault(x => x.BusinessPartnerGroupCode == obj.BusinessPartnerGroupCode);
            if (val != null)
            {
                val.BusinessPartnerGroupDescription = obj.BusinessPartnerGroupDescription;

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.UPDATE), "Tipo de Pago", obj.BusinessPartnerGroupCode)
                };
                BlActivityLog.Save(activity);
            }
            else
            {
                context.BusinessPartnerGroups.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Tipo de Pago", obj.BusinessPartnerGroupCode)
                };
                BlActivityLog.Save(activity);
            }
            context.SaveChanges();
        }

        public void Delete(string BusinessPartnerGroupCode)
        {
            var obj = context.BusinessPartnerGroups.FirstOrDefault(x=>x.BusinessPartnerGroupCode == BusinessPartnerGroupCode);
            if(obj != null)
            {
                context.BusinessPartnerGroups.Remove(obj);
                context.SaveChanges();
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Tipo de Pago", obj.BusinessPartnerGroupCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
