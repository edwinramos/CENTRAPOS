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
    public class DlBusinessPartner
    {
        private WEBPOSContext context = new WEBPOSContext();
        public IEnumerable<DeBusinessPartner> ReadAll()
        {
            return context.BusinessPartners.ToList();
        }
        public IQueryable<DeBusinessPartner> ReadAllQueryable()
        {
            return context.BusinessPartners;
        }
        public IEnumerable<DeBusinessPartner> Read(DeBusinessPartner obj)
        {
            var data = context.BusinessPartners.ToList();

            if (!string.IsNullOrEmpty(obj.BusinessPartnerCode))
                data = data.Where(x=>x.BusinessPartnerCode == obj.BusinessPartnerCode).ToList();

            if (!string.IsNullOrEmpty(obj.BusinessPartnerDescription))
                data = data.Where(x => x.BusinessPartnerDescription == obj.BusinessPartnerDescription).ToList();

            return data;
        }

        public void Save(DeBusinessPartner obj)
        {
            var val = context.BusinessPartners.FirstOrDefault(x => x.BusinessPartnerCode == obj.BusinessPartnerCode);
            if (val != null)
            {
                val.BusinessPartnerDescription = obj.BusinessPartnerDescription;
                val.BusinessPartnerType = obj.BusinessPartnerType;
                val.BusinessPartnerGroupCode = obj.BusinessPartnerGroupCode;
                val.PriceListCode = obj.PriceListCode;
                val.RNC = obj.RNC;

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.UPDATE), "Socio", obj.BusinessPartnerCode)
                };
                BlActivityLog.Save(activity);
            }
            else
            {
                context.BusinessPartners.Add(obj);

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Socio", obj.BusinessPartnerCode)
                };
                BlActivityLog.Save(activity);
            }
            context.SaveChanges();
        }

        public void Delete(string businessPartnerCode)
        {
            var obj = context.BusinessPartners.FirstOrDefault(x=>x.BusinessPartnerCode == businessPartnerCode);
            if(obj != null)
            {
                foreach (var contact in new DlBusinessPartnerContact().Read(new DeBusinessPartnerContact { BusinessPartnerCode = businessPartnerCode }))
                {
                    context.BusinessPartnerContacts.Remove(contact);
                }
                context.SaveChanges();

                context.BusinessPartners.Remove(obj);
                context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Socio", obj.BusinessPartnerCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
