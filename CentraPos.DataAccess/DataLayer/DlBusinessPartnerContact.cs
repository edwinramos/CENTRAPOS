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
    public class DlBusinessPartnerContact
    {
        private CentraPosDBContext context = new CentraPosDBContext();
        public IEnumerable<DeBusinessPartnerContact> ReadAll()
        {
            return context.BusinessPartnerContacts.ToList();
        }
        public IQueryable<DeBusinessPartnerContact> ReadAllQueryable()
        {
            return context.BusinessPartnerContacts;
        }
        public IEnumerable<DeBusinessPartnerContact> Read(DeBusinessPartnerContact obj)
        {
            var data = context.BusinessPartnerContacts.ToList();

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
            var val = context.BusinessPartnerContacts.FirstOrDefault(x => x.BusinessPartnerCode == obj.BusinessPartnerCode && x.BusinessPartnerContactCode == obj.BusinessPartnerContactCode);
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
                context.BusinessPartnerContacts.Add(obj);
                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.CREATE), "Contacto de Socio", obj.BusinessPartnerCode + "|" + obj.BusinessPartnerContactCode)
                };
                BlActivityLog.Save(activity);
            }
            context.SaveChanges();
        }

        public void Delete(string businessPartnerCode, string businessPartnerContactCode)
        {
            var obj = context.BusinessPartnerContacts.FirstOrDefault(x=> x.BusinessPartnerCode == businessPartnerCode && x.BusinessPartnerContactCode == businessPartnerContactCode);
            if(obj != null)
            {
                context.BusinessPartnerContacts.Remove(obj);
                context.SaveChanges();

                var activity = new DeActivityLog
                {
                    ActivityMessage = string.Format(ActivityLogHelper.GetActivityText(LogActivities.DELETE), "Contacto de Socio", obj.BusinessPartnerCode + "|" + obj.BusinessPartnerContactCode)
                };
                BlActivityLog.Save(activity);
            }
        }
    }
}
