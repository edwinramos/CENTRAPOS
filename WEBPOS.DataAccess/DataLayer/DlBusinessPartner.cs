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
    public class DlBusinessPartner : BaseRepository<WEBPOSContext, DeBusinessPartner>
    {
        public DlBusinessPartner(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeBusinessPartner> ReadAll()
        {
            return Context.BusinessPartners.ToList();
        }
        public IQueryable<DeBusinessPartner> ReadAllQueryable()
        {
            return Context.BusinessPartners;
        }
        public IEnumerable<DeBusinessPartner> Read(DeBusinessPartner obj)
        {
            var data = Context.BusinessPartners.ToList();

            if (!string.IsNullOrEmpty(obj.BusinessPartnerCode))
                data = data.Where(x=>x.BusinessPartnerCode == obj.BusinessPartnerCode).ToList();

            if (!string.IsNullOrEmpty(obj.BusinessPartnerDescription))
                data = data.Where(x => x.BusinessPartnerDescription == obj.BusinessPartnerDescription).ToList();

            return data;
        }

        public void Save(DeBusinessPartner obj)
        {
            var val = Context.BusinessPartners.FirstOrDefault(x => x.BusinessPartnerCode == obj.BusinessPartnerCode);
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
                Context.BusinessPartners.Add(obj);

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Socio", obj.BusinessPartnerCode)
                };
                BlActivityLog.Save(activity);
            }
            Context.SaveChanges();
        }

        public void Delete(string businessPartnerCode)
        {
            var obj = Context.BusinessPartners.FirstOrDefault(x=>x.BusinessPartnerCode == businessPartnerCode);
            if(obj != null)
            {
                foreach (var contact in new DlBusinessPartnerContact().Read(new DeBusinessPartnerContact { BusinessPartnerCode = businessPartnerCode }))
                {
                    Context.BusinessPartnerContacts.Remove(contact);
                }
                Context.SaveChanges();

                Context.BusinessPartners.Remove(obj);
                Context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Socio", obj.BusinessPartnerCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
