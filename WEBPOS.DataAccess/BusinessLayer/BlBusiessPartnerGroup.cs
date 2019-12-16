using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlBusinessPartnerGroup
    {
        public static IEnumerable<DeBusinessPartnerGroup> ReadAll()
        {
            var dl = new DlBusinessPartnerGroup();
            return dl.ReadAll();
        }
        public static IQueryable<DeBusinessPartnerGroup> ReadAllQueryable()
        {
            var dl = new DlBusinessPartnerGroup();
            return dl.ReadAllQueryable();
        }
        public static IEnumerable<DeBusinessPartnerGroup> Read(DeBusinessPartnerGroup obj)
        {
            var dl = new DlBusinessPartnerGroup();
            return dl.Read(obj);
        }

        public static void Save(DeBusinessPartnerGroup obj)
        {
            var dl = new DlBusinessPartnerGroup();
            dl.Save(obj);
        }

        public static void Delete(DeBusinessPartnerGroup obj)
        {
            var dl = new DlBusinessPartnerGroup();
            dl.Delete(obj.BusinessPartnerGroupCode);
        }
    }
}
