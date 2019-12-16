using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlBusinessPartnerContact
    {
        public static IEnumerable<DeBusinessPartnerContact> ReadAll()
        {
            var dl = new DlBusinessPartnerContact();
            return dl.ReadAll();
        }
        public static IQueryable<DeBusinessPartnerContact> ReadAllQueryable()
        {
            var dl = new DlBusinessPartnerContact();
            return dl.ReadAllQueryable();
        }
        public static IEnumerable<DeBusinessPartnerContact> Read(DeBusinessPartnerContact obj)
        {
            var dl = new DlBusinessPartnerContact();
            return dl.Read(obj);
        }

        public static void Save(DeBusinessPartnerContact obj)
        {
            var dl = new DlBusinessPartnerContact();
            dl.Save(obj);
        }

        public static void Delete(DeBusinessPartnerContact obj)
        {
            var dl = new DlBusinessPartnerContact();
            dl.Delete(obj.BusinessPartnerCode, obj.BusinessPartnerContactCode);
        }
    }
}
