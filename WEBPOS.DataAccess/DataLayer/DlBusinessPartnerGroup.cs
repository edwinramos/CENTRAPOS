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
    public class DlBusinessPartnerGroup : BaseRepository<WEBPOSContext, DeBusinessPartnerGroup>
    {
        public DlBusinessPartnerGroup(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeBusinessPartnerGroup> ReadAll()
        {
            return Context.BusinessPartnerGroups.ToList();
        }
        public IQueryable<DeBusinessPartnerGroup> ReadAllQueryable()
        {
            return Context.BusinessPartnerGroups;
        }
        public IEnumerable<DeBusinessPartnerGroup> Read(DeBusinessPartnerGroup obj)
        {
            var data = Context.BusinessPartnerGroups.ToList();

            if (!string.IsNullOrEmpty(obj.BusinessPartnerGroupCode))
                data = data.Where(x=>x.BusinessPartnerGroupCode == obj.BusinessPartnerGroupCode).ToList();

            if (!string.IsNullOrEmpty(obj.BusinessPartnerGroupDescription))
                data = data.Where(x => x.BusinessPartnerGroupDescription == obj.BusinessPartnerGroupDescription).ToList();

            return data;
        }

        public void Save(DeBusinessPartnerGroup obj)
        {
            var val = Context.BusinessPartnerGroups.FirstOrDefault(x => x.BusinessPartnerGroupCode == obj.BusinessPartnerGroupCode);
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
                Context.BusinessPartnerGroups.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Tipo de Pago", obj.BusinessPartnerGroupCode)
                };
                BlActivityLog.Save(activity);
            }
            Context.SaveChanges();
        }

        public void Delete(string BusinessPartnerGroupCode)
        {
            var obj = Context.BusinessPartnerGroups.FirstOrDefault(x=>x.BusinessPartnerGroupCode == BusinessPartnerGroupCode);
            if(obj != null)
            {
                Context.BusinessPartnerGroups.Remove(obj);
                Context.SaveChanges();
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Tipo de Pago", obj.BusinessPartnerGroupCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
