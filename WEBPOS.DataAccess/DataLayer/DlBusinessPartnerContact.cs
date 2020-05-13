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
    public class DlBusinessPartnerContact : BaseRepository<WEBPOSContext, DeBusinessPartnerContact>
    {
        public DlBusinessPartnerContact(WEBPOSContext context = null) : base(context) { }
        public IEnumerable<DeBusinessPartnerContact> ReadAll()
        {
            return Context.BusinessPartnerContacts.ToList();
        }
        public IQueryable<DeBusinessPartnerContact> ReadAllQueryable()
        {
            return Context.BusinessPartnerContacts;
        }
        public IEnumerable<DeBusinessPartnerContact> Read(DeBusinessPartnerContact obj)
        {
            var data = Context.BusinessPartnerContacts.ToList();

            if (!string.IsNullOrEmpty(obj.BusinessPartnerCode))
                data = data.Where(x => x.BusinessPartnerCode == obj.BusinessPartnerCode).ToList();

            if (!string.IsNullOrEmpty(obj.BusinessPartnerContactCode))
                data = data.Where(x=>x.BusinessPartnerContactCode == obj.BusinessPartnerContactCode).ToList();

            if (!data.All(x=>x.BusinessPartnerCode == obj.BusinessPartnerCode))
                return new List<DeBusinessPartnerContact>();

            return data;
        }

        public void Save(DeBusinessPartnerContact obj)
        {
            var val = Context.BusinessPartnerContacts.FirstOrDefault(x => x.BusinessPartnerCode == obj.BusinessPartnerCode && x.BusinessPartnerContactCode == obj.BusinessPartnerContactCode);
            if (val != null)
            {
                val.ContactName = obj.ContactName;
                val.ContactTitle = obj.ContactTitle;
                val.Telephone1 = obj.Telephone1;
                val.Telephone2 = obj.Telephone2;
                val.Email = obj.Email;

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.UPDATE), "Contacto de Socio", obj.BusinessPartnerCode + "|" + obj.BusinessPartnerContactCode)
                };
                BlActivityLog.Save(activity);
            }
            else
            {
                Context.BusinessPartnerContacts.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Contacto de Socio", obj.BusinessPartnerCode + "|" + obj.BusinessPartnerContactCode)
                };
                BlActivityLog.Save(activity);
            }
            Context.SaveChanges();
        }

        public void Delete(string businessPartnerCode, string businessPartnerContactCode)
        {
            var obj = Context.BusinessPartnerContacts.FirstOrDefault(x=> x.BusinessPartnerCode == businessPartnerCode && x.BusinessPartnerContactCode == businessPartnerContactCode);
            if(obj != null)
            {
                Context.BusinessPartnerContacts.Remove(obj);
                Context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Contacto de Socio", obj.BusinessPartnerCode + "|" + obj.BusinessPartnerContactCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
